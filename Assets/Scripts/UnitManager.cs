using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UnitManager : MonoBehaviour
{
    /*
     * The unit manager will handle all unit information and processes
     */
    public int MAP_SIZE { set; get; }
    public float TILE_SIZE { set; get; }
    public float TILE_OFFSET { set; get; }
    
    //List of unit prefabs that can be spawned
    public List<GameObject> unitPrefabs;
    //List of units that have been spawned in the battle so far
    private List<GameObject> activeUnits;
    //Grid representation of current unit locations, with each unit having a unique id and 0 being no unit
    private int[,] units { set; get; }

    private void Start()
    {
        SpawnAllUnits();
    }

    //refreshes each unit belonging to player p
    public void ResetPlayer(int p)
    {
        foreach (GameObject u in activeUnits)
        {
            if (u.GetComponent<Unit>().Team == p)
            {
                u.GetComponent<Unit>().RefreshUnit();
            }
        }
    }

    //Currently spawns some preset units on the battlefield, needs  work to spawn selected units
    public void SpawnAllUnits()
    {
        activeUnits = new List<GameObject>();
        units = new int[MAP_SIZE, MAP_SIZE];

        for (int i = 5; i < MAP_SIZE - 5; i++)
        {
            SpawnUnit(0, i, 1, 0, 0);
        }
        for (int i = 5; i < MAP_SIZE - 5; i++)
        {
            SpawnUnit(1, i, MAP_SIZE - 2, 1, 180);
        }
    }

    //Spawns a unit of the given index at position x, y, with team t and rotation r
    public void SpawnUnit(int index, int x, int y, int t, float r)
    {
        //Instansiate the unit to the correct transform then add it to the activeUnits list
        GameObject go = Instantiate(unitPrefabs[index], GetTileCenter(x, y), Quaternion.Euler(0f, r, 0f)) as GameObject;
        go.transform.SetParent(transform);
        activeUnits.Add(go);
        units[x, y] = activeUnits.Count();

        //Initialise Unit paramaters
        GetUnit(units[x, y]).id = activeUnits.Count();
        GetUnit(units[x, y]).Team = t;
        GetUnit(units[x, y]).SetPosition(x, y);
    }

    //Moves a give unit to positon x, y using the MoveUnit event
    public void MoveUnit(Unit selectedUnit, MoveNode[,] moveGrid, int x, int y)
    {
        if (moveGrid[x, y].code == 0)
        {
            return;
        }

        bool willAttack = false;
        int enemyX = 0;
        int enemyY = 0;
        if (moveGrid[x, y].code == 2)
        {
            enemyX = x;
            enemyY = y;
            int tempx = x;
            x = moveGrid[x, y].previous.x;
            y = moveGrid[tempx, y].previous.y;
            willAttack = true;
        }
        MoveNode node = moveGrid[x, y];
        MoveNode preNode = moveGrid[x, y].previous;
        List<MoveUnit> moveSet = new List<MoveUnit>();
        MoveUnit move;

        while (node.code != 5)
        {
            move = new MoveUnit();
            move.target = selectedUnit.gameObject;
            move.startPosition = GetTileCenter(preNode.x, preNode.y);
            move.endPosition = GetTileCenter(node.x, node.y);
            moveSet.Add(move);
            node = preNode;
            preNode = node.previous;
        }
        if (moveGrid[x, y].code == 1)
        {
            moveSet.Reverse();
            GameManager.Instance.activeEventManager.pushEvents(moveSet.ToArray());

            units[selectedUnit.currentX, selectedUnit.currentY] = 0;
            selectedUnit.SetPosition(x, y);
            units[x, y] = selectedUnit.id;
            GetUnit(units[x, y]).moveRem -= moveGrid[x, y].dist;
        }
        if (willAttack)
        {
            Combat c = new Combat();
            c.TargetA = GetUnit(units[x, y]);
            c.TargetB = GetUnit(units[enemyX, enemyY]);
            GameManager.Instance.activeEventManager.pushEvent(c);
            GetUnit(units[x, y]).moveRem = 0;
        }
    }

    // Returns a MoveNode array with the possible move locations for a given unit,
    // The MoveNode contains the type of move available as well as the previous node
    public MoveNode[,] PossibleMove(Unit selectedUnit)
    {
        Unit c;
        int code;
        int x;
        int y;
        int dist = 0;
        int i = 0;

        MoveNode[,] r = new MoveNode[MAP_SIZE, MAP_SIZE];
        for (int j = 0; j < MAP_SIZE; j++)
        {
            for (int k = 0; k < MAP_SIZE; k++)
            {
                r[j, k] = new MoveNode();
            }
        }

        SearchNode start = new SearchNode(selectedUnit.currentX, selectedUnit.currentY, 0);
        List<SearchNode> Open_List = new List<SearchNode>();
        List<SearchNode> Closed_List = new List<SearchNode>();
        Open_List.Add(start);

        while (Open_List.Any() && i < 10000)
        {
            i++;
            x = Open_List[0].x;
            y = Open_List[0].y;
            dist = Open_List[0].dist;

            c = GetUnit(units[x, y]);

            code = 0;
            if (c == null)
            {
                code = 1;
            }
            else if (c.Team != selectedUnit.Team)
            {
                code = 2;
            }
            else if (selectedUnit.currentX == x && selectedUnit.currentY == y)
            {
                code = 5;
            }
            r[x, y].code = code;
            r[x, y].dist = dist;
            r[x, y].x = x;
            r[x, y].y = y;

            if ((code == 1 || code == 5) && dist < selectedUnit.moveRem)
            {
                if (x + 1 < MAP_SIZE)
                {
                    start = new SearchNode(x + 1, y, dist + 1);
                    if (!(Closed_List.Contains(start)) && !(Open_List.Contains(start)))
                    {
                        Open_List.Add(start);
                        r[x + 1, y].previous = r[x, y];
                    }

                }
                if (x - 1 >= 0)
                {
                    start = new SearchNode(x - 1, y, dist + 1);
                    if (!(Closed_List.Contains(start)) && !(Open_List.Contains(start)))
                    {
                        Open_List.Add(start);
                        r[x - 1, y].previous = r[x, y];
                    }
                }
                if (y + 1 < MAP_SIZE)
                {
                    start = new SearchNode(x, y + 1, dist + 1);
                    if (!(Closed_List.Contains(start)) && !(Open_List.Contains(start)))
                    {
                        Open_List.Add(start);
                        r[x, y + 1].previous = r[x, y];
                    }
                }
                if (y - 1 >= 0)
                {
                    start = new SearchNode(x, y - 1, dist + 1);
                    if (!(Closed_List.Contains(start)) && !(Open_List.Contains(start)))
                    {
                        Open_List.Add(start);
                        r[x, y - 1].previous = r[x, y];
                    }
                }
            }
            Closed_List.Add(Open_List[0]);
            Open_List.Remove(Open_List[0]);
        }
        return r;
    }

    //Returns a vector three of the center of a given coordinate, including its height
    public Vector3 GetTileCenter(int x, int y)
    {
        Vector3 origin = Vector3.zero;
        origin.x += (TILE_SIZE * x) + TILE_OFFSET;
        origin.z += (TILE_SIZE * y) + TILE_OFFSET;
        origin.y += (GameManager.Instance.activeBoardManager.GetHeight(x, y));
        return origin;
    }

    // Checks every unit on the battlefield and deactivates it if health remaining is
    // Less then zero
    private void CheckUnitDeaths()
    {
        for (int i = 0; i < MAP_SIZE; i++)
        {
            for (int j = 0; j < MAP_SIZE; j++)
            {
                if (units[i, j] != 0)
                {
                    if (GetUnit(units[i, j]).healthRem <= 0)
                    {
                        GetUnit(units[i, j]).healthRem = 0;
                        GetUnit(units[i, j]).gameObject.SetActive(false);
                        units[i, j] = 0;
                    }
                }
            }
        }
    }

    // Returns the unit with id n
    private Unit GetUnit(int n)
    {
        if (n < 1)
        {
            return null;
        }
        return activeUnits[n - 1].GetComponent<Unit>();
    }

    //Returns the unit at position x, y
    public Unit GetUnitAt(int x, int y)
    {
        return (GetUnit(units[x, y]));
    }
}

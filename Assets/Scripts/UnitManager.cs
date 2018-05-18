using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class UnitManager : MonoBehaviour
{
    public int MAP_SIZE { set; get; }
    public float TILE_SIZE { set; get; }
    public float TILE_OFFSET { set; get; }

    private int selectedX = -1;
    private int selectedY = -1;
    private const float hitRange = 50f;

    public Text HealthText;
    public Text MoveText;
    public Text AttackText;
    public Text DefenceText;

    public GameObject Plane;
    public List<GameObject> unitPrefabs;
    private List<GameObject> activeUnits;
    public int[,] units { set; get; }

    private MoveNode[,] allowedMoves { set; get; }

    private Unit selectedUnit;



    private void Start()
    {
        SpawnAllUnits();
        Vector3 h = new Vector3();
        h.x = (MAP_SIZE / 2);
        h.z = (MAP_SIZE / 2);
        this.Plane.transform.localPosition = h;
        this.Plane.transform.localScale = new Vector3(MAP_SIZE, 0, MAP_SIZE);

        HealthText = GameObject.Find("HealthNumb").GetComponent<Text>();
        MoveText = GameObject.Find("MoveNumb").GetComponent<Text>();
        AttackText = GameObject.Find("AttackNumb").GetComponent<Text>();
        DefenceText = GameObject.Find("DefenceNumb").GetComponent<Text>();
    }

    private void Update()
    {
        UpdateSelection();
        DrawSelection();
        if (Time.time >= 2)
            CheckUnitDeaths();

        if (Input.GetMouseButtonDown(0))
        {
            if (selectedX >= 0 && selectedY >= 0)
            {
                SelectUnit(selectedX, selectedY);
            }
        }
        if (Input.GetMouseButtonDown(1))
        {
            if (selectedX >= 0 && selectedY >= 0 && selectedUnit != null)
            {
                MoveUnit(selectedX, selectedY);
            }
        }
    }

    public void ResetPlayer(int p)
    {
        DeselectUnit();
        foreach (GameObject u in activeUnits)
        {
            if (u.GetComponent<Unit>().Team == p)
            {
                u.GetComponent<Unit>().RefreshUnit();
            }
        }
    }

    private void SpawnAllUnits()
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

    private void SpawnUnit(int index, int x, int y, int t, float a)
    {
        //Instansiate the unit to the correct transform then add it to the activeUnits list
        GameObject go = Instantiate(unitPrefabs[index], GetTileCenter(x, y), Quaternion.Euler(0f, a, 0f)) as GameObject;
        go.transform.SetParent(transform);
        activeUnits.Add(go);
        units[x, y] = activeUnits.Count();

        //Initialise Unit paramaters
        getUnit(units[x, y]).id = activeUnits.Count();
        getUnit(units[x, y]).Team = t;
        getUnit(units[x, y]).SetPosition(x, y);
    }

    private void SelectUnit(int x, int y)
    {
        DeselectUnit();
        if (getUnit(units[x, y]) == null)
        {
            return;
        }
        else if (getUnit(units[x, y]).Team != GameManager.Instance.playerTurn)
        {
            return;
        }

        DeselectUnit();
        selectedUnit = getUnit(units[x, y]);
        allowedMoves = PossibleMove();
        Board_Highlights.Instance.HighLightAllowedMoves(allowedMoves);
        HealthText.text = (selectedUnit.healthRem).ToString() + "/" + (selectedUnit.Health).ToString();
        MoveText.text = ((selectedUnit.moveRem).ToString() + "/" + (selectedUnit.Move).ToString());
        AttackText.text = (selectedUnit.AttackLow).ToString() + "-" + (selectedUnit.AttackHigh).ToString();
        DefenceText.text = (selectedUnit.Defence).ToString();
    }

    private void DeselectUnit()
    {
        Board_Highlights.Instance.HideHighlights();
        selectedUnit = null;
        HealthText.text = "";
        MoveText.text = "";
        AttackText.text = "";
        DefenceText.text = "";
    }

    private void MoveUnit(int x, int y)
    {
        if (allowedMoves[x, y].code == 0)
        {
            return;
        }

        bool willAttack = false;
        int enemyX = 0;
        int enemyY = 0;
        if (allowedMoves[x, y].code == 2)
        {
            enemyX = x;
            enemyY = y;
            int tempx = x;
            x = allowedMoves[x, y].previous.x;
            y = allowedMoves[tempx, y].previous.y;
            willAttack = true;
        }
        MoveNode node = allowedMoves[x, y];
        MoveNode preNode = allowedMoves[x, y].previous;
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
        if (allowedMoves[x, y].code == 1)
        {
            moveSet.Reverse();
            GameManager.Instance.activeEventManager.pushEvents(moveSet.ToArray());

            units[selectedUnit.currentX, selectedUnit.currentY] = 0;
            selectedUnit.SetPosition(x, y);
            units[x, y] = selectedUnit.id;
            getUnit(units[x, y]).moveRem -= allowedMoves[x, y].dist;
        }
        if (willAttack)
        {
            Combat c = new Combat();
            c.TargetA = getUnit(units[x, y]);
            c.TargetB = getUnit(units[enemyX, enemyY]);
            GameManager.Instance.activeEventManager.pushEvent(c);
            getUnit(units[x, y]).moveRem = 0;
        }
        SelectUnit(x, y);
    }

    private void DrawSelection()
    {
        if (selectedX >= 0 && selectedY >= 0)
        {
            Debug.DrawLine(
                Vector3.forward * selectedY + Vector3.right * selectedX,
                Vector3.forward * (selectedY + 1) + Vector3.right * (selectedX + 1));
            Debug.DrawLine(
                 Vector3.forward * (selectedY + 1) + Vector3.right * selectedX,
                 Vector3.forward * selectedY + Vector3.right * (selectedX + 1));
        }
    }

    private void UpdateSelection()
    {
        if (!Camera.main)
        {
            return;
        }

        RaycastHit hit;
        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, hitRange, LayerMask.GetMask("BoardPlane")))
        {
            selectedX = (int)hit.point.x;
            selectedY = (int)hit.point.z;
        }
        else
        {
            selectedX = -1;
            selectedY = -1;
        }
    }

    private MoveNode[,] PossibleMove()
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

        SearchNode start = new SearchNode(selectedX, selectedY, 0);
        List<SearchNode> Open_List = new List<SearchNode>();
        List<SearchNode> Closed_List = new List<SearchNode>();
        Open_List.Add(start);

        while (Open_List.Any() && i < 10000)
        {
            i++;
            x = Open_List[0].x;
            y = Open_List[0].y;
            dist = Open_List[0].dist;

            c = getUnit(units[x, y]);

            code = 0;
            if (c == null)
            {
                code = 1;
            }
            else if (c.Team != selectedUnit.Team)
            {
                code = 2;
            }
            else if (selectedX == x && selectedY == y)
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

    public Vector3 GetTileCenter(int x, int y)
    {
        Vector3 origin = Vector3.zero;
        origin.x += (TILE_SIZE * x) + TILE_OFFSET;
        origin.z += (TILE_SIZE * y) + TILE_OFFSET;
        origin.y += (GameManager.Instance.activeBoardManager.getHeight(x, y));
        return origin;
    }

    private void CheckUnitDeaths()
    {
        for (int i = 0; i < MAP_SIZE; i++)
        {
            for (int j = 0; j < MAP_SIZE; j++)
            {
                if (units[i, j] != 0)
                {
                    if (getUnit(units[i, j]).healthRem <= 0)
                    {
                        getUnit(units[i, j]).healthRem = 0;
                        getUnit(units[i, j]).gameObject.SetActive(false);
                        units[i, j] = 0;
                    }
                }
            }
        }
    }

    // Returns the unit at position n in the activeUnits list
    private Unit getUnit(int n)
    {
        if (n < 1)
        {
            return null;
        }
        return activeUnits[n - 1].GetComponent<Unit>();
    }
}

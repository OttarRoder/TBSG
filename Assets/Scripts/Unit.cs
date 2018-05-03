using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public struct T_val
{
    //Structure to hold the x and y coordinate of a position as will as a distance from an origin point
    public int x;
    public int y;
    public int dist;

    public T_val(int p1, int p2, int p3)
    {
        x = p1;
        y = p2;
        dist = p3;
    }

    public override bool Equals(object obj)
    {
        if (obj is T_val )
        {
            T_val c = (T_val)obj;
            return x == c.x && y == c.y && dist <= c.dist;
        }
        else
        {
            return false;
        }
    }

    public override int GetHashCode()
    {
        return x.GetHashCode() ^ y.GetHashCode() ^ dist.GetHashCode();
    }
}

public abstract class Unit : MonoBehaviour
{
    //The unit class should eventualy hold just the template of a game unit, current work includes
    //moving the possible move function to the unit manager
    private const int MAP_SIZE = 20;

    public int currentX { set; get; }
    public int currentY { set; get; }
    public int Team { set; get; }
    public int moveRem { set; get; }
    public int healthRem { set; get; }

    public int Move = 5;
    public int Attack = 5;
    public float Defense = 0.5f;
    public int Health = 10;

    private void Start()
    {
        moveRem = Move;
        healthRem = Health;
    }

    public void SetPosition(int x, int y)
    {
        currentX = x;
        currentY = y;
    }

    public virtual int[,] PossibleMove()
    {
        Unit c;
        int code;
        int x;
        int y;
        int dist = 0;
        int i = 0;

        int[,] r = new int[MAP_SIZE, MAP_SIZE];

        T_val start = new T_val(currentX, currentY, 0);
        List<T_val> Open_List = new List<T_val>();
        List<T_val> Closed_List = new List<T_val>();
        Open_List.Add(start);

        while (Open_List.Any() && i<10000)
        {
            i++;
            x = Open_List[0].x;
            y = Open_List[0].y;
            dist = Open_List[0].dist;

            c = UnitManager.instance.units[x, y];

            code = 0;
            if(c == null)
            {
                code = 1;
            }
            else if(c.Team != Team)
            {
                code = 2;
            }
            else if(currentX == x && currentY == y)
            {
                code = 5;
            }
            r[x, y] = code;

            if ((code == 1 || code == 5) && dist < moveRem)
            {
                if (x + 1 < MAP_SIZE)
                {
                    start = new T_val(x + 1, y, dist + 1);
                    if (!(Closed_List.Contains(start)) && !(Open_List.Contains(start)))
                        Open_List.Add(start);
                }
                if (x - 1 >= 0)
                {
                    start = new T_val(x - 1, y, dist + 1);
                    if (!(Closed_List.Contains(start)) && !(Open_List.Contains(start)))
                        Open_List.Add(start);
                }
                if (y + 1 < MAP_SIZE)
                {
                    start = new T_val(x, y + 1, dist + 1);
                    if (!(Closed_List.Contains(start)) && !(Open_List.Contains(start)))
                        Open_List.Add(start);
                }
                if (y - 1 >= 0)
                {
                    start = new T_val(x, y - 1, dist + 1);
                    if (!(Closed_List.Contains(start)) && !(Open_List.Contains(start)))
                        Open_List.Add(start);
                }
            }
            Closed_List.Add(Open_List[0]);
            Open_List.Remove(Open_List[0]);
        }
        return r;
    }

    public virtual void RefreshUnit()
    {
        moveRem = Move;
    }
}

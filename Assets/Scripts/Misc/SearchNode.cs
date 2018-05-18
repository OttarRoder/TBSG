using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SearchNode {
    //Structure to hold the x and y coordinate of a position as will as a distance from an origin point
    public int x;
    public int y;
    public int dist;

    public SearchNode(int p1, int p2, int p3)
    {
        x = p1;
        y = p2;
        dist = p3;
    }

    public override bool Equals(object obj)
    {
        if (obj is SearchNode)
        {
            SearchNode c = (SearchNode)obj;
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

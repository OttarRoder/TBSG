using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    public int Defence = 2;
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

    public virtual void RefreshUnit()
    {
        moveRem = Move;
    }
}

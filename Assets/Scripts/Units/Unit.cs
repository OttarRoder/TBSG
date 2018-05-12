using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Unit : MonoBehaviour
{
    public int currentX { set; get; }
    public int currentY { set; get; }
    public int Team { set; get; }
    public int moveRem { set; get; }
    public int healthRem { set; get; }

    public int Move = 5;
    public int Defence = 5;
    public int Health = 25;

    public int AttackLow = 4;
    public int AttackHigh = 7;
    public float AttackSpeed = 1;

    public float CritChance = 0.1f;
    public float CritMult = 1.5f;

    private void Start()
    {
        moveRem = Move;
        healthRem = Health;
    }

    private void Update()
    {
        if (UnitManager.Instance.GetTileCenter(currentX, currentY) == this.gameObject.transform.position)
        {
            this.gameObject.GetComponent<Animator>().SetBool("IsMoving",false);
        }
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

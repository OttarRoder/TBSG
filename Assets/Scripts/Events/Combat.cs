using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Combat : Event
{
    public Unit targetA;
    public Unit targetB;
	
	public override bool Run ()
    {
        targetB.healthRem -= (targetA.Attack - targetB.Defense);
        targetA.healthRem -= (targetB.Attack - targetA.Defense);    
        return true;
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Combat : Event
{
    public Unit targetA;
    public Unit targetB;
	
	public override bool Run ()
    {
        targetB.healthRem -= (targetA.Attack - targetB.Defence);
        targetA.healthRem -= (targetB.Attack - targetA.Defence);    
        return true;
	}
}

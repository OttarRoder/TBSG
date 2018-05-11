using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Combat : Event
{
    public Unit TargetA;
    public Unit TargetB;

    private int AttackA;
    private int AttackB;
    private bool CritA;
    private bool CritB;

    public override void Initiate()
    {
        AttackA = Random.Range(TargetA.AttackLow, TargetA.AttackHigh);
        AttackB = Random.Range(TargetB.AttackLow, TargetB.AttackHigh);
        CritA = Random.value <= TargetA.CritChance;
        CritB = Random.value <= TargetB.CritChance;
        if(CritA)
        {
            AttackA *= (int)(TargetA.CritMult);
        }
        if(CritB)
        {
            AttackB *= (int)(TargetB.CritMult);
        }
        
    }

    public override bool Run ()
    {
        TargetB.healthRem -= (AttackA - TargetB.Defence);
        TargetA.healthRem -= (AttackB - TargetA.Defence);
        TargetA.gameObject.GetComponent<Animator>().SetTrigger("Attack");
        TargetB.gameObject.GetComponent<Animator>().SetTrigger("Attack");
        return true;
	}
}

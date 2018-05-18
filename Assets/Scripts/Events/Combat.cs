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
        float HeightA = GameManager.Instance.activeBoardManager.GetHeight(TargetA.currentX, TargetA.currentY);
        float HeightB = GameManager.Instance.activeBoardManager.GetHeight(TargetB.currentX, TargetB.currentY);

        if (CritA)
        {
            AttackA = (int)(TargetA.CritMult * AttackA);
        }
        if(CritB)
        {
            AttackB = (int)(TargetB.CritMult * AttackB);
        }
        if (HeightA > HeightB)
        {
            AttackA = (int)(AttackA * 1.2f);
        }
        else if (HeightB > HeightA)
        {
            AttackB = (int)(AttackB * 1.2f);
        }

    }

    public override bool Run ()
    {
        TargetA.transform.LookAt(TargetB.transform);
        TargetB.transform.LookAt(TargetA.transform);
        TargetA.gameObject.GetComponent<AudioSource>().Play();
        TargetB.gameObject.GetComponent<AudioSource>().PlayDelayed(Random.Range(0, 0.3f));
        TargetB.healthRem -= (AttackA - TargetB.Defence);
        Debug.Log(AttackA - TargetB.Defence);
        TargetA.healthRem -= (AttackB - TargetA.Defence);
        Debug.Log(AttackB - TargetA.Defence);
        TargetA.gameObject.GetComponent<Animator>().SetTrigger("Attack");
        TargetB.gameObject.GetComponent<Animator>().SetTrigger("Attack");
        return true;
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartAnimation : Event
{
    private Unit Target;
    private string Anim;

    public StartAnimation(Unit t, string s)
    {
        Target = t;
        Anim = s;
    }

    public override void Initiate()
    {
        Target.GetComponent<Animator>().SetBool(Anim, true);
    }
}

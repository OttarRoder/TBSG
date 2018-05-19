using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndAnimation : Event
{
    private Unit Target;
    private string Anim;

    public EndAnimation(Unit t, string s)
    {
        Target = t;
        Anim = s;
    }

    public override void Initiate()
    {
        Target.GetComponent<Animator>().SetBool(Anim, false);
    }
}

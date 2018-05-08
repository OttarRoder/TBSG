using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Event
{
    public virtual void Initiate(){}
    public virtual bool Run()
    {
        return true;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Event : MonoBehaviour
{
    public bool initiate()
    {
        function();
        return true;
    }

    protected virtual void function(){}
}

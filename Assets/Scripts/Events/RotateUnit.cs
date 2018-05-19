using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateUnit : Event
{
    private Vector3 Direction;
    private GameObject Target;

    public RotateUnit(Vector3 r, GameObject go)
    {
        Direction = r;
        Target = go;
    }

    public override void Initiate()
    {
        if(Mathf.Abs(Direction.x) > Mathf.Abs(Direction.z))
        {
            if(Direction.x > 0)
            {
                Target.transform.rotation = Quaternion.Euler(0, 270f, 0);
            }
            else if(Direction.x < 0)
            {
                Target.transform.rotation = Quaternion.Euler(0, 90f, 0);
            }
        }
        else if(Mathf.Abs(Direction.z) > Mathf.Abs(Direction.x))
        {
            if(Direction.z > 0)
            {
                Target.transform.rotation = Quaternion.Euler(0, 180f, 0);
            }
            else if(Direction.z < 0)
            {
                Target.transform.rotation = Quaternion.Euler(0, 0, 0);
            }
        }
    }
}

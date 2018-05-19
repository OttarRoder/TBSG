using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveUnit : Event
{
    public Vector3 StartPosition { set; get; }
    public Vector3 EndPosition { set; get; }
    public GameObject Target;
    public float speed = 3.0f;

    private float startTime;
    private float journeyLength;

    public MoveUnit(Vector3 start, Vector3 end, GameObject t)
    {
        Target = t;
        StartPosition = start;
        EndPosition = end;
    }

    public override void Initiate()
    {
        startTime = Time.time;
        journeyLength = Vector3.Distance(StartPosition, EndPosition);
        Vector3 temp = EndPosition;
        temp.y = StartPosition.y;
    }

    public override bool Run()
    {
        float distCovered = (Time.time - startTime) * speed;
        float fracJourney = distCovered / journeyLength;
        Target.transform.position = Vector3.Lerp(StartPosition, EndPosition, fracJourney);

        if (Target.transform.position == EndPosition)
        {
            return true;
        }
        return false;
    }
}


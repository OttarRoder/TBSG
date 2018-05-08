using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveUnit : Event
{
    public Vector3 startPosition { set; get; }
    public Vector3 endPosition { set; get; }
    public GameObject target;
    public float speed = 5.0f;

    private float startTime;
    private float journeyLength;

    public override void Initiate()
    {
        startTime = Time.time;
        journeyLength = Vector3.Distance(startPosition, endPosition);
    }

    public override bool Run()
    {
        float distCovered = (Time.time - startTime) * speed;
        float fracJourney = distCovered / journeyLength;
        target.transform.position = Vector3.Lerp(startPosition, endPosition, fracJourney);
        if (target.transform.position == endPosition)
        {
            return true;
        }
        return false;
    }
}


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveUnit : Event
{
    public Vector3 startPosition { set; get; }
    public Vector3 endPosition { set; get; }
    public Unit target { set; get; }
    public float speed = 5.0f;

    private bool active = false;
    private float startTime;
    private float journeyLength;

    protected override void function()
    {
        startTime = Time.time;
        journeyLength = Vector3.Distance(startPosition, endPosition);
        active = true;
    }

    private void Update()
    {
        if(active)
        {
            float distCovered = (Time.time - startTime) * speed;
            float fracJourney = distCovered / journeyLength;
            target.transform.position = Vector3.Lerp(startPosition, endPosition, fracJourney);
        }
    }
}


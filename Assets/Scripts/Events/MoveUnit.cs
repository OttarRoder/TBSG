using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveUnit : Event
{
    public Vector3 startPosition { set; get; }
    public Vector3 endPosition { set; get; }
    public GameObject target;
    public float speed = 3.0f;

    private float startTime;
    private float journeyLength;

    public override void Initiate()
    {
        startTime = Time.time;
        journeyLength = Vector3.Distance(startPosition, endPosition);
        Vector3 temp = endPosition;
        temp.y = startPosition.y;
        target.transform.LookAt(temp);
    }

    public override bool Run()
    {
        target.GetComponent<Animator>().SetBool("IsMoving", true);
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


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveSet : Event
{
    private Unit Unit;
    private Vector3 Facing;
    private List<Vector3> Positions;
    private List<Event> eventList;

    public MoveSet(Unit _unit, List<Vector3> _positions, Vector3 _facing)
    {
        Name = "MoveSet";

        Unit = _unit;
        Positions = _positions;
        Facing = _facing;
    }

    public override void Initiate()
    {
        MoveUnit tempm;
        RotateUnit tempr;
        eventList = new List<Event>();

        for(int i = 0; i < Positions.Count-1; i +=2)
        {
            tempm = new MoveUnit(Positions[i + 1], Positions[i], Unit.gameObject);
            eventList.Add(tempm);
            tempr = new RotateUnit(Positions[i + 1] - Positions[i], Unit.gameObject);
            eventList.Add(tempr);
        }
    }

    public override bool Run()
    {
        StartAnimation sanim = new StartAnimation(Unit, Unit.moveAnimation);
        EndAnimation eanim = new EndAnimation(Unit, Unit.moveAnimation);

        GameManager.Instance.activeEventManager.pushTrigger(new RotateUnit(Facing, Unit.gameObject));
        GameManager.Instance.activeEventManager.pushTrigger(eanim);
        GameManager.Instance.activeEventManager.pushTriggers(eventList.ToArray());
        GameManager.Instance.activeEventManager.pushTrigger(sanim);
        return true;
    }
}

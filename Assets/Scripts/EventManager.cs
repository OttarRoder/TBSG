using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    /*
     * The purpose of this class is to sequentialy handle in game events,
     * events in the trigger stack are handled first followed by events in
     * the event queue.
    */

    public bool Empty { set; get; }
    public bool NotActive { set; get; }

    private Queue<Event> EventQueue;
    private Stack<Event> TriggerStack;
    private Event CurrentEvent;
    void Start ()
    {
        Empty = true;
        NotActive = true;
        EventQueue = new Queue<Event>();
        TriggerStack = new Stack<Event>();
	}
	
	void Update ()
    {
        Empty = isEmpty();
        if(!Empty && NotActive)
        {
            initiateNext();
        }
        if(CurrentEvent != null)
        {
            NotActive = CurrentEvent.Run();
            if(NotActive)
            {
                CurrentEvent = null;
            }
        }
	}

    public void pushEvent(Event a)
    {
        EventQueue.Enqueue(a);
    }

    public void pushEvents(Event[] a)
    {
        for(int i = 0; i < a.Length; i++)
        {
            EventQueue.Enqueue(a[i]);
        }
    }

    public void pushTrigger(Event a)
    {
        TriggerStack.Push(a);
    }

    public void pushTriggers(Event[] a)
    {
        for(int i = 0; i < a.Length; i++)
        {
            TriggerStack.Push(a[i]);
        }
    }


    private bool isEmpty()
    {
        if(EventQueue.Count == 0 && TriggerStack.Count == 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private void initiateNext()
    {
        if(TriggerStack.Count > 0)
        {
            CurrentEvent = TriggerStack.Pop();
        }
        else if(EventQueue.Count > 0)
        {
            CurrentEvent = EventQueue.Dequeue();
        }
        NotActive = false;
        CurrentEvent.Initiate();
    }
}

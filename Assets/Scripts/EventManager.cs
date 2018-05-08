using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    public static EventManager Instance { set; get; }
    /*
     * The purpose of this class is to sequentialy handle in game events,
     * events in the trigger stack are handled first followed by events in
     * the event queue.
    */
    public bool empty { set; get; }
    public bool notActive { set; get; }

    private Queue<Event> eventQueue;
    private Stack<Event> triggerStack;
    private Event currentEvent;
    void Start ()
    {
        Instance = this;
        empty = true;
        notActive = true;
        eventQueue = new Queue<Event>();
        triggerStack = new Stack<Event>();
	}
	
	void Update ()
    {
        empty = isEmpty();
        if(!empty && notActive)
        {
            initiateNext();
        }
        if(currentEvent != null)
        {
            notActive = currentEvent.Run();
            if(notActive)
            {
                currentEvent = null;
            }
        }
	}

    public void pushEvent(Event a)
    {
        eventQueue.Enqueue(a);
    }

    public void pushEvents(Event[] a)
    {
        for(int i = 0; i < a.Length; i++)
        {
            eventQueue.Enqueue(a[i]);
        }
    }

    public void pushTrigger(Event a)
    {
        triggerStack.Push(a);
    }

    public void pushTriggers(Event[] a)
    {
        for(int i = 0; i < a.Length; i++)
        {
            triggerStack.Push(a[i]);
        }
    }


    private bool isEmpty()
    {
        if(eventQueue.Count == 0 && triggerStack.Count == 0)
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
        if(triggerStack.Count > 0)
        {
            currentEvent = triggerStack.Pop();
        }
        else if(eventQueue.Count > 0)
        {
            currentEvent = eventQueue.Dequeue();
        }
        notActive = false;
        currentEvent.Initiate();
    }
}

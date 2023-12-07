using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    
    private Dictionary<Event, Action<IEventPacket>> eventDictionary;

    private static EventManager eventManager;
    public static EventManager Instance
    {
        get
        {
            if(!eventManager)
            {
                var go = new GameObject().AddComponent(typeof(EventManager));
                go.name = "EventManager";
                eventManager = go.GetComponent<EventManager>();//FindObjectOfType(typeof(EventManager)) as EventManager;
            }
            if(!eventManager)
            {
                Debug.LogError("There needs to be one active EventManager script on a GameObject in your scene!");
            }
            else
            {
                eventManager.Init();
                DontDestroyOnLoad(eventManager);
            }
            return eventManager;
        }
    }

    void Init()
    {
        if(eventDictionary == null)
        {
            eventDictionary = new Dictionary<Event, Action<IEventPacket>>();
        }
    }

    public static void StartListening(Event e, Action<IEventPacket> listener)
    {
        Action<IEventPacket> thisEvent;
        if(Instance.eventDictionary.TryGetValue(e, out thisEvent))
        {
            thisEvent += listener;
            Instance.eventDictionary[e] = thisEvent;
        }
        else
        {
            thisEvent += listener;
            Instance.eventDictionary.Add(e, thisEvent);
        }
    }

    public static void StopListening(Event e, Action<IEventPacket> listener)
    {
        if (eventManager == null)
            return;
        Action<IEventPacket> thisEvent;
        if(Instance.eventDictionary.TryGetValue(e, out thisEvent))
        {
            thisEvent -= listener;
            Instance.eventDictionary[e] = thisEvent;
        }
    }

    public static void TriggerEvent(Event e, IEventPacket packet)
    {
        Action<IEventPacket> thisEvent = null;
        if(Instance.eventDictionary.TryGetValue(e, out thisEvent))
        {
            if(thisEvent != null)
                thisEvent.Invoke(packet);
        }
    }




}

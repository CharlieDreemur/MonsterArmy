using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Sirenix.OdinInspector;
using System;

public class SEvent :  UnityEvent<string>{}
/// <summary>
/// 事件系统，负责整个游戏的event
/// </summary>
public class EventManager : Singleton<EventManager>
{
    [SerializeField]
    private Dictionary<string, SEvent>eventDictionary;
    [ShowInInspector]
    private static bool isInit = false;
    protected override void Awake()
    {
        base.Awake();
        eventDictionary = new Dictionary<string, SEvent>();
        isInit = true;
        Debug.LogWarning(Instance);
    }
    public static void StartListening(string eventName, UnityAction<string> listener){
        SEvent thisEvent = null;
        if(Instance.eventDictionary.TryGetValue(eventName, out thisEvent)){
            thisEvent.AddListener(listener);
        }
        else{
            thisEvent = new SEvent();
            thisEvent.AddListener(listener);
            Instance.eventDictionary.Add(eventName, thisEvent);
        }
    }   

    public static void StopListening(string eventName, UnityAction<string> listener){
        if(!isInit){
            return;
        }
        SEvent thisEvent = null;
        if(Instance.eventDictionary.TryGetValue(eventName, out thisEvent)){
            thisEvent.RemoveListener(listener);
        }
    }

    public static void TriggerEvent(string eventName, string value){
        SEvent thisEvent = null;
        if(Instance.eventDictionary.TryGetValue(eventName, out thisEvent)){
            thisEvent.Invoke(value);
        }
        else{
            Debug.LogWarning($"The event: {eventName} does not exist in the EventManager, value: {value}");
        }
    }


}

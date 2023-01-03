using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Sirenix.OdinInspector;

public class SEvent :  UnityEvent<string>{}
/// <summary>
/// 事件系统，负责整个游戏的event
/// </summary>
public class EventManager : Singleton<EventManager>
{
    [ShowInInspector]
    private Dictionary<string, SEvent>eventDictionary;

    public void Init()
    {
        if (eventDictionary == null)
        {
            eventDictionary = new Dictionary<string, SEvent>();
        }
    }


    public static void StartListening(string eventName, UnityAction<string> listener){
        if(Instance == null){
            //Debug.LogWarning("EventManager does not init");
            return;
        }
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
        if(Instance == null){
            //Debug.LogWarning("EventManager does not init");
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

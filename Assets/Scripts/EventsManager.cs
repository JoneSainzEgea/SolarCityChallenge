using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventsManager : MonoBehaviour
{
    private static Dictionary<string, Action> normalEvents = new Dictionary<string, Action>();
    private static Dictionary<string, Delegate> specialEvents = new Dictionary<string, Delegate>();

    public static void TriggerNormalEvent(string eventName)
    {
        if (normalEvents.TryGetValue(eventName, out Action action))
            action?.Invoke();
    }

    public static void TriggerSpecialEvent<T>(string eventName, T eventData)
    {
        if (specialEvents.TryGetValue(eventName, out Delegate action))
        {
            foreach (Delegate d in action.GetInvocationList())
            {
                if (d.Target == null) continue;
                ((Action<T>)d)?.Invoke(eventData);
            }
        }
    }

    public static void CallNormalEvents(string nameEvent, Action _action)
    {
        if (normalEvents.TryGetValue(nameEvent, out Action action))
            normalEvents[nameEvent] = action + _action;
        else
            normalEvents.Add(nameEvent, _action);
    }

    public static void CallSpecialEvents<T>(string nameEvent, Action<T> _action)
    {
        if (specialEvents.TryGetValue(nameEvent, out Delegate action))
            action = Delegate.Combine(action, _action);
        else
        {
            action = _action;
            specialEvents.Add(nameEvent, action);
        }
    }

    public static void StopCallNormalEvents(string eventName, Action _action)
    {
        if (normalEvents.TryGetValue(eventName, out Action action))
        {
            action -= _action;

            if (action == null)
                normalEvents.Remove(eventName);
            else
                normalEvents[eventName] = action;
        }
    }

    public static void StopCallSpecialEvents<T>(string eventName, Action<T> _action)
    {
        if (specialEvents.TryGetValue(eventName, out Delegate action))
        {
            if (action is Action<T> typedAction)
            {
                typedAction -= _action;

                if (typedAction == null)
                    specialEvents.Remove(eventName);
                else
                    specialEvents[eventName] = typedAction;
            }
        }
    }

    public static void CleanAllEvents()
    {
        specialEvents.Clear();
        normalEvents.Clear();
    }
}

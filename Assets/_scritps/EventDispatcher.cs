using System;
using System.Collections.Generic;
public sealed class EventDispatcher<EVENT_TYPE, PARAMETER>
{
    private EventDispatcher() { }
    readonly static Dictionary<EVENT_TYPE, Action<PARAMETER>> _delegates = new Dictionary<EVENT_TYPE, Action<PARAMETER>>();

    public static void AddListener(EVENT_TYPE eventType, Action<PARAMETER> eventCallback)
    {
        if (_delegates.ContainsKey(eventType))
            _delegates[eventType] += eventCallback;
        else
            _delegates[eventType] = eventCallback;
    }

    public static void RemoveListener(EVENT_TYPE eventType, Action<PARAMETER> eventCallback)
    {
        if (_delegates.ContainsKey(eventType))
            _delegates[eventType] -= eventCallback;
    }

    public static void ClearListener(EVENT_TYPE eventType)
    {
        if (_delegates.ContainsKey(eventType))
            _delegates.Remove(eventType);
    }

    public static void DispatchEvent(EVENT_TYPE eventType, PARAMETER param)
    {
        if (_delegates.ContainsKey(eventType))
            _delegates[eventType]?.Invoke(param);
    }
}
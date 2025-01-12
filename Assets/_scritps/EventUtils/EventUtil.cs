using System;

/// <summary>
/// 事件工具
/// </summary>
public static class EventUtil
{
    /// <summary> 事件发送器 </summary>
    private static EventSender<EventDef, object> sender = new EventSender<EventDef, object>();

    /// <summary> 添加事件监听器 </summary>
    /// <param name="eventType">事件类型</param>
    /// <param name="eventHandler">事件处理器</param>
    public static void AddListener(EventDef eventType, Action<object> eventHandler)
    {
        sender.AddListener(eventType, eventHandler);
    }

    /// <summary> 移除事件监听器 </summary>
    /// <param name="eventType">事件类型</param>
    /// <param name="eventHandler">事件处理器</param>
    public static void RemoveListener(EventDef eventType, Action<object> eventHandler)
    {
        sender.RemoveListener(eventType, eventHandler);
    }

    /// <summary> 是否已经拥有该类型的事件监听器 </summary>
    /// <param name="eventType">事件类型</param>
    public static bool HasListener(EventDef eventType)
    {
        return sender.HasListener(eventType);
    }

    /// <summary> 发送事件 </summary>
    /// <param name="eventType">事件类型</param>
    public static void Dispatch(EventDef eventType)
    {
        sender.Dispatch(eventType, null);
    }

    /// <summary> 发送事件 </summary>
    /// <param name="eventType">事件类型</param>
    public static void Dispatch(EventDef eventType, object eventArg)
    {
        sender.Dispatch(eventType, eventArg);
    }

    /// <summary> 清理所有事件监听器 </summary>
    public static void Clear()
    {
        sender.Clear();
    }

}
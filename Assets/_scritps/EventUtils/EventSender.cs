using System;
using System.Collections.Generic;

/// <summary>
/// 事件发送器
/// </summary>
public class EventSender<TKey, TValue>
{

    /// <summary> 事件表 </summary>
    private Dictionary<TKey, Action<TValue>> dict = new Dictionary<TKey, Action<TValue>>();

    /// <summary> 添加事件监听器 </summary>
    /// <param name="eventType">事件类型</param>
    /// <param name="eventHandler">事件处理器</param>
    public void AddListener(TKey eventType, Action<TValue> eventHandler)
    {
        Action<TValue> callbacks;
        if (dict.TryGetValue(eventType, out callbacks))
        {
            dict[eventType] = callbacks + eventHandler;
        }
        else
        {
            dict.Add(eventType, eventHandler);
        }
    }

    /// <summary> 移除事件监听器 </summary>
    /// <param name="eventType">事件类型</param>
    /// <param name="eventHandler">事件处理器</param>
    public void RemoveListener(TKey eventType, Action<TValue> eventHandler)
    {
        Action<TValue> callbacks;
        if (dict.TryGetValue(eventType, out callbacks))
        {
            callbacks = (Action<TValue>)Delegate.RemoveAll(callbacks, eventHandler);
            if (callbacks == null)
            {
                dict.Remove(eventType);
            }
            else
            {
                dict[eventType] = callbacks;
            }
        }
    }

    /// <summary> 是否已经拥有该类型的事件监听器 </summary>
    /// <param name="eventType">事件名称</param>
    public bool HasListener(TKey eventType)
    {
        return dict.ContainsKey(eventType);
    }

    /// <summary> 发送事件 </summary>
    /// <param name="eventType">事件类型</param>
    /// <param name="eventArg">事件参数</param>
    public void Dispatch(TKey eventType, TValue eventArg)
    {
        Action<TValue> callbacks;
        if (dict.TryGetValue(eventType, out callbacks))
        {
            callbacks.Invoke(eventArg);
        }
    }

    /// <summary> 清理所有事件监听器 </summary>
    public void Clear()
    {
        dict.Clear();
    }

}
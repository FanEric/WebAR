using System;
using System.Collections.Generic;

/// <summary>
/// �¼�������
/// </summary>
public class EventSender<TKey, TValue>
{

    /// <summary> �¼��� </summary>
    private Dictionary<TKey, Action<TValue>> dict = new Dictionary<TKey, Action<TValue>>();

    /// <summary> ����¼������� </summary>
    /// <param name="eventType">�¼�����</param>
    /// <param name="eventHandler">�¼�������</param>
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

    /// <summary> �Ƴ��¼������� </summary>
    /// <param name="eventType">�¼�����</param>
    /// <param name="eventHandler">�¼�������</param>
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

    /// <summary> �Ƿ��Ѿ�ӵ�и����͵��¼������� </summary>
    /// <param name="eventType">�¼�����</param>
    public bool HasListener(TKey eventType)
    {
        return dict.ContainsKey(eventType);
    }

    /// <summary> �����¼� </summary>
    /// <param name="eventType">�¼�����</param>
    /// <param name="eventArg">�¼�����</param>
    public void Dispatch(TKey eventType, TValue eventArg)
    {
        Action<TValue> callbacks;
        if (dict.TryGetValue(eventType, out callbacks))
        {
            callbacks.Invoke(eventArg);
        }
    }

    /// <summary> ���������¼������� </summary>
    public void Clear()
    {
        dict.Clear();
    }

}
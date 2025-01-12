using System;

/// <summary>
/// �¼�����
/// </summary>
public static class EventUtil
{
    /// <summary> �¼������� </summary>
    private static EventSender<EventDef, object> sender = new EventSender<EventDef, object>();

    /// <summary> ����¼������� </summary>
    /// <param name="eventType">�¼�����</param>
    /// <param name="eventHandler">�¼�������</param>
    public static void AddListener(EventDef eventType, Action<object> eventHandler)
    {
        sender.AddListener(eventType, eventHandler);
    }

    /// <summary> �Ƴ��¼������� </summary>
    /// <param name="eventType">�¼�����</param>
    /// <param name="eventHandler">�¼�������</param>
    public static void RemoveListener(EventDef eventType, Action<object> eventHandler)
    {
        sender.RemoveListener(eventType, eventHandler);
    }

    /// <summary> �Ƿ��Ѿ�ӵ�и����͵��¼������� </summary>
    /// <param name="eventType">�¼�����</param>
    public static bool HasListener(EventDef eventType)
    {
        return sender.HasListener(eventType);
    }

    /// <summary> �����¼� </summary>
    /// <param name="eventType">�¼�����</param>
    public static void Dispatch(EventDef eventType)
    {
        sender.Dispatch(eventType, null);
    }

    /// <summary> �����¼� </summary>
    /// <param name="eventType">�¼�����</param>
    public static void Dispatch(EventDef eventType, object eventArg)
    {
        sender.Dispatch(eventType, eventArg);
    }

    /// <summary> ���������¼������� </summary>
    public static void Clear()
    {
        sender.Clear();
    }

}
using System.Collections.Generic;
using System;
public static class EventManager
{
    private static Dictionary<EventEnum, Action<object>> events = new Dictionary<EventEnum, Action<object>>();


    public static void RegistEvent(EventEnum id, Action<object> action)
    {
        if (events.ContainsKey(id))
        {
            events[id] += action;
        }
        else
        {
            events[id] = action;
        }
    }


    public static void CallEvent(EventEnum id, object arg = null)
    {
        if (events.ContainsKey(id))
        {
            events[id](arg);
        }
        else
        {
            BattleDebug.Log("没有注册该事件" + id);
        }
    }
}
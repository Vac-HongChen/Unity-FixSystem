using System;
public static class BattleDebug
{
    public static Action<object> Log = log;
    public static Action<object> LogFile = log;
    public static Action<object> LogError = log;
    private static void log(object o)
    {

    }
}
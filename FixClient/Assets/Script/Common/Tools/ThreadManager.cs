using System.Threading;
using System.Collections.Generic;
public static class ThreadManager
{
    private static List<Thread> threads = new List<Thread>();
    public static void StartThread(ThreadStart action)
    {
        Thread thread = new Thread(action);
        threads.Add(thread);
        thread.Start();
    }
    public static void Close()
    {
        foreach (var item in threads)
        {
            item.Abort();
        }
        threads.Clear();
    }
}
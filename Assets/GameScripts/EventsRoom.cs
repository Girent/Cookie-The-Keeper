using System;

public class EventsRoom
{
    public static Action EndWarmup;

    public static void OnEndWarmup ()
    {
        if(EndWarmup != null)
            EndWarmup.Invoke();
    }
}

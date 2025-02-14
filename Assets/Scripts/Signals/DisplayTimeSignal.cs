using UnityEngine;

public class DisplayTimeSignal : ISignal
{
    public float Time;

    public DisplayTimeSignal(float time)
    {
        Time = time;
    }
}

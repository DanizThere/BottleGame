using UnityEngine;

public class EndBottlesSignal : ISignal
{
    public int Count;

    public EndBottlesSignal(int count)
    {
        Count = count;
    }
}

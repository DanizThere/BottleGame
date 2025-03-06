using UnityEngine;

public interface IStrategy
{
    public Awaitable Execute();
    public bool End();
}

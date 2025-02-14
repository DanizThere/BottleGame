using UnityEngine;

public class ChangeActionSignal : ISignal
{
    public bool HandleAction;

    public ChangeActionSignal(bool handleAction)
    {
        HandleAction = handleAction;
    }
}

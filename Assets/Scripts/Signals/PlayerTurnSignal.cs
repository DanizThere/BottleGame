using UnityEngine;

public class PlayerTurnSignal : ISignal
{
    public PlayerTurnSignal()
    {
        ServiceLocator.Instance.Get<EventBus>().Invoke(new StartPlaySignal());
    }
}

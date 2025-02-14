using UnityEngine;

public class EnemyTurnSignal : ISignal
{
    public EnemyTurnSignal()
    {
        ServiceLocator.Instance.Get<EventBus>().Invoke(new StopPlaySignal());
    }
}

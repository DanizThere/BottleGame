using UnityEngine;

public class DeathSignal : ISignal 
{
    public int Score;
    public int Points;
    public DeathSignal(int score, int points)
    {
        Score = score;
        Points = points;

        ServiceLocator.Instance.Get<EventBus>().Invoke(new StopPlaySignal());
        ServiceLocator.Instance.Get<EventBus>().Invoke(new StopUseSignal());
    }
}

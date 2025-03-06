using System.Collections.Generic;
using System.Linq;

public class TurnManager : IService, IDispose
{
    private EventBus eventBus;
    private Stack<int> turns = new Stack<int>();
    public void Init()
    {
        eventBus = ServiceLocator.Instance.Get<EventBus>();

        eventBus.Subscribe<UnsubscibeSignal>(Dispose);
        eventBus.Subscribe<DeathSignal>(AllTurns);
    }

    public int ShowLastTurn()
    {
        return turns.Peek();
    }

    public void AddTurn(int turn)
    {
        turns.Push(turn);
    }

    public void AllTurns(DeathSignal signal)
    {
        var a = turns.GroupBy(p => p).Where(p => p.Count() > 0).Select(p => p.Count()).ToArray();
    }

    public void Dispose(UnsubscibeSignal signal)
    {
        eventBus.Unsubscribe<DeathSignal>(AllTurns);
    }
}

using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TurnManager : MonoBehaviour, IService, IDispose
{
    private EventBus eventBus;
    private Stack<int> turns = new Stack<int>();

    public void Init(EventBus eventBus)
    {
        this.eventBus = eventBus;

        this.eventBus.Subscribe<UnsubscibeSignal>(Dispose);
        this.eventBus.Subscribe<DeathSignal>(AllTurns);
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

        foreach(var b in a)
        {
            Debug.Log(b + " turns");
        }
    }

    public void Dispose(UnsubscibeSignal signal)
    {
        eventBus.Unsubscribe<DeathSignal>(AllTurns);

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TurnManager : IService
{
    private Reactive<Turn> currentTurn = new Reactive<Turn>();
    private IntermediateTurn checkTurn = new IntermediateTurn();
    public void Init()
    {

    }

    public Guid ShowLastTurn()
    {
        return currentTurn.Value.Name;
    }

    public async void AddTurn(Turn turn)
    {
        currentTurn.Value = checkTurn;

        await checkTurn.CheckBottles();

        currentTurn.Value = turn;
    }
}

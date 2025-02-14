using System;
using UnityEngine;

public class GameManager : MonoBehaviour, IService, IDispose
{
    private EventBus bus;
    private SeedGenerator generator;
    private SaveManager saveManager;
    public Player player { get; private set; }
   
    public int pointsInGame;
    public int Score { get; private set; } = 0;

    public void Init(EventBus eventBus, Player player, SaveManager save)
    {
        generator = new SeedGenerator();
        generator.Generate();

        bus = eventBus;
        bus.Subscribe<TakeEffectSignal>(SetTurn);
        bus.Subscribe<WhenDeadSignal>(CheckSavethrows);
        bus.Subscribe<WhenDeadSignal>(SetAlive);
        bus.Subscribe<UnsubscibeSignal>(Dispose);

        bus.Invoke(new StartUseSignal());
        bus.Invoke(new StartPlaySignal());

        this.player = player;
        saveManager = save;
    }

    public void SetTurn(TakeEffectSignal signal)
    {
        switch (signal.Person)
        {
            case TypeOfPerson.PLAYER:
                bus.Invoke(new EnemyTurnSignal());
                bus.Invoke(new StopUseSignal());
                break;
            case TypeOfPerson.ENEMY:
                bus.Invoke(new PlayerTurnSignal());
                bus.Invoke(new StartUseSignal());
                break;
            case TypeOfPerson.NONE:
                Debug.Log("Todo");
                break;
        }
    }

    public void SetAlive(WhenDeadSignal signal)
    {
        signal.DNDPerson.SetHits(signal.DNDPerson.maxHit / 2);
    }

    public void CheckSavethrows(WhenDeadSignal signal)
    {
        switch ((signal.existsSavethrows, signal.Person))
        {
            case (2,TypeOfPerson.PLAYER):
                bus.Invoke(new DialogueSignal("Savethrow_2"));
                break;
            case (1, TypeOfPerson.PLAYER):
                bus.Invoke(new DialogueSignal("Savethrow_1"));
                break;
            case (0, TypeOfPerson.PLAYER):
                if(Score < pointsInGame) Score = pointsInGame;
                bus.Invoke(new DeathSignal(Score, LevelPoints()));
                saveManager.Save();
                saveManager.SavePoints(LevelPoints());
                saveManager.Save(Score);
                bus.Invoke(new DialogueSignal("Savethrow_1"));
                break;

            case (1, TypeOfPerson.ENEMY):
                bus.Invoke(new DialogueSignal("Savethrow_2"));
                break;
            case (0, TypeOfPerson.ENEMY):
                bus.Invoke(new DialogueSignal("Savethrow_1"));
                break;

            default:
                bus.Invoke(new DialogueSignal("Savethrow_default"));
                break;
        }
    }

    public int LevelPoints()
    {
        int result = (int)Math.Log(pointsInGame);
        return pointsInGame / result;
    }

    public void Exit()
    {
        Application.Quit();
    }

    public void Dispose(UnsubscibeSignal signal)
    {
        bus.Unsubscribe<TakeEffectSignal>(SetTurn);
    }
}

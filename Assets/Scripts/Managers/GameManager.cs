using System;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : MonoBehaviour, IService, IDispose
{
    private Func<EventBus> bus;
    private Func<SaveManager> saveManager;
    private GameState GameState = GameState.On_UI;
    private TurnManager turnManager;
    private int pointsInGame;
    private Func<BottlesManager> bottlesManager;

    private Player player;
    private Enemy enemy;
    public void Init(Func<EventBus> eventBus, Func<SaveManager> saveManager, Func<BottlesManager> bottlesManager)
    {
        turnManager = new TurnManager();

        bus = eventBus;
        this.saveManager = saveManager;
        this.bottlesManager = bottlesManager;

        turnManager.Init();
        SetGameState(GameState);

        player = FindAnyObjectByType<Player>();
        enemy = FindAnyObjectByType<Enemy>();
    }

    private void Start()
    {
        //bus.Subscribe<IntermediateSignal>(SetTurn);
        //bus.Subscribe<WhenDeadSignal>(CheckSavethrows);
        //bus.Subscribe<WhenDeadSignal>(SetAlive);
        bus().Subscribe<DeathSignal>(x => SetGameState(GameState.On_UI));
        bus().Subscribe<UnsubscibeSignal>(Dispose);
        bus().Subscribe<ChangeStateSignal>(ToGame);
        bus().Subscribe<TavernSignal>(TavernTurn);

        bus().Invoke(new EndBottlesSignal(bottlesManager().CountOfBottles));

    }

    public void TavernTurn(TavernSignal signal)
    {
        if (bottlesManager().BottlesExist.Count == 0)
        {
            bus().Invoke(new EndBottlesSignal(bottlesManager().CountOfBottles));
            player.ResetAgree();
            enemy.ResetAgree();
            return;
        }

        if (player.isAgreed && !enemy.isAgreed) bottlesManager().UseBottle(player, enemy);
    }

    public void SetTurn(IntermediateSignal signal)
    {
        switch (turnManager.ShowLastTurn())
        {
            case (int)TypeOfPerson.PLAYER:
                bus().Invoke(new EnemyTurnSignal());
                bus().Invoke(new StopUseSignal());
                break;
            case (int)TypeOfPerson.ENEMY:
                bus().Invoke(new PlayerTurnSignal());
                bus().Invoke(new StartUseSignal());
                break;
            case (int)TypeOfPerson.NONE:
                Debug.Log("Todo");
                break;
        }
    }

    public void ToGame(ChangeStateSignal signal)
    {
        if(GameState == GameState.On_UI)
        {
            SetGameState(GameState.On_Game);
        }
    } 

    public void SetGameState(GameState gameState)
    {
        GameState = gameState;

        switch (GameState)
        {
            case GameState.On_UI:
                bus().Invoke(new OnUISignal());
                bus().Invoke(new StopPlaySignal());
                bus().Invoke(new StopUseSignal());
                break;
            case GameState.On_Game:
                bus().Invoke(new OffUISignal());
                bus().Invoke(new StartPlaySignal());
                bus().Invoke(new StartUseSignal());
                break;
            case GameState.On_Pause:
                bus().Invoke(new OnUISignal());
                bus().Invoke(new StopPlaySignal());
                bus().Invoke(new StopUseSignal());
                break;
        }
    }

    public int LevelPoints()
    {
        int result = (int)Math.Log(pointsInGame);
        return pointsInGame / result;
    }

    public void Dispose(UnsubscibeSignal signal)
    {
        bus().Unsubscribe<IntermediateSignal>(SetTurn);
    }

    public void UpdatePoints(int value) => pointsInGame += value;
    public void AddTurn(int value) => turnManager.AddTurn(value);
}

public enum GameState
{
    On_UI,
    On_Game,
    On_Pause
}

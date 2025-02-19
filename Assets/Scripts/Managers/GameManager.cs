using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour, IService, IDispose
{
    private EventBus bus;
    private SeedGenerator generator;
    private SaveManager saveManager;
    public GameState GameState { get; private set; } = GameState.On_UI;
    public TurnManager turnManager {  get; private set; }
    [SerializeField] private MusicMan musicManager;
    public Player player { get; private set; }
   
    public int pointsInGame;
    public int Score { get; private set; } = 0;

    public void Init(EventBus eventBus, Player player, SaveManager save)
    {
        generator = new SeedGenerator();
        turnManager = new TurnManager();
        generator.Generate();

        bus = eventBus;

        this.player = player;
        saveManager = save;

        turnManager.Init(bus);
        musicManager.Init(this.player.transform);  
    }

    private void Start()
    {
        bus.Subscribe<IntermediateSignal>(SetTurn);
        bus.Subscribe<WhenDeadSignal>(CheckSavethrows);
        bus.Subscribe<WhenDeadSignal>(SetAlive);
        bus.Subscribe<DeathSignal>(x => SetGameState(GameState.On_UI));
        bus.Subscribe<UnsubscibeSignal>(Dispose);
    }

    public void SetTurn(IntermediateSignal signal)
    {
        switch (turnManager.ShowLastTurn())
        {
            case (int)TypeOfPerson.PLAYER:
                bus.Invoke(new EnemyTurnSignal());
                bus.Invoke(new StopUseSignal());
                break;
            case (int)TypeOfPerson.ENEMY:
                bus.Invoke(new PlayerTurnSignal());
                bus.Invoke(new StartUseSignal());
                break;
            case (int)TypeOfPerson.NONE:
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

    public void SetGameState(GameState gameState)
    {
        GameState = gameState;

        switch (GameState)
        {
            case GameState.On_UI:
                bus.Invoke(new OnUISignal());
                bus.Invoke(new StopPlaySignal());
                bus.Invoke(new StopUseSignal());
                break;
            case GameState.On_Game:
                bus.Invoke(new OffUISignal());
                bus.Invoke(new StartPlaySignal());
                bus.Invoke(new StartUseSignal());
                break;
            case GameState.On_Pause:
                bus.Invoke(new OnUISignal());
                bus.Invoke(new StopPlaySignal());
                bus.Invoke(new StopUseSignal());
                break;
        }

        Debug.Log("Im tired boss " + GameState.ToString());
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

    public void LoadLevel(int level)
    {
        SceneManager.LoadScene(level);
    }

    public void Dispose(UnsubscibeSignal signal)
    {
        bus.Unsubscribe<IntermediateSignal>(SetTurn);
    }
}

public enum GameState
{
    On_UI,
    On_Game,
    On_Pause
}

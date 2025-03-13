using System;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : MonoBehaviour, IService
{
    private Func<SaveManager> saveManager;
    private TurnManager turnManager;
    private int pointsInGame = 1;
    private Func<BottlesManager> bottlesManager;

    private Player player;
    private Enemy enemy;

    public event Action StartGame;
    public event Action OnSelected;
    public event Action OnDeath;
    public int PointsInGame => pointsInGame;
    public void Init(Func<SaveManager> saveManager, Func<BottlesManager> bottlesManager)
    {
        turnManager = new TurnManager();

        this.saveManager = saveManager;
        this.bottlesManager = bottlesManager;

        turnManager.Init();

        player = FindAnyObjectByType<Player>();
        enemy = FindAnyObjectByType<Enemy>();
    }

    private void Start()
    {
        //bus().Subscribe<DeathSignal>(x => SetGameState(GameState.On_UI));
        //bus().Subscribe<ChangeStateSignal>(ToGame);

        //заменить выше

        StartGame?.Invoke();
    }

    public int LevelPoints()
    {
        int result = (int)Math.Log(pointsInGame) + 1;
        return pointsInGame / result;
    }

    public void UpdatePoints(int value) => pointsInGame += value;
    public void AddTurn(Turn value) => turnManager.AddTurn(value);
    public bool CheckTurn(Guid value)
    {
        return turnManager.ShowLastTurn() == value;
    }

    public void Intermediate() => OnSelected?.Invoke();
}

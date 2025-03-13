using System;
using UnityEngine;

public class CommonBottle : MonoBehaviour
{
    [Range(0f, 1f)]
    public float weight = .1f;
    public int Points = 10;
    private GameManager gameManager;
    private BottlesManager bottlesManager;
    public event Action<int> PointsUpdated;

    private void Start()
    {
        gameManager = ServiceLocator.Instance.Get<GameManager>();
        bottlesManager = ServiceLocator.Instance.Get<BottlesManager>();

        PointsUpdated += gameManager.UpdatePoints;
    }

    private void Awake()
    {
        gameObject.name = $"{GetType()}";
    }

    public virtual void TakeEffect(DNDManipulator person)
    {
    }

    public virtual void TakeEffect(Player player)
    {
        bottlesManager.HandlePlayerChoise(this);
        player.HandleAgree();
    }

    public virtual void TakeEffect(Enemy enemy)
    {
        enemy.HandleAgree();
        bottlesManager.HandleEnemyChoise(this);
    }

    public virtual void SetEffect(Player player)
    {
        PointsUpdated?.Invoke(Points);
    }
    public virtual void SetEffect(Enemy enemy)
    {
        Debug.Log("yappie");
    }
}

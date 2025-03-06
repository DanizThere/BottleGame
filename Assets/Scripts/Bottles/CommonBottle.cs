using UnityEngine;

public class CommonBottle : MonoBehaviour
{
    [Range(0f, 1f)]
    public float weight = .1f;
    public int Points = 10;

    protected EventBus eventBus;
    private GameManager gameManager;
    private BottlesManager bottlesManager;

    private void Start()
    {
        eventBus = ServiceLocator.Instance.Get<EventBus>();
        gameManager = ServiceLocator.Instance.Get<GameManager>();
        bottlesManager = ServiceLocator.Instance.Get<BottlesManager>();
    }

    private void Awake()
    {
        gameObject.name = $"{GetType()}";
    }

    public virtual void TakeEffect(DNDManipulator person)
    {
        gameManager.AddTurn(person.person.turnValue);

        eventBus.Invoke(new TavernSignal());
    }

    public virtual void TakeEffect(Player player)
    {
        bottlesManager.HandlePlayerChoise(this);
        //gameManager.AddTurn(player.dndManipulator.person.turnValue);
        player.HandleAgree();

        eventBus.Invoke(new TavernSignal());
    }

    public virtual void TakeEffect(Enemy enemy)
    {
        //gameManager.AddTurn(enemy.manipulator.person.turnValue);
        enemy.HandleAgree();
        bottlesManager.HandleEnemyChoise(this);

        eventBus.Invoke(new TavernSignal());
    }

    public virtual void SetEffect(Player player)
    {
        gameManager.UpdatePoints(Points);
    }
    public virtual void SetEffect(Enemy enemy)
    {
        Debug.Log("yappie");
    }
}

using System.Threading.Tasks;
using UnityEngine;

public class CommonBottle : MonoBehaviour
{
    [Range(0f, 1f)]
    public float weight = .1f;
    public int Points = 10;

    protected ServiceLocator serviceLocator;
    protected EventBus eventBus;

    private void Start()
    {
        serviceLocator = ServiceLocator.Instance;
        eventBus = serviceLocator.Get<EventBus>();
    }

    private void Awake()
    {
        gameObject.name = $"{GetType()}";
    }

    public virtual void TakeEffect(DNDPerson person)
    {
        serviceLocator.Get<BottlesManager>().ReleaseBottle(this);
        eventBus.Invoke(new IntermediateSignal());
    }

    public virtual void TakeEffect(Player player)
    {
        serviceLocator.Get<GameManager>().pointsInGame += Points;
        serviceLocator.Get<GameManager>().turnManager.AddTurn(player.turnValue);
        serviceLocator.Get<BottlesManager>().ReleaseBottle(this);
        eventBus.Invoke(new IntermediateSignal());
    }

    public virtual void TakeEffect(Enemy enemy)
    {
        serviceLocator.Get<BottlesManager>().ReleaseBottle(this);
        serviceLocator.Get<GameManager>().turnManager.AddTurn(enemy.turnValue);
        eventBus.Invoke(new IntermediateSignal());
    }
}

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

    public virtual void TakeEffect(DNDPerson person)
    {
        if(person.typeOfPerson == TypeOfPerson.PLAYER) serviceLocator.Get<GameManager>().pointsInGame += Points;
        serviceLocator.Get<BottlesManager>().ReleaseBottle(this);
        eventBus.Invoke(new TakeEffectSignal(person.typeOfPerson));
    }
}

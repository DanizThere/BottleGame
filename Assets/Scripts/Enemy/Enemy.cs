using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class Enemy : DNDPerson, IEnemy, IAction, IDispose
{
    private List<CommonBottle> bottles;
    private bool canPlay;

    private void Awake()
    {
        personClass = GetComponent<DNDClasses>();

        PersonInit(1.5f, personClass, TypeOfPerson.ENEMY, 120, 1, 8, 8, 8, 8, 8, 8, "Враг без имени");
    }

    private void Start()
    {
        canPlay = true;

        eventBus = ServiceLocator.Instance.Get<EventBus>();
        eventBus.Subscribe<EnemyTurnSignal>(ActSignal, 2);
        eventBus.Subscribe<EnemyTurnSignal>(UpdateBottles<EnemyTurnSignal>, 0);
        eventBus.Subscribe<DeathSignal>(DontPlay);

        eventBus.Subscribe<UnsubscibeSignal>(Dispose);
    }

    public async void Act()
    {
        await Task.Delay(3000);
        if (!canPlay) return;
        bottles[Random.Range(0, bottles.Count)].GetComponent<CommonBottle>().TakeEffect(this);
    }

    public void DontPlay(DeathSignal signal)
    {
        canPlay = false;
    }

    public void ActSignal(EnemyTurnSignal signal)
    {
        Act();
    }

    public void UpdateBottles<T>(T signal)
    {
        bottles = ServiceLocator.Instance.Get<BottlesManager>().BottlesExist;
    }

    public void TryBelief(DNDPerson person, CommonBottle bottle)
    {
        if (person.CanBelief())
        {

        }
    }

    public void Use(IInteractable interactable)
    {
        interactable.Interact();
    }

    public bool Check()
    {
        return true;
    }

    public void SuccessCheck(IAction action) 
    {
    
    }

    public void Dispose(UnsubscibeSignal signal)
    {
        eventBus.Unsubscribe<EnemyTurnSignal>(ActSignal);
        eventBus.Unsubscribe<EnemyTurnSignal>(UpdateBottles<EnemyTurnSignal>);
        eventBus.Unsubscribe<DeathSignal>(DontPlay);

    }

    public override void CheckHP()
    {
    }

    public override bool CanBelief()
    {
        return true;
    }
}

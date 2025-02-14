using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class Enemy : DNDPerson, IEnemy, IAction, IDispose
{
    private List<CommonBottle> bottles;
    private void Start()
    {
        typeOfPerson = TypeOfPerson.ENEMY;

        eventBus = ServiceLocator.Instance.Get<EventBus>();
        eventBus.Subscribe<EnemyTurnSignal>(ActSignal, 2);
        eventBus.Subscribe<EnemyTurnSignal>(UpdateBottles<EnemyTurnSignal>, 0);

        eventBus.Subscribe<UnsubscibeSignal>(Dispose);
    }

    public async void Act()
    {
        await Task.Delay(3000);
        bottles[UnityEngine.Random.Range(0, bottles.Count)].GetComponent<CommonBottle>().TakeEffect(this);
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
    }

    public override void CheckHP()
    {
    }

    public override bool CanBelief()
    {
        return true;
    }
}

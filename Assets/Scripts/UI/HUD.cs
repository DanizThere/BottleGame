using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour, IDispose
{
    [SerializeField] private GameObject Hud;
    [SerializeField] private GameObject Heal;
    [SerializeField] private GameObject Damage;
    private EventBus eventBus;

    private void Awake()
    {
        Hud = GameObject.Find("HUD");
    }

    private void Start()
    {
        eventBus = ServiceLocator.Instance.Get<EventBus>();

        eventBus.Subscribe<HealSignal>(HealAction);
        eventBus.Subscribe<DamageSignal>(DamageAction);
        eventBus.Subscribe<UnsubscibeSignal>(Dispose);
    }

    public async void HealAction(HealSignal signal)
    {
        if(signal.Person == TypeOfPerson.PLAYER)
        {
            Heal.SetActive(true);
            await Task.Delay(TimeSpan.FromSeconds(.5f));
            Heal.SetActive(false);
        }
    }
    public async void DamageAction(DamageSignal signal)
    {
        if(signal.Person == TypeOfPerson.PLAYER)
        {
            Damage.SetActive(true);
            await Task.Delay(TimeSpan.FromSeconds(.5f));
            Damage.SetActive(false);
        } 
    }


    public void Dispose(UnsubscibeSignal signal)
    {
        eventBus.Unsubscribe<HealSignal>(HealAction);
        eventBus.Unsubscribe<DamageSignal>(DamageAction);
    }
}

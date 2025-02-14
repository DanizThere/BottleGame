using System;
using System.Collections;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

public class Timer : MonoBehaviour, IDispose
{
    public float deltaTimer = 15;
    public float timer { get; private set; }
    private EventBus eventBus;
    private CancellationTokenSource token;
    private SoundManager soundManager;
    [SerializeField] private AudioClip[] clockTick;

    private void Awake()
    {
        token = new CancellationTokenSource();
    }

    private void Start()
    {
        eventBus = ServiceLocator.Instance.Get<EventBus>();
        soundManager = ServiceLocator.Instance.Get<SoundManager>();

        eventBus.Subscribe<TakeEffectSignal>(CancelTimer<TakeEffectSignal>, 3);
        eventBus.Subscribe<DeathSignal>(CancelTimer<DeathSignal>,0);
        eventBus.Subscribe<TakeEffectSignal>(ResetToken, 2);
        eventBus.Subscribe<TakeEffectSignal>(TurnTimer, 1);
        eventBus.Subscribe<UnsubscibeSignal>(Dispose);
    }

    public void ResetTimer()
    {
        timer = deltaTimer;
    }

    public void ResetToken(TakeEffectSignal signal)
    {
        token.Dispose();
        token = new CancellationTokenSource();
    }

    public void SetTimer(float time)
    {
        deltaTimer = time;
    }

    public void TurnTimer(TakeEffectSignal signal)
    {
        ResetTimer();
        WorkTimer(signal, token.Token);
    }


    //todo: переписать таймер, чтобы он начинался исключительно в момент наступления хода
    private async Task WorkTimer(TakeEffectSignal signal, CancellationToken token)
    {
        do
        {
            if (token.IsCancellationRequested)
            {
                break;
            }
            soundManager.PlaySoundRandom(clockTick, transform, .5f);
            eventBus.Invoke(new DisplayTimeSignal(timer));
            timer--;
            await Task.Delay(1000);
        }
        while (timer >= 0);
        if (token.IsCancellationRequested)
        {
            return;
        }

        switch (signal.Person)
        {
            case TypeOfPerson.PLAYER:
                eventBus.Invoke(new PlayerTurnSignal());
                eventBus.Invoke(new StartUseSignal());
                break;
            case TypeOfPerson.ENEMY:
                eventBus.Invoke(new EnemyTurnSignal());
                eventBus.Invoke(new StopUseSignal());
                break;
        }
    }

    private void CancelTimer<T>(T signal) where T : ISignal
    {
        token.Cancel();
    }

    public void Dispose(UnsubscibeSignal signal)
    {
        token.Cancel();

        eventBus.Unsubscribe<TakeEffectSignal>(TurnTimer);
        eventBus.Unsubscribe<TakeEffectSignal>(ResetToken);
        eventBus.Unsubscribe<TakeEffectSignal>(CancelTimer<TakeEffectSignal>);
    }
}

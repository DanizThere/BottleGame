using System;
using System.Collections;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

public class Timer : MonoBehaviour, IDispose
{
    public float deltaTimer = 15;
    public float timer { get; private set; }
    private bool canCount;
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

        eventBus.Subscribe<IntermediateSignal>(CancelTimer<IntermediateSignal>, 3);
        eventBus.Subscribe<DeathSignal>(StopTimer,0);
        eventBus.Subscribe<IntermediateSignal>(ResetToken, 2);
        eventBus.Subscribe<IntermediateSignal>(TurnTimer, 1);
        eventBus.Subscribe<UnsubscibeSignal>(Dispose);

        canCount = true;
    }

    public void ResetTimer()
    {
        timer = deltaTimer;
    }

    public void ResetToken(IntermediateSignal signal)
    {
        token.Dispose();
        token = new CancellationTokenSource();
    }

    public void SetTimer(float time)
    {
        deltaTimer = time;
    }

    public void TurnTimer(IntermediateSignal signal)
    {
        ResetTimer();
        WorkTimer(signal, token.Token);
    }


    //todo: ���������� ������, ����� �� ��������� ������������� � ������ ����������� ����
    private async Task WorkTimer(IntermediateSignal signal, CancellationToken token)
    {
        if (!canCount) return;
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

        //switch (signal.Person)
        //{
        //    case TypeOfPerson.PLAYER:
        //        eventBus.Invoke(new PlayerTurnSignal());
        //        eventBus.Invoke(new StartUseSignal());
        //        break;
        //    case TypeOfPerson.ENEMY:
        //        eventBus.Invoke(new EnemyTurnSignal());
        //        eventBus.Invoke(new StopUseSignal());
        //        break;
        //}
    }

    private void CancelTimer<T>(T signal) where T : ISignal
    {
        token.Cancel();
    }

    private void StopTimer(DeathSignal signal) { 
        token.Cancel();
        canCount = false; }

    public void Dispose(UnsubscibeSignal signal)
    {
        token.Cancel();

        eventBus.Unsubscribe<IntermediateSignal>(TurnTimer);
        eventBus.Unsubscribe<IntermediateSignal>(ResetToken);
        eventBus.Unsubscribe<IntermediateSignal>(CancelTimer<IntermediateSignal>);
    }
}

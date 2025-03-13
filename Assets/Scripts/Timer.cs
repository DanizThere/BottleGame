using System;
using System.Collections;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

public class Timer : MonoBehaviour
{
    public float deltaTimer = 15;
    public float timer { get; private set; }
    private bool canCount;
    private CancellationTokenSource token;
    private SoundManager soundManager;
    [SerializeField] private AudioClip[] clockTick;

    private void Awake()
    {
        token = new CancellationTokenSource();
    }

    private void Start()
    {
        soundManager = ServiceLocator.Instance.Get<SoundManager>();

        //eventBus.Subscribe<IntermediateSignal>(CancelTimer<IntermediateSignal>, 3);
        //eventBus.Subscribe<DeathSignal>(StopTimer,0);
        //eventBus.Subscribe<IntermediateSignal>(ResetToken, 2);
        //eventBus.Subscribe<IntermediateSignal>(TurnTimer, 1);
        //eventBus.Subscribe<UnsubscibeSignal>(Dispose); заменить

        canCount = true;
    }

    public void ResetTimer()
    {
        timer = deltaTimer;
    }

    public void ResetToken()
    {
        token.Dispose();
        token = new CancellationTokenSource();
    }

    public void SetTimer(float time)
    {
        deltaTimer = time;
    }

    public void TurnTimer()
    {
        ResetTimer();
        WorkTimer(token.Token);
    }


    //todo: переписать таймер, чтобы он начинался исключительно в момент наступления хода
    private async Awaitable WorkTimer(CancellationToken token)
    {
        if (!canCount) return;
        do
        {
            if (token.IsCancellationRequested)
            {
                break;
            }
            soundManager.PlaySoundRandom(clockTick, transform, .5f);
            timer--;
            await Awaitable.WaitForSecondsAsync(1);
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

    private void CancelTimer()
    {
        token.Cancel();
    }

    private void StopTimer() 
    { 
        token.Cancel();
        canCount = false; 
    }

}

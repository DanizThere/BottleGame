using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BottlesManager : MonoBehaviour, IService, IDispose
{
    private Dictionary<float, PoolObject<CommonBottle>> bottlesPool = new Dictionary<float, PoolObject<CommonBottle>>();
    [Header("Bottles Prefab")]
    [SerializeField] private GameObject[] bottles;
    public List<CommonBottle> BottlesExist = new List<CommonBottle>();
    public int CountOfBottles = 5;

    private Func<EventBus> eventBus;
    private Func<SoundManager> soundManager;
    [SerializeField] private AudioClip bottleUseSound;

    private CommonBottle playerChoise;
    private CommonBottle enemyChoise;

    public void Init(Func<SoundManager> soundManager, Func<EventBus> eventBus)
    {
        this.soundManager = soundManager;
        this.eventBus = eventBus;
    }

    private void Start()
    {
        AddBottle();
        eventBus().Subscribe<IntermediateSignal>(CheckBottles);
        eventBus().Subscribe<EndBottlesSignal>(SetRandomBottles);

        eventBus().Subscribe<UnsubscibeSignal>(Dispose);
    }

    public void Spawn(CommonBottle bottles)
    {
        PoolObject<CommonBottle> pool;
        var key = bottles.weight;
        if (!bottlesPool.ContainsKey(key))
        {
            pool = new PoolObject<CommonBottle>(bottles, 5);
            bottlesPool.Add(key, pool);
        }
    }

    public void CheckBottles(IntermediateSignal signal)
    {
        if (BottlesExist.Count == 0)
        {
            eventBus().Invoke(new EndBottlesSignal(CountOfBottles));
        }
    }

    public void AddBottle()
    {
        for (int i = 0; i < bottles.Length; i++)
        {
            for (int j = 0; j < bottles.Length; j++)
            {
                if (bottles[i].GetComponent<CommonBottle>().weight < bottles[j].GetComponent<CommonBottle>().weight)
                {
                    var bottle = bottles[j];
                    bottles[j] = bottles[i];
                    bottles[i] = bottle;
                }
            }
        }

        foreach (var bottle in bottles)
        {
            Spawn(bottle.GetComponent<CommonBottle>());
        }
    }

    public CommonBottle TakeRandom()
    {
        return BottlesExist[UnityEngine.Random.Range(0, BottlesExist.Count)];
    }

    public void SetRandomBottles(EndBottlesSignal signal)
    {
        for (int i = 0; i < signal.Count; i++)
        {
            float rand = UnityEngine.Random.Range(0, 1.0f);
            foreach (var bottles in bottlesPool)
            {
                if (rand <= bottles.Key)
                {
                    var gm = bottlesPool[bottles.Key].Get();
                    BottlesExist.Add(gm);
                    gm.transform.localPosition = new Vector3(i, -.4f, Mathf.Pow(-1, i) / 4);
                    break;
                }
            }
        }
        eventBus().Invoke(new HandleTextSignal(ShowCurrentBottles()));
    }

    public string ShowCurrentBottles()
    {
        string bottlesInARow = "В этом раунде следующие бутылки: ";

        var a = BottlesExist.GroupBy(p => p.name).Where(p => p.Count() > 0).Select(p => p.Count() +" "+ p.Key).ToArray();

        for(int i = 0; i < a.Length; i++)
        {
            if(i == a.Length - 1)
            {
                bottlesInARow += $"{a[i]}.";
            }
            else
            {
                bottlesInARow += $"{a[i]},";
            }
        }
        return bottlesInARow;
    }

    public void HandlePlayerChoise(CommonBottle bottle) => playerChoise = bottle;
    public void HandleEnemyChoise(CommonBottle bottle) => enemyChoise = bottle;

    public void UseBottle(Player player, Enemy enemy)
    { 
        if(playerChoise == enemyChoise)
        {
            if (player.dndManipulator.GetCharacteristicValue("Charisma") + UnityEngine.Random.Range(0, (int)Dices.D20) >= enemy.manipulator.GetCharacteristicValue("Charisma") + UnityEngine.Random.Range(0, (int)Dices.D20))
                playerChoise.SetEffect(player);
            else enemyChoise.SetEffect(enemy);

            return;
        }
        playerChoise.SetEffect(player);
        ReleaseBottle(playerChoise);
        player.ResetAgree();
        enemyChoise.SetEffect(enemy);
        ReleaseBottle(enemyChoise);
        enemy.ResetAgree();
    }

    public void ReleaseBottle(CommonBottle bottle)
    {
        bottlesPool[bottle.weight].Release(bottle);
        soundManager().PlaySound(bottleUseSound, bottle.transform, 1f);
        BottlesExist.Remove(bottle);
    }

    public void Dispose(UnsubscibeSignal signal)
    {
        eventBus().Unsubscribe<IntermediateSignal>(CheckBottles);
        eventBus().Unsubscribe<EndBottlesSignal>(SetRandomBottles);
    }
}

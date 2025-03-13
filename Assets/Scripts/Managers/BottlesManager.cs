using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BottlesManager : MonoBehaviour, IService
{
    private Dictionary<float, PoolObject<CommonBottle>> bottlesPool = new Dictionary<float, PoolObject<CommonBottle>>();
    [Header("Bottles Prefab")]
    [SerializeField] private List<GameObject> bottles = new List<GameObject>();
    public List<CommonBottle> BottlesExist = new List<CommonBottle>();
    public int CountOfBottles = 5;

    private Func<SoundManager> soundManager;
    private Func<GameManager> gameManager;
    [SerializeField] private AudioClip bottleUseSound;

    private CommonBottle playerChoise;
    private CommonBottle enemyChoise;

    private string CurrentBottles = null;

    public event Action InstantiateBottles;
    public event Action<string> ShowBottles;
    public void Init(Func<SoundManager> soundManager, Func<GameManager> gameManager)
    {
        this.soundManager = soundManager;
        this.gameManager = gameManager;
    }

    private void Awake()
    {
        AddBottle();
    }


    private void Start()
    {
        //eventBus().Subscribe<IntermediateSignal>(CheckBottles);
        //eventBus().Subscribe<EndBottlesSignal>(SetRandomBottles);

        gameManager().StartGame += SetBottles;
        InstantiateBottles += SetBottles;
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

    public bool CheckBottles()
    {
        if (BottlesExist.Count == 0)
        {
            InstantiateBottles?.Invoke();
            return false;
        }
        return true;
    }

    public void AddBottle()
    {
        var bottlesSorted = bottles.OrderBy(p => p.GetComponent<CommonBottle>().weight).ToList();

        foreach (var bottle in bottlesSorted)
        {
            Spawn(bottle.GetComponent<CommonBottle>());
        }
    }

    public CommonBottle TakeRandom() => BottlesExist[UnityEngine.Random.Range(0, BottlesExist.Count)];



    public void SetBottles()
    {
        int count = CountOfBottles;
        SetRandomBottles(count);

        ShowBottles?.Invoke(CurrentBottles);
    }

    private void SetRandomBottles(int count)
    {
        for (int i = 0; i < count; i++)
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
        CurrentBottles = ShowCurrentBottles();
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
            if (player.Person.charisma.Value + UnityEngine.Random.Range(0, (int)Dices.D20) >= enemy.Person.charisma.Value + UnityEngine.Random.Range(0, (int)Dices.D20))
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
}

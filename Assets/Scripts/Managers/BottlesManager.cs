using System.Collections.Generic;
using UnityEngine;

public class BottlesManager : MonoBehaviour, IService, IDispose
{
    private Transform[] bottlesTransform;
    private Dictionary<float, PoolObject<CommonBottle>> bottlesPool = new Dictionary<float, PoolObject<CommonBottle>>();
    [Header("Bottles Prefab")]
    [SerializeField] private GameObject[] bottles;
    [SerializeField] private Transform ParentTransform;
    public List<CommonBottle> BottlesExist = new List<CommonBottle>();
    public int CountOfBottles = 5;

    private EventBus eventBus;
    private SoundManager soundManager;
    [SerializeField] private AudioClip bottleUseSound;
    private void Awake()
    {
        for(int i = 0; i < bottles.Length; i++)
        {
            for(int j = 0; j < bottles.Length; j++)
            {
                if(bottles[i].GetComponent<CommonBottle>().weight < bottles[j].GetComponent<CommonBottle>().weight)
                {
                    var bottle = bottles[j];
                    bottles[j] = bottles[i];
                    bottles[i] = bottle;
                }
            }
        }
    }

    public void Init(SoundManager sound, EventBus eventBus)
    {
        AddBottle();

        soundManager = sound;
        bottlesTransform = ParentTransform.GetComponentsInChildren<Transform>();

        this.eventBus = eventBus;
        eventBus.Subscribe<TakeEffectSignal>(CheckBottles);
        eventBus.Subscribe<EndBottlesSignal>(SetRandomBottles);
        eventBus.Invoke(new EndBottlesSignal(CountOfBottles));

        eventBus.Subscribe<UnsubscibeSignal>(Dispose);
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

    public void CheckBottles(TakeEffectSignal signal)
    {
        if (BottlesExist.Count == 0)
        {
            eventBus.Invoke(new EndBottlesSignal(CountOfBottles));
        }
    }

    private void AddBottle()
    {
        foreach (var bottle in bottles)
        {
            Spawn(bottle.GetComponent<CommonBottle>());
        }
    }

    public void SetRandomBottles(EndBottlesSignal signal)
    {
        for (int i = 0; i < signal.Count; i++)
        {
            float rand = Random.Range(0, 1.0f);
            foreach (var bottles in bottlesPool)
            {
                if (rand <= bottles.Key)
                {
                    var gm = bottlesPool[bottles.Key].Get();
                    BottlesExist.Add(gm);
                    gm.transform.position = bottlesTransform[i].position;
                    break;
                }
            }
        }
    }

    public void ReleaseBottle(CommonBottle bottle)
    {
        bottlesPool[bottle.weight].Release(bottle);
        soundManager.PlaySound(bottleUseSound, bottle.transform, 1f);
        BottlesExist.Remove(bottle);
    }

    public void Dispose(UnsubscibeSignal signal)
    {
        eventBus.Unsubscribe<TakeEffectSignal>(CheckBottles);
        eventBus.Unsubscribe<EndBottlesSignal>(SetRandomBottles);
    }
}

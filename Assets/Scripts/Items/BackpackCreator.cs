using UnityEngine;

public class BackpackCreator : MonoBehaviour
{
    private BackpackBuilderr backpackBuilder;
    [SerializeField] private Backpack parent;
    [SerializeField] private BackpackModel model;
    [SerializeField] private int capacity;

    private Backpack result;

    private void Awake()
    {
        backpackBuilder = new BackpackBuilderr();
    }

    private void Start()
    {
        result = backpackBuilder
            .Reset()
            .WithRootPrefab(parent)
            .Model(model)
            .Capacity(capacity)
            .Build();

        FindAnyObjectByType<Player>().SetBackpack(result);
    }
}

using UnityEngine;

public class BackpackBuilderr
{
    private Backpack backpack;
    private BackpackModel model;
    private int capacity;
    public BackpackBuilderr WithRootPrefab(Backpack backpack)
    {
        this.backpack = backpack;

        return this;
    }

    public BackpackBuilderr Model(BackpackModel model)
    {
        this.model = model;

        return this;
    }

    public BackpackBuilderr Capacity(int capacity)
    {
        this.capacity = capacity;

        return this;
    }

    public Backpack Build(Transform container = null)
    {
        var createdBackpack = Object.Instantiate(backpack, container);
        var createdModel = Object.Instantiate(model);

        createdBackpack.SetModel(createdModel);
        createdBackpack.Init(capacity);

        return createdBackpack;
    }

    public BackpackBuilderr Reset()
    {
        backpack = null;
        model = null;
        capacity = 0;

        return this;
    }
}

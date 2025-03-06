using UnityEngine;

public class Backpack : MonoBehaviour
{
    public InventorySystem inventorySystem { get; private set; }
    private InventoryCell cell;
    private BackpackModel model;
    private int count;
    [SerializeField] private string path;
    [SerializeField] private float xSpace, ySpace;

    public void Init(int Count)
    {
        count = Count;

        inventorySystem = new InventorySystem(count);
        cell = Resources.Load<InventoryCell>(path);
        CreateInventory();
    }

    public void SetModel(BackpackModel Model)
    {
        model = Model;
        model.transform.SetParent(gameObject.transform);
    }

    public void CreateInventory()
    {
        count = Mathf.Clamp(count, 4, (int)model.size);

        int rows = count / 2;

        for(int i = 0; i < 2; i++)
        {
            for(int j = 0; j < rows; j++)
            {
                var go = Instantiate(cell, this.transform);
                go.transform.position = new Vector3(j, 0, i);
            }
        }
    }

    public void ShowInventory()
    {
        var cell = gameObject.GetComponentsInChildren<InventoryCell>();

        for (int i = 0; i < inventorySystem.inventory.Count; i++)
        {
            inventorySystem.inventory[i].transform.SetParent(cell[i].transform);
            cell[i].SetItem(inventorySystem.inventory[i]);
        }
    }
}

public enum BackpackSize
{
    SMALL = 4,
    MEDUIM = 8,
    LARGE = 12,
    EXTRALARGE = 16
}

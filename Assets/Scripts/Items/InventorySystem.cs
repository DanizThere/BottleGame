using System.Collections.Generic;
using UnityEngine;
public class InventorySystem
{
    public List<ItemObject> inventory;
    private int count;

    public InventorySystem(int Count)
    {
        count = Count;
        inventory = new List<ItemObject>();
    }

    public void Add(ItemObject item)
    {
        if (inventory.Count >= count)
        {
            ServiceLocator.Instance.Get<UIManager>().ShowAnotherText("Вы не можете взять эту вещь, ваш инвентарь заполнен");
            return;
        }
        if (item.stackable)
        {
            inventory.Find(x => x == item).count += item.count;
            return;
        }
        inventory.Add(item);
    }

    public void Remove(ItemObject item)
    {
        if (item.stackable)
        {
            inventory.Find(x => x == item).count -= item.count;
            if(inventory.Find(x => x == item).count <= 0) inventory.Remove(item);
            return;
        }

        inventory.Remove(item);
    }
}

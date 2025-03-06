using UnityEngine;

public class ItemObject : MonoBehaviour, IDescription
{
    [SerializeField] private Item item;
    public int count;
    public bool stackable = false;
    public GameObject itemPrefab;
    public ItemInteract itemInteract;

    private void Awake()
    {
        itemInteract = GetComponent<ItemInteract>();
    }


    public string Description {
        get
        {
            return item.Description;
        }
        set
        {
            item.Description = value;
        }
    }
    public string NameOfInteract
    {
        get
        {
            return item.Title;
        }
        set
        {
            item.Title = value;
        }
    }
}

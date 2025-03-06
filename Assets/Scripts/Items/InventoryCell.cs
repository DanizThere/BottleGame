using UnityEngine;

public class InventoryCell : MonoBehaviour, IInteractable
{
    private Vector3 scaleOrig;
    [SerializeField] private Item item;
    private int count;
    private bool stackable = false;
    private GameObject itemPrefab;
    private ItemInteract itemInteract;

    private void Awake()
    {
        scaleOrig = transform.localScale;
    }

    private void OnEnable()
    {
        StartAnim();
    }

    public void SetItem(ItemObject obj)
    {
        count = obj.count;
        stackable = obj.stackable;
        itemPrefab = obj.itemPrefab;
        itemInteract = obj.itemInteract;


    }

    public async void StartAnim()
    {
        transform.localScale = new Vector3(.1f,.1f,.1f);
        for(float i = 0; i < 1f; i += Time.deltaTime)
        {
            transform.localScale = Vector3.Lerp(transform.localScale, scaleOrig, i * 5);
            await Awaitable.WaitForSecondsAsync(.01f);
        }
    }

    public void Interact()
    {
        if(itemInteract != null)
            itemInteract.Interact();
    }
}

using UnityEngine;

[CreateAssetMenu(menuName = "Create Item")]
public class Item : ScriptableObject
{
    public string ID;
    public GameObject prefab;
    public string Title;
    public string Description;
}

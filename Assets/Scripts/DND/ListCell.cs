using UnityEngine;

public class ListCell : MonoBehaviour, IInteractable, IDescription
{
    protected Player player;
    public Vector3 offset;

    [SerializeField] protected string desc = "����� �� ������ ��������/�������� ���� �����.";
    [SerializeField] protected string nameOf = "���� ������������";


    public string Description { get; set; }
    public string NameOfInteract { get; set; }

    private void Awake()
    {
        player = FindAnyObjectByType<Player>();

        Description = desc;
        NameOfInteract = nameOf;
    }

    public virtual async void SelectCell(Player player)
    {
        ServiceLocator.Instance.Get<UIManager>().listManager.DisableAllButtonsInList();
        Vector3 letterPos = transform.TransformPoint(-offset);
        await player.Flow(letterPos,transform.rotation);
    }

    public void Interact()
    {
        SelectCell(player);
    }

}

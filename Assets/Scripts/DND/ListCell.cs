using UnityEngine;

public class ListCell : MonoBehaviour, IInteractable, IDescription
{
    protected PlayerMove player;
    public Vector3 offset;

    [SerializeField] protected string desc = "����� �� ������ ��������/�������� ���� �����.";
    [SerializeField] protected string nameOf = "���� ������������";

    public string Description { get; set; }
    public string NameOfInteract { get; set; }

    private void Awake()
    {
        player = FindAnyObjectByType<PlayerMove>();

        Description = desc;
        NameOfInteract = nameOf;
    }

    public virtual async void SelectCell()
    {
        ServiceLocator.Instance.Get<UIManager>().listManager.DisableAllButtonsInList();
        Vector3 letterPos = transform.TransformPoint(-offset);
        await player.Flow(letterPos,transform.rotation);
    }

    public virtual void Interact()
    {
        SelectCell();
        ServiceLocator.Instance.Get<GameManager>().SetGameState(GameState.On_UI);
    }

}

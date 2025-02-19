using UnityEngine;

public class PlayerCommon : MonoBehaviour
{
    private CameraMove cam;
    private Player person;
    private EventBus eventBus;
    private void Start()
    {
        eventBus = ServiceLocator.Instance.Get<EventBus>();
    }

    public void Init(Player person, CameraMove cam)
    {
        this.person = person;
        this.cam = cam;
    }

    public bool CanUse(Player player) {
        return player.canUse;
    }

    public void Act()
    {
        

    }

    

    public void Use(IInteractable interactable)
    {
        interactable.Interact();
    }
}

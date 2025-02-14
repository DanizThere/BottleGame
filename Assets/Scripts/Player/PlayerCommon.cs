using UnityEngine;

public class PlayerCommon : MonoBehaviour, IAction
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
        if (CanUse(person) == false) return;

        RaycastHit hit;
        if (Physics.Raycast(cam.MouseOnWorldScreen(), out hit))
        {
            var bottle = hit.collider.GetComponent<CommonBottle>();
            var characterList = hit.collider.GetComponent<CharacterList>();
            var cell = hit.collider.GetComponent<ListCell>();
            if (bottle != null)
            {
                bottle.TakeEffect(person);
            }
            if(characterList != null)
            {
                Use(characterList);
            }
            if(cell != null)
            {
                Use(cell);
            }
            Debug.Log(hit.collider.gameObject.name);
        }

    }

    public void TryBelief(DNDPerson person, CommonBottle bottle)
    {
        if (person.CanBelief())
        {
            if (person.characteristics["Charisma"][1] > Random.Range(0, (int)Dices.D20))
            {
                bottle.TakeEffect(person);
                person.PlusLevelOfStress(30);
            }
            return;
        }
        eventBus.Invoke(new TakeEffectSignal(person.typeOfPerson));
    }

    public void Use(IInteractable interactable)
    {
        interactable.Interact();
    }
}

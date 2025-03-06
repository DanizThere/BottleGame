using System;
using System.Threading;
using UnityEngine;

public class Player : MonoBehaviour, IPlayerInput, IDispose, IEntity
{
    private CameraMove cam;
    private Camera cams;
    private EventBus eventBus;
    private Backpack backpack;
    private PlayerMove playerMove;
    private Mob mob;
    public DNDManipulator dndManipulator { get; private set; }

    public bool canUse { get; private set; }
    public bool isAgreed = false;

    [SerializeField] private Transform orientation;
    [SerializeField] private DNDPerson person;
    [SerializeField] private DNDClasses classes;

    private void Awake()
    {
        cams = Camera.main;
        cam = cams.GetComponent<CameraMove>();

        dndManipulator = new DNDManipulator()
            .SetPerson(person)
            .InitCharacteristics(8, 8, 8, 8, 8, 8)
            .SetName("No name")
            .SetClasses(classes)
            .SetsStartHits(person.characteristics["Dexterity"][1])
            .SetTurnValue((int)TypeOfPerson.PLAYER);
    }

    private void Start()
    {
        eventBus = ServiceLocator.Instance.Get<EventBus>();

        eventBus.Subscribe<StartUseSignal>(SetUseTrue);
        eventBus.Subscribe<StopUseSignal>(SetUseFalse);
        eventBus.Subscribe<UnsubscibeSignal>(Dispose);
    }

    public void SetUseTrue(StartUseSignal signal) => canUse = true;
    public void SetUseFalse(StopUseSignal signal) => canUse = false;

    public void Dispose(UnsubscibeSignal signal)
    {
    }

    public void SetBackpack(Backpack backpack) => this.backpack = backpack;

    public void Act()
    {
        if (!canUse) return;

        RaycastHit hit;
        if (Physics.Raycast(cam.MouseOnWorldScreen(), out hit))
        {
            var bottle = hit.collider.GetComponent<CommonBottle>();
            var interactable = hit.collider.GetComponent<IInteractable>();
            var itemObject = hit.collider.GetComponent<ItemObject>();
            if (bottle != null)
            {
                bottle.TakeEffect(this);
            }
            if (interactable != null)
            {
                Use(interactable);
            }
            if(itemObject != null)
            {
                backpack.inventorySystem.Add(itemObject);
                backpack.ShowInventory();
            }
            Debug.Log(hit.collider.gameObject.name);
        }
    }

    public void Use(IInteractable interactable)
    {
        interactable.Interact();
    }

    public void Cancel()
    {
        eventBus.Invoke(new ChangeStateSignal());
    }

    public void SetMob(Mob mob) => this.mob = mob;

    public void HandleAgree()
    {
        eventBus.Invoke(new StopPlaySignal());
        eventBus.Invoke(new StopUseSignal());
        Debug.Log(isAgreed + " player");
        isAgreed = true;
    }

    public void ResetAgree()
    {
        eventBus.Invoke(new StartPlaySignal());
        eventBus.Invoke(new StartUseSignal());

        isAgreed = false;
    }

}

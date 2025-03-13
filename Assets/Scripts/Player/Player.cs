using System;
using System.Threading;
using UnityEngine;

public class Player : MonoBehaviour, IPlayerInput, IEntity
{
    private CameraMove cam;
    private Camera cams;
    private Backpack backpack;
    private PlayerMove playerMove;
    private Mob mob;
    private GameManager gameManager;
    private DNDManipulator dndManipulator;
    private DNDPerson person;

    private bool canUse;
    private bool isAgreed = false;

    [SerializeField] private Transform orientation;
    [SerializeField] private DNDClasses classes;

    public DNDPerson Person => person;
    private bool CanUse => canUse;
    public bool IsAgreed => isAgreed;
    public DNDManipulator Manipulator => dndManipulator;

    private void Awake()
    {
        cams = Camera.main;
        cam = cams.GetComponent<CameraMove>();

        person = new DNDPerson()
            .SetName("No name")
            .InitCharacteristics(8,8,8,8,8,8)
            .SetTurnValue(person.turnValue);

        dndManipulator = new DNDManipulator()
            .SetPerson(person)
            .SetClasses(classes)
            .SetsStartHits(person.dexterity.Value);
    }

    private void Start()
    {
        gameManager = ServiceLocator.Instance.Get<GameManager>();

        gameManager.StartGame += () => canUse = true;
        gameManager.OnSelected += () => canUse = false;
        gameManager.OnDeath += () => canUse = false;
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
        
    }

    public void SetMob(Mob mob) => this.mob = mob;

    public void HandleAgree()
    {
        Debug.Log(isAgreed + " player");
        isAgreed = true;
    }

    public void ResetAgree()
    {
        isAgreed = false;
    }
}

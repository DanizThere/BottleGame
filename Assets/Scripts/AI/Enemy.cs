using UnityEngine;

public class Enemy : MonoBehaviour, IEntity
{
    private bool isBelieved = false;
    private EventBus eventBus;
    [SerializeField] private DNDPerson person;
    [SerializeField] private DNDClasses classes;
    public DNDManipulator manipulator { get; private set; }
    private Mob mob;
    private Backpack backpack;

    public CommonBottle enchanted { get; private set; }
    public bool isAgreed = true;

    private void Awake()
    {
        manipulator = new DNDManipulator()
            .SetPerson(person)
            .InitCharacteristics(8, 8, 8, 8, 8, 8)
            .SetName("Enemy")
            .SetClasses(classes)
            .SetsStartHits(person.characteristics["Dexterity"][1])
            .SetTurnValue((int)TypeOfPerson.ENEMY);
    }

    private void Start()
    {
        isBelieved = false;

        eventBus = ServiceLocator.Instance.Get<EventBus>();
    }

    public bool IsBelieved()
    {
        return isBelieved;
    }

    public void SetEnchanted(CommonBottle enchanted) => this.enchanted = enchanted;

    public CommonBottle Bottle()
    {
        return enchanted;
    }

    public void SetEnchantedBottle(CommonBottle enchantedBottle) => enchanted = enchantedBottle;
    public void SetMob(Mob mob) => this.mob = mob;
    public void SetBackpack(Backpack backpack) => this.backpack = backpack;

    public void HandleAgree()
    {
        Debug.Log(isAgreed + " enemy");

        isAgreed = false;
    }

    public void ResetAgree()
    {
        isAgreed = true;
    }
}

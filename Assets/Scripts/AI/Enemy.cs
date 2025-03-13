using UnityEngine;

public class Enemy : MonoBehaviour, IEntity
{
    private bool isBelieved = false;
    private DNDPerson person;
    [SerializeField] private DNDClasses classes;
    private DNDManipulator manipulator;
    private Mob mob;
    private Backpack backpack;
    private bool isAgreed = true;
    private CommonBottle chosenBottle;
    private BottlesManager bottlesManager;
    private Turn yourTurn;

    public Turn YourTurn => yourTurn;
    public float BottlePrice
    {
        get
        {
            float price = 0;
            foreach (CommonBottle btls in bottlesManager.BottlesExist)
            {
                price += btls.weight;
            }
            return price;
        }
    }
    public bool IsAgried => isAgreed;
    public CommonBottle ChosenBottle
    {
        get => chosenBottle;
        set => chosenBottle = value;
    }
    public DNDPerson Person => person;
    public int Hits
    {
        get
        {
            return manipulator.person.hits;
        }
        set
        {
            manipulator.person.hits = value;
        }
    }

    public DNDManipulator Manipulator => manipulator;

    private void Awake()
    {
        person = new DNDPerson()
            .SetTurnValue(person.turnValue)
            .SetName("Nameless sorcerer")
            .InitCharacteristics(8, 8, 8, 8, 8, 8);


        manipulator = new DNDManipulator()
            .SetPerson(person)
            .SetClasses(classes)
            .SetsStartHits(person.dexterity.Value);

        yourTurn = new Turn();
        Debug.Log(yourTurn.Name);
    }

    private void Start()
    {
        bottlesManager = ServiceLocator.Instance.Get<BottlesManager>();
        isBelieved = false;
    }

    public bool IsBelieved()
    {
        return isBelieved;
    }

    public void SetEnchanted(CommonBottle enchanted) => ChosenBottle = enchanted;

    public void SetEnchantedBottle(CommonBottle enchantedBottle) => ChosenBottle = enchantedBottle;
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

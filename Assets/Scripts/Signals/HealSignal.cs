using UnityEngine;

public class HealSignal : ISignal
{
    public int HP;
    public TypeOfPerson Person;
    public HealSignal(int hp, TypeOfPerson person)
    {
        HP = hp;
        Person = person;
    }
}

using UnityEngine;

public class DamageSignal : ISignal
{
    public int HP;
    public TypeOfPerson Person;
    public DamageSignal(int hp, TypeOfPerson person)
    {
        HP = hp;
        Person = person;
    }
}

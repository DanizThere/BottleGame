using UnityEngine;

public class TakeEffectSignal : ISignal
{
    public TypeOfPerson Person;

    public TakeEffectSignal(TypeOfPerson person)
    {
        Person = person;
    }
}

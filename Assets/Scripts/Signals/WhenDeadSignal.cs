using UnityEngine;

public class WhenDeadSignal : ISignal
{
    public TypeOfPerson Person;
    public DNDPerson DNDPerson;
    public int existsSavethrows;

    public WhenDeadSignal(DNDPerson person, int existsSavethrows)
    {
        Person = person.typeOfPerson;
        DNDPerson = person;
        this.existsSavethrows = existsSavethrows;
    }
}

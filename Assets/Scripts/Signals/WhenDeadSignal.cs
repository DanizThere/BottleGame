using UnityEngine;

public class WhenDeadSignal : ISignal
{
    public TypeOfPerson Person;
    public DNDManipulator DNDPerson;
    public int existsSavethrows;

    public WhenDeadSignal(DNDManipulator person, int existsSavethrows)
    {
        Person = person.person.typeOfPerson;
        DNDPerson = person;
        this.existsSavethrows = existsSavethrows;
    }
}

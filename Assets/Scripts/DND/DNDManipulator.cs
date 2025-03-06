using UnityEngine;

public class DNDManipulator
{
    public DNDPerson person {  get; private set; }
    public DNDClasses classes { get; private set; }

    public DNDManipulator InitCharacteristics(int characteristicStrength, int characteristicDexterity, int characteristicConstitution, int characteristicIntelligence, int characteristicWisdom, int characteristicCharisma)
    {
        person.characteristics.Add("Strength", new int[] { characteristicStrength, SetValue(characteristicStrength) });
        person.characteristics.Add("Dexterity", new int[] { characteristicDexterity, SetValue(characteristicDexterity) });
        person.characteristics.Add("Constitution", new int[] { characteristicConstitution, SetValue(characteristicConstitution) });
        person.characteristics.Add("Intelligence", new int[] { characteristicIntelligence, SetValue(characteristicIntelligence) });
        person.characteristics.Add("Wisdom", new int[] { characteristicWisdom, SetValue(characteristicWisdom) });
        person.characteristics.Add("Charisma", new int[] { characteristicCharisma, SetValue(characteristicCharisma) });

        return this;
    }

    public DNDManipulator SetPerson(DNDPerson person)
    {
        this.person = person;

        return this;
    }

    public DNDManipulator SetClasses(DNDClasses classes)
    {
        this.classes = classes;

        return this;
    }

    public DNDManipulator SetName(string name)
    {
        person.personName = name;

        return this;
    }

    public DNDManipulator SetsStartHits(int value)
    {
        person.maxHit = SetStartHits(value);
        person.hits = person.maxHit;

        return this;
    }

    public DNDManipulator SetTurnValue(int turnValue)
    {
        person.turnValue = turnValue;

        return this;
    }

    public int SetValue(int characteristic)
    {
        if (characteristic == 1)
        {
            return -5;
        }
        return characteristic / 2 - 5;
    }

    

    public void TakeDamage(int value)
    {
        if (person.intervalHits > 0)
        {
            person.intervalHits -= value;
        }
        else if (value > person.intervalHits)
        {
            int halfHits = value - person.intervalHits;
            person.intervalHits = 0;
            person.hits -= halfHits;
        }
        else person.hits -= value;
    }

    public void TakeHeal(int value)
    {
        person.hits += value;
    }

    public void SetIntervalAfterHeal()
    {
        if (person.hits > person.maxHit && person.intervalHits == 0)
        {
            person.intervalHits = person.hits - person.maxHit;
            person.hits -= person.intervalHits;
        }
        else person.hits = Mathf.Clamp(person.hits, 0, person.maxHit);
    }

    public void MinusLevelOfStress(int value)
    {
        person.levelOfStress -= Mathf.RoundToInt(value * person.stressMultipliyer);
        person.levelOfStress = Mathf.Clamp(person.levelOfStress, 0, person.maxLevelOfStress);
    }

    public void PlusLevelOfStress(int value)
    {
        person.levelOfStress += Mathf.RoundToInt(value * (person.stressMultipliyer * .7f));
        person.levelOfStress = Mathf.Clamp(person.levelOfStress, 0, person.maxLevelOfStress);
    }

    public int SetStartHits(int value)
    {
        int hits = (int)classes.scriptableClasses.diceOfHits + value;
        for (int i = 1; i < classes.classLevel; i++)
        {
            hits += Random.Range(0, (int)classes.scriptableClasses.diceOfHits) + value;
        }
        return hits;
    }

    public void SetInternalHits(int value)
    {
        person.intervalHits = value;
    }

    public void SetHits(int value)
    {
        person.hits = value;
    }

    public void CheckHP()
    {
        if (person.hits <= 0)
        {
            ServiceLocator.Instance.Get<EventBus>().Invoke(new WhenDeadSignal(this, person.savethrowsFromDeath));
            person.savethrowsFromDeath--;
        }
    }

    public int GetCharacteristicValue(string key)
    {
        int[] value = new int[2];
        if(person.characteristics.TryGetValue(key, out value))
        {
            return value[1];
        }
        return 0;
    }

    public int GetCharacteristic(string key)
    {
        int[] value = new int[2];
        if (person.characteristics.TryGetValue(key, out value))
        {
            return value[0];
        }
        return 0;
    }
}

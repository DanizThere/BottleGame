using System;
using UnityEngine;

public class DNDManipulator
{
    public DNDPerson person {  get; private set; }
    public DNDClasses classes { get; private set; }

    public event Action HealthChanged;

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

    public DNDManipulator SetsStartHits(int value)
    {
        person.maxHit = SetStartHits(value);
        person.hits = person.maxHit;

        return this;
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

        HealthChanged?.Invoke();
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
            hits += UnityEngine.Random.Range(0, (int)classes.scriptableClasses.diceOfHits) + value;
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
            person.savethrowsFromDeath--;
        }
    }
}

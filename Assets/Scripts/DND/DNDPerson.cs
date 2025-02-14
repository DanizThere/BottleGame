using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class DNDPerson : MonoBehaviour
{
    #region stats

    public Dictionary<string, int[]> characteristics = new Dictionary<string, int[]>();
    #endregion

    public string personName;
    protected int levelOfStress;
    protected int maxLevelOfStress;
    protected float stressMultipliyer;
    protected DNDClasses personClass;
    public int savethrowsFromDeath { get; protected set; } = 3;
    public int maxHit;
    public int hits;
    public int intervalHits;
    public TypeOfPerson typeOfPerson;

    protected EventBus eventBus;

    public virtual void PersonInit(float stressMultipliyer, DNDClasses personClass, TypeOfPerson typeOfPerson, int maxLevelOfStress, int savethrowsFromDeath, int characteristicStrength, int characteristicDexterity, int characteristicConstitution, int characteristicIntelligence, int characteristicWisdom, int characteristicCharisma, string personName)
    {
        this.maxLevelOfStress = maxLevelOfStress;
        this.levelOfStress = maxLevelOfStress;
        this.stressMultipliyer = stressMultipliyer;
        this.personClass = personClass;
        this.typeOfPerson = typeOfPerson;
        this.savethrowsFromDeath = savethrowsFromDeath;
        this.personName = personName;

        characteristics.Add("Strength", new int[] { characteristicStrength, SetValue(characteristicStrength) });
        characteristics.Add("Dexterity", new int[] { characteristicDexterity, SetValue(characteristicDexterity) });
        characteristics.Add("Constitution", new int[] { characteristicConstitution, SetValue(characteristicConstitution) });
        characteristics.Add("Intelligence", new int[] { characteristicIntelligence, SetValue(characteristicIntelligence) });
        characteristics.Add("Wisdom", new int[] { characteristicWisdom, SetValue(characteristicWisdom) });
        characteristics.Add("Charisma", new int[] { characteristicCharisma, SetValue(characteristicCharisma) });

        maxHit = personClass.SetStartHits(characteristics["Dexterity"][1]);
        SetStartHealth();
    }

    public void SetStartHealth()
    {
        hits = maxHit;
    }
    protected int SetValue(int characteristic)
    {
        if(characteristic == 1)
        {
            return -5;
        }
        return characteristic / 2 - 5;
    }

    public void TakeDamage(int value)
    {
        if (intervalHits > 0)
        {
            intervalHits -= value;
        }
        else if (value > intervalHits)
        {
            int halfHits = value - intervalHits;
            intervalHits = 0;
            hits -= halfHits;
        }
        else hits -= value;
    }

    public abstract void CheckHP();
    public abstract bool CanBelief();

    public void TakeHeal(int value) {
        hits += value;
    }

    public virtual void SetIntervalAfterHeal()
    {
        if (hits > maxHit && intervalHits == 0)
        {
            intervalHits = hits - maxHit;
            hits -= intervalHits;
        }
        else hits = Mathf.Clamp(hits, 0, maxHit);
    }

    public void MinusLevelOfStress(int value)
    {
        levelOfStress -= Mathf.RoundToInt(value * stressMultipliyer);
        levelOfStress = Mathf.Clamp(levelOfStress, 0, maxLevelOfStress);
    }

    public void PlusLevelOfStress(int value)
    {
        levelOfStress += Mathf.RoundToInt(value * (stressMultipliyer * .7f));
        levelOfStress = Mathf.Clamp(levelOfStress, 0, maxLevelOfStress);
    }

    public void SetInternalHits(int value)
    {
        intervalHits = value;
    }

    public void SetHits(int value)
    {
        hits = value;
    }
}

public enum TypeOfPerson
{
    PLAYER,
    ENEMY,
    PRISONER,
    NONE
}

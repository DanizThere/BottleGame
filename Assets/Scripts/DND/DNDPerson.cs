using System;
using System.Collections.Generic;
using TreeEditor;

[Serializable]
public class DNDPerson
{
    public string personName;
    public int levelOfStress;
    public int maxLevelOfStress;
    public float stressMultipliyer;
    public int savethrowsFromDeath;
    public int maxHit;
    public int hits;
    public int intervalHits;
    public TypeOfPerson typeOfPerson;
    public int turnValue;

    public readonly Reactive<int> strength = new Reactive<int>();
    public readonly Reactive<int> dexterity = new Reactive<int>();
    public readonly Reactive<int> constitution = new Reactive<int>();
    public readonly Reactive<int> intelligence = new Reactive<int>();
    public readonly Reactive<int> wisdom = new Reactive<int>();
    public readonly Reactive<int> charisma = new Reactive<int>();

    public int StatsToSpend = 2;

    public DNDPerson InitCharacteristics(int characteristicStrength, int characteristicDexterity, int characteristicConstitution, int characteristicIntelligence, int characteristicWisdom, int characteristicCharisma)
    {
        strength.Value = characteristicStrength;
        dexterity.Value = characteristicDexterity;
        constitution.Value = characteristicConstitution;
        intelligence.Value = characteristicIntelligence;
        wisdom.Value = characteristicWisdom;
        charisma.Value = characteristicCharisma;

        return this;
    }

    public DNDPerson SetName(string name)
    {
        personName = name;

        return this;
    }

    public DNDPerson SetTurnValue(int turnValue)
    {
        this.turnValue = turnValue;

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
}

public enum TypeOfPerson
{
    PLAYER,
    ENEMY,
    NONE
}

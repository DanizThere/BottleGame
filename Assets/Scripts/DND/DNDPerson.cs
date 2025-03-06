using System;
using System.Collections.Generic;

[Serializable]
public class DNDPerson
{
    public Dictionary<string, int[]> characteristics = new Dictionary<string, int[]>();
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
}

public enum TypeOfPerson
{
    PLAYER,
    ENEMY,
    NONE
}

using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[Serializable]
public class JsonData
{
    public string Title;
    public string Description;
}

[Serializable]
public class Characteristics
{
    public string Title;
    public string Description;
}

[Serializable]
public class Classes
{
    public string Name;
    public string Description;
    public string MainCharacteristic;
}

[Serializable] 
public class LevelOfPerson
{
    public int Level;
    public string Description;
    public int StartPoints;
    public int ToNextPoints;
    public int Bonus;
}

[Serializable]
public class Ability
{
    public string Title; 
    public string Description;
    public int LevelOfAbility;
    public string RequiredClass;
}
[Serializable]
public class AllAbilities
{
    public string Title;
    public string Description;
    public int[] LevelOfAbility;
    public List<Ability> Abilities;
}
[Serializable]
public class LevelProgression
{
    public string Title;
    public string Description;
    public List<LevelOfPerson> Levels;
}
[Serializable]
public class AllCharacteristics
{
    public string Title;
    public string Description;
    public List<Characteristics> Characteristics;
}
[Serializable]
public class AllClasses
{
    public string Title;
    public string Description;
    public List<Classes> Classifications;
}

public class DataLoader : MonoBehaviour, IService
{
    public AllAbilities Abilities;
    public LevelProgression LevelProgression;
    public AllCharacteristics AllCharacteristics;
    public AllClasses AllClasses;

    public void Init()
    {
        string abilPath = File.ReadAllText(Application.dataPath + "/Resources/Data/Abilities.json");
        string levelPath = File.ReadAllText(Application.dataPath + "/Resources/Data/LevelProgression.json");
        string charPath = File.ReadAllText(Application.dataPath + "/Resources/Data/Characteristics.json");
        string classPath = File.ReadAllText(Application.dataPath + "/Resources/Data/Classes.json");

        Abilities = JsonUtility.FromJson<AllAbilities>(abilPath);
        LevelProgression = JsonUtility.FromJson<LevelProgression>(levelPath);
        AllCharacteristics = JsonUtility.FromJson<AllCharacteristics>(charPath);
        AllClasses = JsonUtility.FromJson<AllClasses>(classPath);
    }
}

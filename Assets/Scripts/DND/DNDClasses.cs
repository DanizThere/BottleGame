using UnityEngine;

public class DNDClasses : MonoBehaviour
{
    public ScriptableClasses scriptableClasses;
    public int classLevel = 1;
    public int points = 0;
    public int pointsToNextLevel;
    public int pointsPresLevel;
    public int proficiencyBonus = 2;

    private void Start()
    {
        Init();
    }

    public void Init()
    {
        pointsToNextLevel = ServiceLocator.Instance.Get<DNDDictionary>().levelAndPoints[classLevel][1];
        pointsPresLevel = ServiceLocator.Instance.Get<DNDDictionary>().levelAndPoints[classLevel][0];
        if (classLevel % 4 == 0) proficiencyBonus = 2 + classLevel % 4;
    }

    public int SetStartHits(int value)
    {
        int hits = (int) scriptableClasses.diceOfHits + value;
        for (int i = 1; i < classLevel; i++)
        {
            hits += Random.Range(0, (int)scriptableClasses.diceOfHits) + value;
        }
        return hits;
    }
}

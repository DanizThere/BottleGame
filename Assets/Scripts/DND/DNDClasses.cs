using UnityEngine;

public class DNDClasses : MonoBehaviour
{
    public ScriptableClasses scriptableClasses;
    public int classLevel = 1;
    public int points = 0;
    public int pointsToNextLevel = 300;
    public int pointsPresLevel = 0;
    public int proficiencyBonus = 2;

    private void Start()
    {
        Init();
    }

    public void Init()
    {
        LevelOfPerson levelProgression = ServiceLocator.Instance.Get<DataLoader>().LevelProgression.Levels[classLevel];

        points = levelProgression.StartPoints;
        pointsToNextLevel = levelProgression.ToNextPoints;
        proficiencyBonus = levelProgression.Bonus;
    }
}

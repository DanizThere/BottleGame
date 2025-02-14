using UnityEngine;

public class SaveManager : MonoBehaviour, IService
{
    private SaveFile saveFile;
    private SaveData saveData;
    private Player player;

    public void Init(Player player, int currentSaveFile = 1)
    {
        this.player = player;

        saveFile = new SaveFile(currentSaveFile);
        saveData = saveFile.Load(new SaveData());
    }


    public void Save()
    {
        saveData.PersonName = player.personName;
        saveFile.Save(saveData);
    }

    public void Save(int recordScore)
    {
        saveData.PersonName = player.personName;
        saveData.RecordScore = recordScore;
        saveFile.Save(saveData);
    }

    public void SavePoints(int points)
    {
        saveData.Points += points;
        saveFile.Save(saveData);
    }

    public void Load()
    {
        saveData = saveFile.Load(new SaveData());
        player.personName = saveData.PersonName;
    }

    public int LoadPoints()
    {
        saveData = saveFile.Load(new SaveData());
        return saveData.Points;
    }
}

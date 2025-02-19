
using System;
using System.IO;
using UnityEngine;

public class SaveFile
{
    private readonly string filePath;
    public SaveFile(int SaveNumber = 1)
    {
        var directory = Application.persistentDataPath + "/saves";
        if (!Directory.Exists(directory)) Directory.CreateDirectory(directory);
        filePath = directory + $"/SaveFile_{SaveNumber}.json";
    }

    public SaveData Load(SaveData saveData)
    {
        if (!File.Exists(filePath))
        {
            if (saveData != null)
            {
                Save(saveData);
            }
            return saveData;
        }

        string data = File.ReadAllText(filePath);
        var savedData = JsonUtility.FromJson<SaveData>(data);
        return savedData;
    }

    public void Save(SaveData saveData) { 
        string data = JsonUtility.ToJson(saveData);
        File.WriteAllText(filePath, data);
    }
}

public class SettingsFile
{
    private readonly string filePath;
    public SettingsFile()
    {
        var directory = Application.persistentDataPath + "/settings";
        if (!Directory.Exists(directory)) Directory.CreateDirectory(directory);
        filePath = directory + "/PlayerSettings.json";
    }

    public SettingsData Load(SettingsData settingsData)
    {
        if (!File.Exists(filePath))
        {
            if (settingsData != null)
            {
                Save(settingsData);
            }
            return settingsData;
        }

        string data = File.ReadAllText(filePath);
        var savedData = JsonUtility.FromJson<SettingsData>(data);
        return savedData;
    }

    public void Save(SettingsData settingsData)
    {
        string data = JsonUtility.ToJson(settingsData);
        File.WriteAllText(filePath, data);
    }
}


public class SaveData
{
    public string PersonName = "Без имени";
    public int Points = 0;
    public int Stage = 0;
    public int RecordScore = 0;
}

public class SettingsData
{
    public float MasterVolume = .2f;
    public float MusicVolume = .2f;
    public float SfxVolume = .2f;

    public int QualityLevel = 0;
    public int AntiAlising = QualitySettings.antiAliasing;

    public bool screenMode = true;
    public int vSyncCount = QualitySettings.vSyncCount;
    public int ResolutionX = Screen.currentResolution.width;
    public int ResolutionY = Screen.currentResolution.height;
}

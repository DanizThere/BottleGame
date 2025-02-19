using UnityEngine;

public class SettingManager : MonoBehaviour, IService
{
    private SettingsData settingsData;
    private SettingsFile settingsFile;

    public void Awake()
    {
        settingsFile = new SettingsFile();
        settingsData = settingsFile.Load(new SettingsData());
    }

    public void SaveAll(int vSyncCount, Resolution resolution, int QualityLevel, int AntiAlising, bool screenMode, float SfxVolume, float MusicVolume, float MasterVolume)
    {
        settingsData.vSyncCount = vSyncCount;
        settingsData.ResolutionX = resolution.width;
        settingsData.ResolutionY = resolution.height;
        settingsData.QualityLevel = QualityLevel;
        settingsData.AntiAlising = AntiAlising;
        settingsData.screenMode = screenMode;

        settingsData.SfxVolume = SfxVolume;
        settingsData.MasterVolume = MasterVolume;
        settingsData.MusicVolume = MusicVolume;

        settingsFile.Save(settingsData);
    }

    public void SaveSound(float SfxVolume, float MusicVolume, float MasterVolume)
    {
        settingsData.SfxVolume = SfxVolume;
        settingsData.MasterVolume = MasterVolume;
        settingsData.MusicVolume = MusicVolume;
    }

    public void SaveGraphics(int vSyncCount, Resolution resolution, int QualityLevel, int AntiAlising, bool screenMode)
    {
        settingsData.vSyncCount = vSyncCount;
        settingsData.ResolutionX = resolution.width;
        settingsData.ResolutionY = resolution.height;
        settingsData.QualityLevel = QualityLevel;
        settingsData.AntiAlising = AntiAlising;
        settingsData.screenMode = screenMode;
    }

    public SettingsData Load()
    {
        settingsData = settingsFile.Load(new SettingsData());
        return settingsData;
    }
}

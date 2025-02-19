using UnityEngine;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour
{
    private SettingManager settingManager;
    private SettingsData settingsData;
    private Resolution[] resolutions;
    private string[] qualities;

    [SerializeField] private Slider SFX, Music, Master;
    [SerializeField] private TMPro.TMP_Dropdown Quality, Resolution, AntiAlising;
    [SerializeField] private Toggle VSync, Windowed;
    int chosen = 0;
    private void Awake()
    {
        resolutions = Screen.resolutions;
        qualities = QualitySettings.names;
    }

    private void Start()
    {
        settingManager = ServiceLocator.Instance.Get<SettingManager>();
        settingsData = settingManager.Load();
        foreach (var res in resolutions)
        {
            Resolution.options.Add(new TMPro.TMP_Dropdown.OptionData { text = $"{res.width}x{res.height}" });
        }
        foreach (var res in qualities)
        {
            Quality.options.Add(new TMPro.TMP_Dropdown.OptionData { text = res });
        }
        Resolution.RefreshShownValue();
        Quality.RefreshShownValue();

        for (int i = 0; i < Resolution.options.Count; i++) 
        {
            if (Resolution.options[i].text == $"{settingsData.ResolutionX}x{settingsData.ResolutionY}") Resolution.value = i;
        }

        SFX.value = settingsData.SfxVolume;
        Music.value = settingsData.MusicVolume;
        Master.value = settingsData.MasterVolume;

        VSync.isOn = settingsData.vSyncCount > 0;
        Windowed.isOn = settingsData.screenMode;
    }

    public void ChangeResolution(int res)
    {
        Screen.SetResolution(resolutions[res].width, resolutions[res].width, Windowed.isOn);
        chosen = res;
    }

    public void SetWindowed()
    {
        if (Windowed.isOn) Screen.fullScreen = true;
        else Screen.fullScreen = false;
    }

    public void SetVSync()
    {
        if (VSync.isOn) QualitySettings.vSyncCount = 1;
        else QualitySettings.vSyncCount = 0;
    }

    public void ApplyChanges()
    {
        settingManager.SaveAll(QualitySettings.vSyncCount, Screen.currentResolution, QualitySettings.GetQualityLevel(), QualitySettings.antiAliasing, Windowed.isOn, SFX.value, Music.value, Master.value);
    }
}

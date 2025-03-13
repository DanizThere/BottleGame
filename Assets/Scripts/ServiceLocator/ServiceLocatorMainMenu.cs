using UnityEngine;

public class ServiceLocatorMainMenu : MonoBehaviour
{
    [SerializeField] private SettingManager settingManager;
    [SerializeField] private UIManager uiManager;
    [SerializeField] private SoundManager soundManager;
    [SerializeField] private SaveManager saveManager;
    [SerializeField] private DialogueManager dialogueManager;
    [SerializeField] private MusicMan musicManager;
    [SerializeField] private string SoundResourcePath;

    private Player player;

    private void Awake()
    {
        player = FindAnyObjectByType<Player>();

        ServiceLocator.Initialize();

        ServiceLocator.Instance.Register<SettingManager>(settingManager);
        ServiceLocator.Instance.Register<UIManager>(uiManager);
        ServiceLocator.Instance.Register<SoundManager>(soundManager);
        ServiceLocator.Instance.Register<SaveManager>(saveManager);
        ServiceLocator.Instance.Register<DialogueManager>(dialogueManager);
    }

    private void Start()
    {
        saveManager.Init(player);
        soundManager.Init(SoundResourcePath);
        uiManager.Init(() => saveManager, () => dialogueManager);
        musicManager.Init();
    }
}

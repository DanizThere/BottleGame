using UnityEngine;

public class ServiceLocatorLoader : MonoBehaviour
{
    [SerializeField] private GameManager gameManager;
    [SerializeField] private SoundManager soundManager;
    [SerializeField] private DialogueManager dialogueManager;
    [SerializeField] private BottlesManager bottlesManager;
    [SerializeField] private DNDDictionary dictionary;
    [SerializeField] private SaveManager saveManager;
    [SerializeField] private UIManager uiManager;
    [SerializeField] private SettingManager settingManager;
    [SerializeField] private MusicMan musicManager;
    [SerializeField] private DataLoader dataLoader;

    [SerializeField] private string resourcePath;

    private Player player;
    private void Awake()
    {
        player = FindAnyObjectByType<Player>();
        ServiceLocator.Initialize();

        ServiceLocator.Instance.Register<GameManager>(gameManager);
        ServiceLocator.Instance.Register<SoundManager>(soundManager);
        ServiceLocator.Instance.Register<DialogueManager>(dialogueManager);
        ServiceLocator.Instance.Register<DNDDictionary>(dictionary);
        ServiceLocator.Instance.Register<SaveManager>(saveManager);
        ServiceLocator.Instance.Register<UIManager>(uiManager);
        ServiceLocator.Instance.Register<BottlesManager>(bottlesManager);
        ServiceLocator.Instance.Register<SettingManager>(settingManager);
        ServiceLocator.Instance.Register<DataLoader>(dataLoader);

        dataLoader.Init();
        saveManager.Init(player);
        soundManager.Init(resourcePath);
        musicManager.Init();
        bottlesManager.Init(() => soundManager, () => gameManager);
        gameManager.Init(() => saveManager, () => bottlesManager);
        uiManager.Init(() => saveManager, () => dialogueManager);
    }
}


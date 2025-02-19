using System;
using Unity.VisualScripting;
using UnityEngine;

public class ServiceLocatorLoader : MonoBehaviour
{
    //создание Синглтона
    [SerializeField] private GameManager gameManager;
    [SerializeField] private SoundManager soundManager;
    [SerializeField] private DialogueManager dialogueManager;
    [SerializeField] private BottlesManager bottlesManager;
    [SerializeField] private DNDDictionary dictionary;
    [SerializeField] private SaveManager saveManager;
    [SerializeField] private UIManager uiManager;
    [SerializeField] private SettingManager settingManager;

    [SerializeField] private ListManager listManager;

    [SerializeField] private string resourcePath;

    private EventBus eventBus;
    private Player player;
    private void Awake()
    {
        eventBus = new EventBus();
        player = FindAnyObjectByType<Player>();
        ServiceLocator.Initialize();

        ServiceLocator.Instance.Register<GameManager>(gameManager);
        ServiceLocator.Instance.Register<SoundManager>(soundManager);
        ServiceLocator.Instance.Register<EventBus>(eventBus);
        ServiceLocator.Instance.Register<DialogueManager>(dialogueManager);
        ServiceLocator.Instance.Register<DNDDictionary>(dictionary);
        ServiceLocator.Instance.Register<SaveManager>(saveManager);
        ServiceLocator.Instance.Register<UIManager>(uiManager);
        ServiceLocator.Instance.Register<BottlesManager>(bottlesManager);
        ServiceLocator.Instance.Register<SettingManager>(settingManager);

        gameManager.Init(eventBus, player, saveManager);
        saveManager.Init(player);
        soundManager.Init(resourcePath);
        bottlesManager.Init(soundManager, eventBus);
        uiManager.Init(player, listManager, dialogueManager, saveManager, eventBus);
    }

    private void Start()
    {
        
    }
}


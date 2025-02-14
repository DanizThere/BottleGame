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

    [SerializeField] private ListManager listManager;

    [SerializeField] TMPro.TMP_Text whoTell, output;
    [SerializeField] private string resourcePath;

    private TextWriting textWriting;
    private EventBus eventBus;
    private Player player;
    private void OnEnable()
    {
        textWriting = gameManager.AddComponent<TextWriting>();
        eventBus = new EventBus();
        player = FindAnyObjectByType<Player>();
        ServiceLocator.Initialize();

        ServiceLocator.Instance.Register<GameManager>(gameManager);
        ServiceLocator.Instance.Register<SoundManager>(soundManager);
        ServiceLocator.Instance.Register<EventBus>(eventBus);
        ServiceLocator.Instance.Register<TextWriting>(textWriting);
        ServiceLocator.Instance.Register<DialogueManager>(dialogueManager);
        ServiceLocator.Instance.Register<DNDDictionary>(dictionary);
        ServiceLocator.Instance.Register<SaveManager>(saveManager);
        ServiceLocator.Instance.Register<UIManager>(uiManager);
        ServiceLocator.Instance.Register<BottlesManager>(bottlesManager);

        gameManager.Init(eventBus, player, saveManager);
        textWriting.Init(whoTell, output);
        saveManager.Init(player);
        soundManager.Init(resourcePath);
        dialogueManager.Init(player.transform, textWriting, eventBus);
        bottlesManager.Init(soundManager, eventBus);
        uiManager.Init(player, listManager, eventBus);
    }
}

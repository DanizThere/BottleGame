using UnityEngine;

public class UIManager : MonoBehaviour, IService, IDispose
{
    private EventBus eventBus;
    private DeathMenu deathMenu;
    private DialogueManager dialogueManager;
    private TextWriting textWriting;
    public ListManager listManager { get; private set; }
    public CharacterList characterList;
    private Player player;

    [SerializeField] private GameObject DeadMenu;
    [SerializeField] private GameObject DialogueUI;
    [SerializeField] TMPro.TMP_Text whoTell, output;
    [SerializeField] RectTransform ScrollBarForNames;
    [SerializeField] private string resourcePath;

    private TMPro.TMP_Text anotherText;
    private PoolObject<TMPro.TMP_Text> prefabs;
    private SaveManager saveManager;

    public void Init(Player player, ListManager list, DialogueManager dialogueManager, SaveManager saveMan, EventBus eventBusPref)
    {
        deathMenu = DeadMenu.GetComponent<DeathMenu>();
        if (DeadMenu != null) DeadMenu.SetActive(false);
        if (DialogueUI != null) DialogueUI.SetActive(false);
        textWriting = gameObject.AddComponent<TextWriting>();

        eventBus = eventBusPref;
        saveManager = saveMan;
        textWriting.Init(whoTell, output);
        this.player = player;
        this.dialogueManager = dialogueManager;
        this.dialogueManager.Init(player.transform, textWriting, eventBus);
        characterList.Init(this.player);
        listManager = list;
        deathMenu.Init(player.personClass, saveManager);

        anotherText = Resources.Load<TMPro.TMP_Text>(resourcePath);
        prefabs = new PoolObject<TMPro.TMP_Text>(anotherText, 10);
    }

    private void Start()
    {
        

        eventBus.Subscribe<DeathSignal>(Death, 0);
        eventBus.Subscribe<DeathSignal>(deathMenu.ShowInfo, 1);
        eventBus.Subscribe<DialogueSignal>(DialogueStart);
        eventBus.Subscribe<StopDialogueSignal>(DialogueEnd);
        eventBus.Subscribe<HandleTextSignal>(ShowAnotherText);
        eventBus.Subscribe<UnsubscibeSignal>(Dispose);
    }

    public async void ShowAnotherText(HandleTextSignal signal)
    {
        var txt = prefabs.Get();
        txt.transform.SetParent(ScrollBarForNames);
        txt.transform.SetAsLastSibling();
        txt.rectTransform.localPosition = Vector3.zero;

        await textWriting.SayAt(txt, signal.Text, .04f);

        txt.transform.SetParent(null);
        prefabs.Release(txt);
    }

    public void Death(DeathSignal signal)
    {
        DeadMenu.SetActive(true);
    }

    public void DialogueStart(DialogueSignal signal) 
    {
        DialogueUI.SetActive(true);
    }

    public void DialogueEnd(StopDialogueSignal signal)
    {
        DialogueUI.SetActive(false);
    }

    public void Dispose(UnsubscibeSignal signal)
    {
        eventBus.Unsubscribe<DeathSignal>(Death);
        eventBus.Unsubscribe<DeathSignal>(deathMenu.ShowInfo);
        eventBus.Unsubscribe<DialogueSignal>(DialogueStart);
        eventBus.Unsubscribe<StopDialogueSignal>(DialogueEnd);
        eventBus.Unsubscribe<HandleTextSignal>(ShowAnotherText);
    }
}

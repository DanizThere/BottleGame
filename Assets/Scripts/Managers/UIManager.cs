using System;
using UnityEngine;

public class UIManager : MonoBehaviour, IService, IDispose
{
    private Func<EventBus> eventBus;
    private DeathMenu deathMenu;
    private Func<DialogueManager> dialogueManager;
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
    private Func<SaveManager> saveManager;

    public void Init(ListManager list, Func<EventBus> eventBus, Func<SaveManager> saveManager, Func<DialogueManager> dialogueManager)
    {
        player = FindAnyObjectByType<Player>();
        this.eventBus = eventBus;
        this.saveManager = saveManager;
        this.dialogueManager = dialogueManager;
        listManager = list;
        textWriting = gameObject.AddComponent<TextWriting>();
        textWriting.Init(whoTell, output);
        deathMenu = DeadMenu.GetComponent<DeathMenu>();

        if (DeadMenu != null) DeadMenu.SetActive(false);
        if (DialogueUI != null) DialogueUI.SetActive(false);

        anotherText = Resources.Load<TMPro.TMP_Text>(resourcePath);
        prefabs = new PoolObject<TMPro.TMP_Text>(anotherText, 10);

        //if(characterList != null) characterList.Init(this.player.dndManipulator.person);
        this.dialogueManager().Init(textWriting);
    }

    private void Start()
    {
        deathMenu.Init(player.dndManipulator.classes,() => saveManager());

        eventBus().Subscribe<DeathSignal>(Death, 0);
        eventBus().Subscribe<DeathSignal>(deathMenu.ShowInfo, 1);
        eventBus().Subscribe<DialogueSignal>(DialogueStart);
        eventBus().Subscribe<StopDialogueSignal>(DialogueEnd);
        eventBus().Subscribe<HandleTextSignal>(x => { ShowAnotherText(x.Text); });
        eventBus().Subscribe<UnsubscibeSignal>(Dispose);
    }

    public async void ShowAnotherText(string Text)
    {
        var txt = prefabs.Get();
        txt.transform.SetParent(ScrollBarForNames);
        txt.transform.SetAsLastSibling();
        txt.rectTransform.localPosition = Vector3.zero;

        await textWriting.SayAt(txt, Text, .04f);

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
        eventBus().Unsubscribe<DeathSignal>(Death);
        eventBus().Unsubscribe<DeathSignal>(deathMenu.ShowInfo);
        eventBus().Unsubscribe<DialogueSignal>(DialogueStart);
        eventBus().Unsubscribe<StopDialogueSignal>(DialogueEnd);
        eventBus().Unsubscribe<HandleTextSignal>(x => ShowAnotherText(x.Text));
    }
}

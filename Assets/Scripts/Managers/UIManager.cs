using System;
using UnityEngine;

public class UIManager : MonoBehaviour, IService
{
    private TextWriting textWriting;
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
    private Func<DialogueManager> dialogueManager;

    public void Init(Func<SaveManager> saveManager, Func<DialogueManager> dialogueManager)
    {
        player = FindAnyObjectByType<Player>();
        this.saveManager = saveManager;
        this.dialogueManager = dialogueManager;
        textWriting = gameObject.AddComponent<TextWriting>();
        textWriting.Init(whoTell, output);

        if (DeadMenu != null) DeadMenu.SetActive(false);
        if (DialogueUI != null) DialogueUI.SetActive(false);

        anotherText = Resources.Load<TMPro.TMP_Text>(resourcePath);
        prefabs = new PoolObject<TMPro.TMP_Text>(anotherText, 10);

        this.dialogueManager().Init(textWriting);

    }

    private void Start()
    {
        ServiceLocator.Instance.Get<GameManager>().OnDeath += () => DeadMenu.SetActive(true);
        ServiceLocator.Instance.Get<BottlesManager>().ShowBottles += ShowAnotherText;

        //eventBus().Subscribe<DialogueSignal>(DialogueStart);
        //eventBus().Subscribe<StopDialogueSignal>(DialogueEnd);
        // заменить
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

    public void DialogueStart() 
    {
        DialogueUI.SetActive(true);
    }

    public void DialogueEnd()
    {
        DialogueUI.SetActive(false);
    }
}

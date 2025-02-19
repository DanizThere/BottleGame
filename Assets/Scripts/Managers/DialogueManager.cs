using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

public class DialogueManager : MonoBehaviour, IService, IDispose
{
    private Dictionary<string, SimpleSentence> sentences = new Dictionary<string, SimpleSentence>();
    private EventBus eventBus;
    private TextWriting textWriting;
    private Transform player;
    private CancellationTokenSource token = new CancellationTokenSource();

    [SerializeField] private SimpleSentence[] sentencesPrefab;
    
    public void Init(Transform player, TextWriting txtWriting, EventBus bus)
    {
        this.player = player;
        eventBus = bus;
        textWriting = txtWriting;

        eventBus.Subscribe<DialogueSignal>(StartDialogue, 1);
        eventBus.Subscribe<UnsubscibeSignal>(Dispose);

        foreach (var sentence in sentencesPrefab)
        {
            Add(sentence);
        }
    }

    private void FixedUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Space)) {
            CancelString();
        }
    }

    public void Add(SimpleSentence sentence)
    {
        string key = sentence.sentence[0].id;
        if (sentences.ContainsKey(key)) {
#if UNITY_EDITOR
            Debug.LogError($"��� ����: {key}, ��� ����������");
#endif
            return;
        }
        sentences.Add(key, sentence);
    }

    public void Release(SimpleSentence sentence)
    {
        string key = sentence.sentence[0].id;
        if (!sentences.ContainsKey(key))
        {
#if UNITY_EDITOR
            Debug.LogError($"{key} �� ����������");
#endif
            return;
        }

        sentences.Remove(key);
    }

    public async void StartDialogue(DialogueSignal signal)
    {
        if (!sentences.ContainsKey(signal.Key)) 
        {
#if UNITY_EDITOR
            Debug.LogError($"{signal.Key} �� ����������");
#endif
            return;
        }

        await sentences[signal.Key].StartType(ResetToken().Token, eventBus, textWriting, player);
    }

    public void CancelString()
    {
        token.Cancel();
    }

    public CancellationTokenSource ResetToken()
    {
        token = new CancellationTokenSource();
        return token;
    }

    public void Dispose(UnsubscibeSignal signal)
    {
        eventBus.Unsubscribe<DialogueSignal>(StartDialogue);
    }
}

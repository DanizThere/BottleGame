using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

public class DialogueManager : MonoBehaviour, IService, IDispose
{
    private Dictionary<string, SimpleSentence> sentences = new Dictionary<string, SimpleSentence>();
    private EventBus eventBus;
    private Player player;
    private TextWriting textWriting;
    private CancellationTokenSource token = new CancellationTokenSource();

    [SerializeField] private SimpleSentence[] sentencesPrefab;
    
    public void Init(TextWriting txtWriting)
    {
        eventBus = ServiceLocator.Instance.Get<EventBus>();
        player = FindAnyObjectByType<Player>();
        textWriting = txtWriting;

        eventBus.Subscribe<DialogueSignal>(StartDialogue, 1);
        eventBus.Subscribe<UnsubscibeSignal>(Dispose);

        foreach (var sentence in sentencesPrefab)
        {
            Add(sentence);
        }
    }

    public void Add(SimpleSentence sentence)
    {
        string key = sentence.sentence[0].id;
        if (sentences.ContainsKey(key)) {
#if UNITY_EDITOR
            Debug.LogError($"Это ключ: {key}, уже существует");
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
            Debug.LogError($"{key} не существует");
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
            Debug.LogError($"{signal.Key} не существует");
#endif
            return;
        }

        await sentences[signal.Key].StartType(ResetToken().Token, eventBus, textWriting, player.transform);
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

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

public class DialogueManager : MonoBehaviour, IService
{
    private Dictionary<string, SimpleSentence> sentences = new Dictionary<string, SimpleSentence>();
    private Player player;
    private TextWriting textWriting;
    private CancellationTokenSource token = new CancellationTokenSource();

    [SerializeField] private SimpleSentence[] sentencesPrefab;
    
    public void Init(TextWriting txtWriting)
    {
        player = FindAnyObjectByType<Player>();
        textWriting = txtWriting;

        //eventBus.Subscribe<DialogueSignal>(StartDialogue, 1);
        //eventBus.Subscribe<UnsubscibeSignal>(Dispose); ��������

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

    public async void StartDialogue(string key)
    {
        if (!sentences.ContainsKey(key)) 
        {
#if UNITY_EDITOR
            Debug.LogError($"{key} �� ����������");
#endif
            return;
        }

        await sentences[key].StartType(ResetToken().Token, textWriting, player.transform);
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
}

using System;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

public class SimpleSentence : MonoBehaviour
{
    public Sentence[] sentence;

    public async Task StartType(CancellationToken token, TextWriting textWriting, Transform player)
    {
        foreach(var sentence in sentence)
        {
            await textWriting.Say(sentence, player, ServiceLocator.Instance.Get<DialogueManager>().ResetToken().Token);
        }
        //eventBus.Invoke(new StopDialogueSignal());  заменить на метод конца диалога

        return;
    }
}

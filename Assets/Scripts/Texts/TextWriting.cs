using System;
using System.Threading;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;

public class TextWriting : MonoBehaviour, IService
{
    private TMPro.TMP_Text whoTell;
    private TMPro.TMP_Text output;

    public void Init(TMPro.TMP_Text whoTell, TMPro.TMP_Text output)
    {
        this.whoTell = whoTell;
        this.output = output;
    }


    //Написание предложения. Отсутствует скриптовый объект, только сам текст. Использовать как мысли и т.п.
    public async Awaitable Say(string sentence, float duration = .08f, float awaitSec = 1f)
    {
        ClearField();
        foreach (char c in sentence) {
            output.text += c;
            await Awaitable.WaitForSecondsAsync(duration);
        }
        await Awaitable.WaitForSecondsAsync(awaitSec);
    }


    public async Awaitable Say(Sentence sentence, float awaitSec = 1f)
    {
        whoTell.text = sentence.author;
        output.text = sentence.text;
        await Awaitable.WaitForSecondsAsync(awaitSec);
    }

    //Написание предложения. Стандарт. Есть возможность озвучки
    public async Awaitable Say(Sentence sentence, Transform soundTransform, CancellationToken token,float duration = .08f, float awaitSec = 1f)
    {
        int i = 0;
        token.Register(() =>
        {
            i = sentence.text.Length;

            ClearField();

            whoTell.text = sentence.author;
            output.text = sentence.text;

            return;
        });

        if (sentence.voice != null)
        {
            ServiceLocator.Instance.Get<SoundManager>().PlaySound(sentence.voice, soundTransform, 1f);
        }

        whoTell.text = sentence.author;
        output.text = null;

        for (; i < sentence.text.Length; i++)
        {
            output.text += sentence.text[i];
            await Awaitable.WaitForSecondsAsync(duration);
        }

        await Awaitable.WaitForSecondsAsync(awaitSec);

    }

    public async Awaitable SayAt(TMPro.TMP_Text outputField, string Text, float duration = 0.08f, float awaitSet = 1f)
    {
        outputField.text = null;
        for (int i = 0; i < Text.Length; i++)
        {
            outputField.text += Text[i];
            await Awaitable.WaitForSecondsAsync(duration);

        }

        await Awaitable.WaitForSecondsAsync(awaitSet);
    }


    public void ClearField()
    {
        whoTell.text = null;
        output.text = null;
    }

}

using System;
using System.Threading;
using System.Threading.Tasks;
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


    //��������� �����������. ����������� ���������� ������, ������ ��� �����. ������������ ��� ����� � �.�.
    public async Task Say(string sentence, float duration = .08f, float awaitSec = 1f)
    {
        whoTell.text = null;
        output.text = null;
        foreach (char c in sentence) {
            output.text += c;
            await Task.Delay(TimeSpan.FromSeconds(duration));
        }
        await Task.Delay(TimeSpan.FromSeconds(awaitSec));
    }


    public async Task Say(Sentence sentence, float awaitSec = 1f)
    {
        whoTell.text = sentence.author;
        output.text = sentence.text;
        await Task.Delay(TimeSpan.FromSeconds(awaitSec));
    }

    //��������� �����������. ��������. ���� ����������� �������
    public async Task Say(Sentence sentence, Transform soundTransform, CancellationToken token,float duration = .08f, float awaitSec = 1f)
    {
        int i = 0;
        token.Register(() =>
        {
            i = sentence.text.Length;

            whoTell.text = null;
            output.text = null;

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
            await Task.Delay(TimeSpan.FromSeconds(duration));
        }

        foreach (char c in sentence.text)
        {

        }
        await Task.Delay(TimeSpan.FromSeconds(awaitSec));

    }

    public void ClearField()
    {
        whoTell.text = null;
        output.text = null;
    }

}

using System.Threading;
using UnityEngine;

public class MusicMan : MonoBehaviour
{
    private AudioClip[] soundtracks;
    [SerializeField] private string[] soundtracksPath;

    private AudioSource audioSource;
    [SerializeField] private string audioSourcePath;

    CancellationTokenSource cancellationTokenSource;
    CancellationToken token;

    public void Init()
    {
        cancellationTokenSource = new CancellationTokenSource();
        token = cancellationTokenSource.Token;

        var go = Resources.Load<AudioSource>(audioSourcePath);
        audioSource = Instantiate(go);
        soundtracks = new AudioClip[soundtracksPath.Length];
        for(int i = 0; i < soundtracksPath.Length; i++)
        {
            soundtracks[i] = Resources.Load<AudioClip>("Music/"+soundtracksPath[i]);
        }

        CuzcoMusic(token);
    }
    //Кузко, музыку!
    public async Awaitable CuzcoMusic(CancellationToken token)
    { 
        while (!token.IsCancellationRequested)
        {
            AudioClip clip = soundtracks[Random.Range(0, soundtracksPath.Length)];
            audioSource.clip = clip;

            audioSource.Play();

            await Awaitable.WaitForSecondsAsync(clip.length);
        }
    }

    public void QuietSound()
    {
        audioSource.volume = 0.1f;
    }
}

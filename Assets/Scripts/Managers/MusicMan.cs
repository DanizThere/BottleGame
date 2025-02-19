using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

public class MusicMan : MonoBehaviour
{
    private AudioClip[] soundtracks;
    [SerializeField] private string[] soundtracksPath;

    private AudioSource audioSource;
    [SerializeField] private string audioSourcePath;

    private Transform playerPos;
    CancellationTokenSource cancellationTokenSource;
    CancellationToken token;

    public void Init(Transform player)
    {
        playerPos = player;

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
    public async Task CuzcoMusic(CancellationToken token)
    { 
        while (!token.IsCancellationRequested)
        {
            AudioClip clip = soundtracks[Random.Range(0, soundtracksPath.Length)];
            audioSource.clip = clip;

            audioSource.Play();

            await Task.Delay(System.TimeSpan.FromSeconds(clip.length));
            Debug.Log("play next");
        }
    }

    public void QuietSound()
    {
        audioSource.volume = 0.1f;
    }
}

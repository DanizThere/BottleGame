using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

public class SoundManager : MonoBehaviour, IService
{
    private AudioPrefab m_AudioSource;
    private PoolObject<AudioPrefab> audioPrefab;

    public void Init(string resourcePath)
    {
        m_AudioSource = Resources.Load<AudioPrefab>(resourcePath);

        audioPrefab = new PoolObject<AudioPrefab>(m_AudioSource, 10);
    }

    //Создание звука с параметрами (местоположение, сам звук, громкость)
    public async Task PlaySound(AudioClip clip, Transform spawnTransform, float volume)
    {
        var audioSource = audioPrefab.Get();//использование объекта из пула
        audioSource.transform.position = spawnTransform.position;
        audioSource.audioSource.clip = clip;
        audioSource.audioSource.volume = volume;

        float clipLength = audioSource.audioSource.clip.length;
        audioSource.audioSource.Play();

        await Task.Delay(System.TimeSpan.FromSeconds(clipLength)); //задержка, чтобы объект не сразу заносился в пул
        audioPrefab.Release(audioSource);
    }

    public async Task PlaySound(AudioClip clip, Transform spawnTransform, float volume, CancellationToken token)
    {
        var audioSource = audioPrefab.Get();//использование объекта из пула
        audioSource.transform.position = spawnTransform.position;
        audioSource.audioSource.clip = clip;
        audioSource.audioSource.volume = volume;

        float clipLength = audioSource.audioSource.clip.length;
        audioSource.audioSource.Play();

        token.Register(() =>
        {
            audioSource.audioSource.Stop();
        });

        await Task.Delay(System.TimeSpan.FromSeconds(clipLength)); //задержка, чтобы объект не сразу заносился в пул
        audioPrefab.Release(audioSource);
    }

    //Все тоже самое, только случайный звук
    public async Task PlaySoundRandom(AudioClip[] clip, Transform spawnTransform, float volume)
    {
        int random = Random.Range(0, clip.Length);
        var audioSource = audioPrefab.Get();
        audioSource.audioSource.clip = clip[random];
        audioSource.audioSource.volume = volume;

        float clipLength = audioSource.audioSource.clip.length;
        audioSource.audioSource.Play();

        await Task.Delay(System.TimeSpan.FromSeconds(clipLength));
        audioPrefab.Release(audioSource);
    }

}

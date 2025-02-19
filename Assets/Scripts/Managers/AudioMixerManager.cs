using UnityEngine;
using UnityEngine.Audio;

public class AudioMixerManager : MonoBehaviour
{
    [SerializeField] private AudioMixer AudioMixer;

    public void SetMainVolume(float volume)
    {
        AudioMixer.SetFloat("MainVolume", Mathf.Log10(volume) * 20f);
    }

    public void SetSFX(float volume)
    {
        AudioMixer.SetFloat("SFX", Mathf.Log10(volume) * 20f);
    }

    public void SetMusic(float volume)
    {
        AudioMixer.SetFloat("Music", Mathf.Log10(volume) * 20f);
    }
}

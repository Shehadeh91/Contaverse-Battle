using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public static AudioManager current;
    // [Header("Values")]


    [Header("References")]
    public AudioMixer masterMixer;
    public AudioMixer soundMixer;
    public AudioMixer musicMixer;

    void Awake()
    {
        if(current == null)
           current = this;
        else
           Destroy(this);
        DontDestroyOnLoad(gameObject);
    }

    public void ChangeMasterVolume(float volume)
    {
        masterMixer.SetFloat("MasterVolume", ConvertToDecibel(volume));
    }
    public void ChangeMusicVolume(float volume)
    {
        masterMixer.SetFloat("MusicVolume", ConvertToDecibel(volume));
    }
    public void ChangeSoundVolume(float volume)
    {
        masterMixer.SetFloat("SoundVolume", ConvertToDecibel(volume));
    }

    public float ConvertToDecibel(float linearFloat)
    {
        //do some calculations here
        return Mathf.Log10(linearFloat) * 20;
    }
}
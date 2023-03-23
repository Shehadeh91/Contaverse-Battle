using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class PersistentAudioSource : MonoBehaviour
{
    [SerializeField]
    private AudioSource audioSource = null;

    public void Play()
    {
        audioSource.transform.SetParent(null);
        DontDestroyOnLoad(audioSource.gameObject);
        audioSource.Play();
        if (!audioSource.loop)
            Destroy(audioSource.gameObject, audioSource.clip.length + 0.1f);
    }
}
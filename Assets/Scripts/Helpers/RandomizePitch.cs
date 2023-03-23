using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class RandomizePitch : MonoBehaviour
{
    [SerializeField]
    private FloatRangeValue range;
    [SerializeField]
    private AudioSource audioSource = null;

    public void Awake()
    {
        if (audioSource == null)
            audioSource = GetComponent<AudioSource>();
        audioSource.pitch = range.randomValue;
    }
}
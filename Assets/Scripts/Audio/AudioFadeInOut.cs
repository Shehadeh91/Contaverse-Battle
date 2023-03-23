using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioFadeInOut : MonoBehaviour
{
    [SerializeField]
    private List<AudioSource> audioSources = null;
    private float[] startVolumes;
    [SerializeField]
    private bool FadeInOnStart = false;
    [SerializeField]
    private float fadeInTime = 1f;

    void Start()
    {
        if(FadeInOnStart)
            AudioFadeIn(fadeInTime);
    }

    public void AudioFadeIn(float time)
    {
        StopAllCoroutines();
        StartCoroutine(FadeIn(time));
    }
    public void AudioFadeOut(float time)
    {
        StopAllCoroutines();
        StartCoroutine(FadeOut(time));
    }
    private IEnumerator FadeIn(float time)
    {
        startVolumes = new float[audioSources.Count];
        for (int i = 0; i < audioSources.Count; i++)
        {
            startVolumes[i] = audioSources[i].volume;
        }

        if(time > 0)
        {
            float timeElapsed = 0f;
            while (timeElapsed <= time)
            {
                for (int i = 0; i < audioSources.Count; i++)
                {
                    audioSources[i].volume = startVolumes[i] * (timeElapsed/time);
                }
                timeElapsed += Time.deltaTime;
                yield return null;
            }
        }
        else
        {
            for (int i = 0; i < audioSources.Count; i++)
            {
                audioSources[i].volume = startVolumes[i];
            }
        }
    }
    private IEnumerator FadeOut(float time)
    {
        

        if(time > 0)
        {
            startVolumes = new float[audioSources.Count];
            for (int i = 0; i < audioSources.Count; i++)
            {
                startVolumes[i] = audioSources[i].volume;
            }

            float timeElapsed = 0f;
            while (timeElapsed <= time)
            {
                for (int i = 0; i < audioSources.Count; i++)
                {
                    audioSources[i].volume = startVolumes[i] * (1 - (timeElapsed/time));
                }
                timeElapsed += Time.deltaTime;
                yield return null;
            }
        }
        else
        {
            foreach (AudioSource audioSource in audioSources)
            {
                audioSource.volume = 0;
            }
        }
    }
}

using UnityEngine;
using UnityEngine.Audio;
using System.Collections;
using System.Collections.Generic;

public class MusicManager : MonoBehaviour
{
    public static MusicManager current;

    [Header("Values")]
    [SerializeField]
    private GameMusicState[] musicStates = null;
    [SerializeField]
    private float defaultFadeTime = 4f;
    [SerializeField]
    private GameMusicState activeState;
    [SerializeField]
    private MusicClip activeClip;
    [SerializeField]
    private int activeSource;
    [SerializeField]
    private FloatRangeValue pauseBetweenClips = new FloatRangeValue(10f, 100f);

    [Header("References")]
    [SerializeField]
    private AudioSource[] audioSources = null;

    void Awake()
    {
        if (current == null)
            current = this;
        else
            Destroy(this);
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        activeSource = -1;
    }
    public void HandleClipEnd()
    {
        // Debug.Log("Clip has ended");

        //set the activeSource to -1 if no new Clip is being played directly
        if (AudioSourcesArePlaying() == false)
            activeSource = -1;

        // Play a new clip after a pause
        StartCoroutine(PlayRandomClipAfterPause());
    }

    private bool AudioSourcesArePlaying()
    {
        bool isPlaying = false;
        foreach (AudioSource audioSource in audioSources)
        {
            if (audioSource.isPlaying == true)
            {
                isPlaying = true;
                break;
            }
        }

        return isPlaying;
    }

    #region State changing

    public void ChangeState(string newStateName)
    {
        // Debug.Log("Changing MusicState");
        int hashCode = newStateName.GetHashCode();
        GameMusicState newState = null;

        for (int i = 0; i < musicStates.Length; i++)
        {
            if (musicStates[i].stateName.GetHashCode() == hashCode)
            {
                newState = musicStates[i];
                break;
            }
        }
        ChangeState(newState);
    }
    public void ChangeState(GameMusicState newState)
    {
        if (newState != null)
        {
            activeState = newState;
            //stop the music of the old state and directly play the new one
            Play(activeState.GetClip(activeState.startClip));
        }
        else
        {
            Debug.LogError("The requeste MusicState was not found.");
        }
    }

    #endregion

    #region Play Music Clips

    public void PlayRandomClip()
    {
        if (activeState != null)
            Play(activeState.GetRandomClip());
    }

    public void PlaySpecificClip(string clipName)
    {
        if (activeState != null)
            Play(activeState.GetClip(clipName));
    }
    private IEnumerator PlayRandomClipAfterPause()
    {
        yield return new WaitForSeconds(pauseBetweenClips.randomValue);

        // Check if a clip has been played while waiting
        // if not play a new random clip
        if (AudioSourcesArePlaying() == false)
            PlayRandomClip();
    }
    private void Play(MusicClip musicClip)
    {
        // Debug.Log($"Playing {musicClip.trackName} now.");
        activeClip = musicClip;

        activeState.lastClip = musicClip.trackName;

        //if none of the audiosources are playing activeSource should be -1
        if (activeSource == -1 || AudioSourcesArePlaying() == false)
        {
            activeSource = 0;
            StartCoroutine(FadeIn(audioSources[0], musicClip, defaultFadeTime));
        }
        // a musicclip is already playing, so crossfade them
        else
        {
            //Fade out the old active audiosource
            StartCoroutine(FadeOut(audioSources[activeSource], defaultFadeTime));

            //loop through the audiosources, to get the next one available
            activeSource++;
            if (activeSource >= audioSources.Length)
                activeSource = 0;

            // Fade in the new active audiosource
            StartCoroutine(FadeIn(audioSources[activeSource], musicClip, defaultFadeTime));
        }
    }

    #endregion

    #region Fades
    private IEnumerator FadeIn(AudioSource audioSource, MusicClip musicClip, float fadeTime)
    {
        audioSource.volume = 0;
        audioSource.PlayOneShot(musicClip.audioClip);
        // Debug.Log($"Played {musicClip.trackName}.");

        while (audioSource.volume < musicClip.volume)
        {
            audioSource.volume += Time.deltaTime / fadeTime;
            if (audioSource.volume > musicClip.volume)
                audioSource.volume = musicClip.volume;

            yield return null;
        }

        // if(UIManager.current != null && UIManager.current.musicUI != null)
        // {
        //     UIManager.current.musicUI.ShowTrackInfo(musicClip);
        // }

        yield return new WaitUntil(() => audioSources[activeSource].isPlaying == false);
        HandleClipEnd();
    }
    private IEnumerator FadeOut(AudioSource audioSource, float fadeTime)
    {
        float startVolume = audioSource.volume;
        while (audioSource.volume > 0)
        {
            audioSource.volume -= startVolume * (Time.deltaTime / fadeTime);
            if (audioSource.volume < 0)
                audioSource.volume = 0;

            yield return null;
        }
        audioSource.Stop();
        if (audioSources[activeSource].isPlaying == false)
            activeSource = -1;
    }

    #endregion
}
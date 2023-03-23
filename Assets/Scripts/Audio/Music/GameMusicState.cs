using UnityEngine;
using UnityEngine.Audio;

[CreateAssetMenu(menuName = "Game/Audio/Music State")]
public class GameMusicState : ScriptableObject
{
    [Header("Values")]
    public string stateName;
    public string startClip;
    public MusicClip[] musicClips;
    [HideInInspector]
    public string lastClip;

    #region Get Music Clips
    
    public MusicClip GetRandomClip()
    {
        int totalWeights = 0;
        for (int i = 0; i < musicClips.Length; i++)
        {
            totalWeights += musicClips[i].weight;
        }

        int randomInt = Random.Range(0, totalWeights);
        int weightsChecked = 0;

        MusicClip returnClip = null;

        while (returnClip == null || returnClip.trackName == lastClip)
        {
            for (int i = 0; i < musicClips.Length; i++)
            {
                weightsChecked += musicClips[i].weight;
                if(randomInt <= weightsChecked)
                {
                    returnClip = musicClips[i];
                }
            }
        }

        return returnClip;
    }
    public MusicClip GetClip(string trackName)
    {
        int hashCode = trackName.GetHashCode();
        for (int i = 0; i < musicClips.Length; i++)
        {
            if(musicClips[i].trackName.GetHashCode() == hashCode)
            {
                return musicClips[i];
            }
        }
        Debug.LogError("Song with name " + trackName + " not found");
        return null;
    }

    #endregion
}
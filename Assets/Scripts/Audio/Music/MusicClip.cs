using UnityEngine;

[CreateAssetMenu(menuName = "Game/Audio/Music Clip")]
public class MusicClip : ScriptableObject
{
    #region Values

    public string trackName;
    public string artistName;
    public AudioClip audioClip;
    public float volume = 1f;
    public int weight = 1;

    #endregion
}
using UnityEngine;

public class PlayMusicState : MonoBehaviour
{
    [SerializeField]
    private GameMusicState musicState;
    public void SetState()
    {
        // Debug.Log("Setting the state to " + newState);
        if(MusicManager.current != null)
            MusicManager.current.ChangeState(musicState);
        else
            Debug.LogError("This script is trying to access the MusicManager, but no MusicManager was found.");
    }
    public void SetState(string newState)
    {
        // Debug.Log("Setting the state to " + newState);
        if(MusicManager.current != null)
            MusicManager.current.ChangeState(newState);
        else
            Debug.LogError("This script is trying to access the MusicManager, but no MusicManager was found.");
    }
}
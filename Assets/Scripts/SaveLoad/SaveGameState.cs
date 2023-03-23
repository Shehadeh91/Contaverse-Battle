using UnityEngine;

public class SaveGameState : MonoBehaviour
{
    public void SaveGlobalVariables()
    {
        SaveManager.Instance.SaveGameState();
    }
}
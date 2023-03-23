using UnityEngine;

public class RaiseGameEvent : MonoBehaviour
{
    [SerializeField]
    private GameEvent gameEvent;


    public void RaiseEvent()
    {
        gameEvent.Raise();
    }
}
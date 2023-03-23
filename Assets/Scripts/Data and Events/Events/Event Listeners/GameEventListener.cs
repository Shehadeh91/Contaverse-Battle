using UnityEngine;
using UnityEngine.Events;

public class GameEventListener : MonoBehaviour, iGameEventListener
{
    [Header("Events")]
    public GameEvent[] gameEvents;
    public UnityEvent unityEvent;

    private void OnEnable()
    {
        foreach (GameEvent gameEvent in gameEvents)
        {
            gameEvent.AddListener(this);
        }
    }
    void OnDisable()
    {
        foreach (GameEvent gameEvent in gameEvents)
        {
            gameEvent.RemoveListener(this);
        }
    }
    public void OnEventRaised(GameEvent source)
    {
        unityEvent.Invoke();
    }
}
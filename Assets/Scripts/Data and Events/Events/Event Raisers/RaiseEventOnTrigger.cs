using UnityEngine;

public class RaiseEventOnTrigger : MonoBehaviour
{
    [SerializeField]
    private GameEvent enterEvent;
    [SerializeField]
    private GameEvent exitEvent;

    void OnTriggerEnter(Collider other)
    {
        enterEvent?.Raise();
    }
    void OnTriggerExit(Collider other)
    {
        exitEvent?.Raise();
    }
}
using UnityEngine;
using UnityEngine.Events;

public class MonoBehaviourEvents : MonoBehaviour
{
    [SerializeField]
    private UnityEvent OnAwake = null;
    [SerializeField]
    private UnityEvent OnStart = null;
    [SerializeField]
    private UnityEvent OnEnabled = null;
    [SerializeField]
    private UnityEvent OnDisabled = null;
    [SerializeField]
    private UnityEvent OnDestroyed = null;

    private void Awake()
    {
        OnAwake.Invoke();
    }

    private void Start()
    {
        OnStart.Invoke();
    }
    private void OnEnable()
    {
        OnEnabled.Invoke();
    }
    private void OnDisable()
    {
        OnDisabled.Invoke();
    }
    private void OnDestroy()
    {
        OnDestroyed.Invoke();
    }
}
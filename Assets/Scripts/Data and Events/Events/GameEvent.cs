using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(menuName = "Variable/Game Event")]
public class GameEvent : ScriptableObject
{
    private List<iGameEventListener> listeners = new List<iGameEventListener>();

    public virtual void Raise()
    {
        for (int i = listeners.Count-1; i >= 0; i--)
        {
            listeners[i].OnEventRaised(this);
        }
    }

    public void AddListener(iGameEventListener listener)
    {
        listeners.Add(listener);
    }
    public void RemoveListener(iGameEventListener listener)
    {
        listeners.Remove(listener);
    }
}
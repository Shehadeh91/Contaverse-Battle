using UnityEngine;
using UnityEngine.Events;

public class UIDropSlot : MonoBehaviour
{
    public UnityEvent onItemDropped;
    // public void OnItemHover()
    // {

    // }
    public void OnItemDropped(UIDraggable draggable)
    {
        onItemDropped?.Invoke();
    }
}
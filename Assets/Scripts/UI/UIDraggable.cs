using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Sirenix.OdinInspector;

[System.Serializable]
public class DropSlotEvent : UnityEvent<UIDropSlot>{}

public class UIDraggable : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private UIDropSlot resetSlot;
    private int resetSiblingIndex;
    private GameObject slotHolder;
    [SerializeField] private UIDropSlot currentSlot;
    [ShowInInspector, ReadOnly] private int returnSiblingIndex;
    private Canvas canvas;
    private GraphicRaycaster graphicRaycaster;

    public UnityEvent onBeginDrag, onEndDrag;
    public DropSlotEvent onDrop;

    void Start()
    {
        if (!canvas)
        {
            canvas = GetComponentInParent<Canvas>();
            graphicRaycaster = canvas.GetComponent<GraphicRaycaster>();
        }
        if(currentSlot == null)
        {
            currentSlot = GetComponentInParent<UIDropSlot>();
        }

        returnSiblingIndex = transform.GetSiblingIndex();
        resetSiblingIndex = returnSiblingIndex;
        resetSlot = currentSlot;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        StopAllCoroutines();
        Debug.Log($"Dragged {gameObject.name}");

        if(slotHolder == null)
        {
            slotHolder = new GameObject("slotHolder", typeof(RectTransform));
            slotHolder.transform.SetParent(transform.parent);
            slotHolder.transform.SetSiblingIndex(returnSiblingIndex);
        }

        transform.SetParent(canvas.transform, true);
        transform.SetAsLastSibling();

        onBeginDrag?.Invoke();
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.localPosition += new Vector3(eventData.delta.x, eventData.delta.y, 0);
    }
    
    public void OnEndDrag(PointerEventData eventData)
    {
        var results = new List<RaycastResult>();
        graphicRaycaster.Raycast(eventData, results);

        bool wasDropped = false;
        foreach (var hit in results)
        {
            // If we found slot.
            var slot = hit.gameObject.GetComponent<UIDropSlot>();
            if(slot == currentSlot)
                break;
            if (slot != null)
            {
                wasDropped = true;
                Drop(slot);
                break;
            }
        }
        if(!wasDropped)
            StartCoroutine(MoveBack());
    }

    private void Drop(UIDropSlot slot)
    {
        Debug.Log($"Dropped {gameObject.name}");
        currentSlot = slot;
        currentSlot.OnItemDropped(this);
        returnSiblingIndex = 0;
        transform.SetParent(currentSlot.transform);
        transform.SetSiblingIndex(returnSiblingIndex);
        onDrop?.Invoke(slot);
    }

    public void Reset()
    {
        Debug.Log($"Reset {gameObject.name}");
        DestroyImmediate(slotHolder);
        resetSlot.OnItemDropped(this);
        currentSlot = resetSlot;
        returnSiblingIndex = resetSiblingIndex;

        transform.SetParent(currentSlot.transform);
        transform.SetSiblingIndex(returnSiblingIndex);
        onEndDrag?.Invoke();
    }

    private IEnumerator MoveBack()
    {
        onEndDrag?.Invoke();
        while(true)
        {
            transform.position = Vector3.Lerp(transform.position, slotHolder.transform.position, 0.4f);
            if((transform.position - slotHolder.transform.position).sqrMagnitude < 5f)
                break;
            yield return null;
        }

        DestroyImmediate(slotHolder);
        transform.SetParent(currentSlot.transform);
        transform.SetSiblingIndex(returnSiblingIndex);
    }
}
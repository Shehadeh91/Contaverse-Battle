using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

public class ShowerScript : MonoBehaviour
{
    [TabGroup("Properties")] [SerializeField] private float secondsToStart;
    [TabGroup("References")] [SerializeField] private BoxCollider showerCollider;
    [TabGroup("Events")] [SerializeField] private UnityEvent onShowerStart, onShowerEnd;

    private void Start()
    {
        showerCollider.enabled = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        BubbleScript bubble = other.GetComponent<BubbleScript>();
        if (bubble == null)
            return;
        bubble.CleanBubble(transform.position);
    }

    public void StartShower()
    {
        StartCoroutine(WaitForWater());
        onShowerStart?.Invoke();
    }

    public void StopShower()
    {
        showerCollider.enabled = false;
        onShowerEnd?.Invoke();
    }

    private IEnumerator WaitForWater()
    {
        yield return new WaitForSeconds(secondsToStart);
        showerCollider.enabled = true;
    }
}

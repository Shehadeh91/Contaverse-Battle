using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BubbleScript : MonoBehaviour
{
    [SerializeField] private float minTimeSeconds, maxTimeSeconds;
    public Action onCleaned;
    private bool hasBeenCleaned = false;

    public void CleanBubble(Vector3 showerPos)
    {
        if(hasBeenCleaned)
            return;
        hasBeenCleaned = true;
        StartCoroutine(BubbleDisappear(showerPos));
        onCleaned?.Invoke();
    }

    public IEnumerator BubbleDisappear(Vector3 showerPos)
    {
        float bubbleScale = transform.localScale.y;
        float secondsToDisappear = Mathf.Clamp((showerPos.y - Mathf.Abs(showerPos.y - transform.position.y)) / 2f, 
            minTimeSeconds, maxTimeSeconds);
        float disappearSpeed = minTimeSeconds / 0.02f + maxTimeSeconds / 0.02f - secondsToDisappear * 50f;
        float diff = bubbleScale / disappearSpeed;

        while (bubbleScale > 0f)
        {
            bubbleScale -= diff;
            transform.localScale = new Vector3(bubbleScale, bubbleScale, bubbleScale);
            yield return new WaitForFixedUpdate();
        }
        Destroy(gameObject);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using Contaquest.Mobile.Input;

public class Soap : MonoBehaviour
{
    [TabGroup("Properties")] [SerializeField] float cleaningPerBubble;
    [TabGroup("Properties")] [SerializeField] private float bubbleCreateSpeed = 0.2f;
    [TabGroup("Properties")] [SerializeField] private LayerMask layerMask;

    [TabGroup("References")] [SerializeField] private FloatRangeReference dirtiness;
    [TabGroup("References")] [SerializeField] private BubbleScript bubblePrefab;

    public static List<BubbleScript> bubblesList = new List<BubbleScript>();

    public void StartCreateBubbles(TouchInputAction touchInputAction)
    {
        StartCoroutine(CreateBubbles(touchInputAction));
    }

    public void StopCreateBubbles()
    {
        StopAllCoroutines();
    }

    private IEnumerator CreateBubbles(TouchInputAction touchInputAction)
    {
        while (true)
        {
            TryCreateBubble(touchInputAction);
            yield return new WaitForSeconds(bubbleCreateSpeed);
        }
    }

    private void TryCreateBubble(TouchInputAction touchInputAction)
    {
        Ray ray = ReferencesManager.Instance.MainCamera.ViewportPointToRay(touchInputAction.currentPosition);
        float rayDistance = 7f;
        Vector3 bubblePos = Vector3.zero;
        //Debug.DrawRay(ray.origin, ray.direction * rayDistance, Color.red);
        Debug.DrawRay(ray.origin, ray.direction, Color.red, 0.1f);
        if (Physics.Raycast(ray, out RaycastHit rayHit, rayDistance, layerMask))
        {
            Washable washablePart = rayHit.collider.GetComponent<Washable>();

            if (washablePart == null)
                return;

            bubblePos = rayHit.point + (rayHit.normal * 0.1f);

            BubbleScript newBubble = Instantiate(bubblePrefab, bubblePos, Quaternion.identity, washablePart.GetParentTransform());
            bubblesList.Add(newBubble);
            newBubble.onCleaned += Clean;
        }
    }

    public void Clean()
    {
        if (dirtiness.Value.Value > 0)
        {
            dirtiness.Value.SetValueClamped(dirtiness.Value.Value - cleaningPerBubble);
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Contaquest.Mobile.Input
{
    public class TouchInteractionDetector : GenericSingleton<TouchInteractionDetector>
    {
        [TabGroup("Properties")]
        [SerializeField] private LayerMask layerMask;
        [SerializeField] private LayerMask UILayerMask;
        [SerializeField] private BoolReference isInMenu;

        public void TryStartTouchInteraction(TouchInputAction inputTouchAction)
        {
            if(isInMenu.Value)
                return;
            if(IsPositionOverUIElement(ScreenUtility.ViewportToScreen(inputTouchAction.currentPosition)))
                return;
            if (EventSystem.current.IsPointerOverGameObject())
                return;
            Ray ray = ReferencesManager.Instance.MainCamera.ViewportPointToRay(inputTouchAction.currentPosition);

            if (Physics.Raycast(ray, out RaycastHit hit, 100f, layerMask))
            {
                iTouchInteractable touchInteractable = hit.collider.GetComponent<iTouchInteractable>();

                if (touchInteractable != null)
                {
                    // Debug.Log("Touch Started " + hit.collider.gameObject.name);

                    touchInteractable.StartTouchInteraction(inputTouchAction);
                }
            }
        }
        public void EndTouchInteraction(TouchInputAction inputTouchAction)
        {
            if(isInMenu.Value)
                return;
            if(IsPositionOverUIElement(ScreenUtility.ViewportToScreen(inputTouchAction.currentPosition)))
                return;
            if (EventSystem.current.IsPointerOverGameObject())
                return;
            Ray ray = ReferencesManager.Instance.MainCamera.ViewportPointToRay(inputTouchAction.currentPosition);

            if (Physics.Raycast(ray, out RaycastHit hit, 100f, layerMask))
            {
                iTouchInteractable touchInteractable = hit.collider.GetComponent<iTouchInteractable>();

                if (touchInteractable != null)
                {
                    //Debug.Log($"Touch Ended hit {hit.transform.gameObject.name}");
                    touchInteractable.EndTouchInteraction(inputTouchAction);
                }
            }
        }

        public bool IsPositionOverUIElement(Vector2 position)
        {
            return IsPointerOverUIElement(GetEventSystemRaycastResults(position));
        }

        private bool IsPointerOverUIElement(List<RaycastResult> eventSystemRaysastResults)
        {
            for (int index = 0; index < eventSystemRaysastResults.Count; index++)
            {
                RaycastResult curRaysastResult = eventSystemRaysastResults[index];
                if (curRaysastResult.gameObject.layer == UILayerMask)
                    return true;
            }
            return false;
        }
    
    
        //Gets all event system raycast results of current mouse or touch position.
        static List<RaycastResult> GetEventSystemRaycastResults(Vector2 position)
        {
            PointerEventData eventData = new PointerEventData(EventSystem.current);
            eventData.position = position;
            List<RaycastResult> raysastResults = new List<RaycastResult>();
            EventSystem.current.RaycastAll(eventData, raysastResults);
            return raysastResults;
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace Contaquest.Mobile.Input
{
    [RequireComponent(typeof(Collider))]
	public class TouchableObject : MonoBehaviour, iTouchInteractable
    {
        [SerializeField] private Collider col;
        [SerializeField] private bool enableOnStart = false;

        [Tooltip("The time (in seconds) it takes for a tap to count as a hold.")]
        [SerializeField] private float holdThresholdTime = 1.0f;
		[SerializeField] [FormerlySerializedAs("onTouched")] private TouchInputUnityEvent onTapped;
        [SerializeField] [FormerlySerializedAs("onHeld")] private TouchInputUnityEvent onHeld;

        protected virtual void Start()
        {
            if(enableOnStart)
                EnableInteractability();
            else
                DisableInteractability();
        }

        public void EnableInteractability()
        {
            col.enabled = true;
        }

        public void DisableInteractability()
        {
            col.enabled = false;
        }

        public void StartTouchInteraction(TouchInputAction touchInputAction)
        {
            
        }

        public void EndTouchInteraction(TouchInputAction touchInputAction)
        {
            if (touchInputAction.Duration < holdThresholdTime)
                OnTapped(touchInputAction);
            else
                OnHeld(touchInputAction);
        }

        protected virtual void OnTapped(TouchInputAction touchInputAction)
        {
            onTapped.Invoke(touchInputAction);
        }
        protected virtual void OnHeld(TouchInputAction touchInputAction)
        {
            onHeld.Invoke(touchInputAction);
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            col = GetComponent<Collider>();
        }
#endif
    }
}

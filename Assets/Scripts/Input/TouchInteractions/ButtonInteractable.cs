using UnityEngine;
using Sirenix.OdinInspector;
using UnityEngine.Events;

namespace Contaquest.Mobile.Input
{
    [RequireComponent(typeof(Collider))]
    public class ButtonInteractable : TouchInteractable
    {
        [TabGroup("Properties")] [SerializeField] private bool isTapButton;

        [TabGroup("State")] [ReadOnly] private bool isActivated = false;

        [TabGroup("Events")] [SerializeField] private UnityEvent onButtonDeactivated;
        [TabGroup("Events")] [SerializeField] private UnityEvent onButtonActivated;

        public BoolUnityEvent onPressed;

        private void OnValidate()
        {
            if (col == null)
                col = GetComponent<Collider>();
        }

        public override void StartTouchInteraction(TouchInputAction touchInputAction)
        {
            if(isTapButton)
            {
                touchInputAction.onEndTouch.AddListener(DeactivateButton);
                onPressed?.Invoke(true);
                ActivateButton();
                return;
            }
            
            onPressed?.Invoke(!isActivated);

            if (isActivated)
                DeactivateButton();
            else
                ActivateButton();
        }

        public override void EndTouchInteraction(TouchInputAction touchInputAction)
        {
            if (isTapButton)
            {
                DeactivateButton(touchInputAction);
            }
        }

        public void ActivateButton()
        {
            if (isActivated)
                return;

            onButtonActivated?.Invoke();
            isActivated = true;
        }
        public void DeactivateButton()
        {
            if (!isActivated)
                return;

            onButtonDeactivated?.Invoke();
            isActivated = false;
        }
        public void DeactivateButton(TouchInputAction touchInputAction)
        {
            touchInputAction.onEndTouch.RemoveListener(DeactivateButton);
            DeactivateButton();
        }
    }
}

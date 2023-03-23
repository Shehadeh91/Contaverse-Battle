using System;
using Contaquest.Mobile.Input;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Contaquest.Mobile.Input
{
    [RequireComponent(typeof(Collider))]
    public class TouchInteractable : MonoBehaviour, iTouchInteractable
    {
        [SerializeField] private bool enableOnStart = false;

        [TabGroup("References")] [SerializeField] protected Collider col;

        private void OnValidate()
        {
            if (col == null)
                col = GetComponent<Collider>();
        }

        protected virtual void Start()
        {
            if (enableOnStart)
                EnableInteractability();
            //else
                //DisableInteractability();
        }

        public virtual void EnableInteractability()
        {
            // Debug.Log($"Enabling Interactability for GameObject {gameObject.name}", gameObject);
            col.enabled = true;
        }

        public virtual void DisableInteractability()
        {
            // Debug.Log($"Disabling Interactability for GameObject {gameObject.name}", gameObject);
            col.enabled = false;
        }

        public virtual void StartTouchInteraction(TouchInputAction touchInputAction)
        {
            // if(TouchInteractionDetector.Instance.activeInteractable == null)
            // {
            //     TouchInteractionDetector.Instance.activeInteractable = this;
            // }
            // else
            // {
            //     return;
            // }
        }

        public virtual void EndTouchInteraction(TouchInputAction touchInputAction)
        {

        }
    }
}

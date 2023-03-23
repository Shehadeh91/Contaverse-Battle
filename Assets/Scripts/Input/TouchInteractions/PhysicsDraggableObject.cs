using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

namespace Contaquest.Mobile.Input.Drag
{
    public class PhysicsDraggableObject : DraggableObject
    {
        [TabGroup("Properties")] [SerializeField] private float forceStrength;
        [TabGroup("Properties")] [SerializeField] private bool addRandomTorqueOnStart;

        [TabGroup("References")] [SerializeField] private Rigidbody rBody;

        #region Draggable Methods

        public override void StartTouchInteraction(TouchInputAction inputTouchAction)
        {
            rBody.isKinematic = false;
            if(addRandomTorqueOnStart)
                rBody.AddTorque(new Vector3(1.2f, 0.5f, 0.765f) * 10);
            base.StartTouchInteraction(inputTouchAction);
        }

        protected override void PerformDragUpdate()
        {
            Vector3 viewportPosition = (Vector3)inputTouchAction.currentPosition + offset;

            Vector3 targetPosition = activeCamera.ViewportToWorldPoint(viewportPosition);
            rBody.AddForce((targetPosition - transformToMove.position) * forceStrength);
        }

        public override void OnTouchReleased(TouchInputAction inputTouchAction)
        {
            rBody.isKinematic = true;

            base.OnTouchReleased(inputTouchAction);
        }

        #endregion
    }
}

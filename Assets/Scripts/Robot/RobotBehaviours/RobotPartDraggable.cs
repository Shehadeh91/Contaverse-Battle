using System;
using Contaquest.Metaverse.Robot;
using Contaquest.Mobile.Input;
using Contaquest.Mobile.Input.Drag;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

namespace Contaquest.Metaverse.Behaviours
{
    public class RobotPartDraggable : DraggableObject
    {
        [TabGroup("References")]
        [SerializeField]
        private PartEquipHandler partEquipHandler;
        [TabGroup("References")]
        [SerializeField]
        private RobotPartBehaviour robotPartBehaviour;
        [TabGroup("Events")]
        [SerializeField]
        private UnityEvent onReachedStartPos, onReachedTargetPos;

        public override void StartTouchInteraction(TouchInputAction inputTouchAction)
        {
            base.StartTouchInteraction(inputTouchAction);
            offset.x = 0f;
            offset.y = 0f;
            robotPartBehaviour.StartGravitateTowardsTransform(transformToMove, false, null);
        }

        protected override void PerformDragUpdate()
        {
            Vector3 viewportPosition = (Vector3)inputTouchAction.currentPosition + offset;

            transformToMove.position = activeCamera.ViewportToWorldPoint(viewportPosition);
        }

        public override void OnTouchReleased(TouchInputAction inputTouchAction)
        {
            bool equipSuccessful = partEquipHandler.TryEquipPart();

            if(!equipSuccessful)
                ReturnToStartPos();

            base.OnTouchReleased(inputTouchAction);
            if (equipSuccessful)
                DisableInteractability();
        }

        private void ReturnToStartPos()
        {
            transformToMove.position = startPositionTransform.position;
            robotPartBehaviour.StartGravitateTowardsTransform(startPositionTransform, true, () => onReachedStartPos.Invoke());
        }
    }
}

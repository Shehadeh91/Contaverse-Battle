using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Contaquest.Metaverse.Rooms
{
    public class ElevatorController : MonoBehaviour
    {
        [TabGroup("References")] public Transform robotTargetTransform;

        [SerializeField] private float elevatorMoveSpeed = 1f;
        [SerializeField] private float elevatorSnapDistance = 0.05f;

        public void StartMoveToPosition(Transform robotTransform, Transform targetTransform, Action callback)
        {
            // Debug.Log("moving elevator to next roomcontroller");

            StopAllCoroutines();
            StartCoroutine(MoveToPosition(robotTransform, targetTransform, callback));
        }
        public void SetPosition(Vector3 targetPos)
        {
            StopAllCoroutines();
            transform.position = targetPos;
        }

        private IEnumerator MoveToPosition(Transform robotTransform, Transform targetTransform, Action callback)
        {
            bool continueMoving = true;
            robotTransform.parent = transform;
            while (continueMoving)
            {
                float distance = (targetTransform.position - transform.position).magnitude;
                float multiplier = 1 / distance;
                transform.position = Vector3.Lerp(transform.position, targetTransform.position, elevatorMoveSpeed * Time.deltaTime * multiplier);

                if ((transform.position - targetTransform.position).sqrMagnitude < elevatorSnapDistance * elevatorSnapDistance)
                {
                    transform.position = targetTransform.position;
                    robotTransform.parent = null;

                    continueMoving = false;
                    callback?.Invoke();
                }
                yield return new WaitForFixedUpdate();
            }
        }
    }
}
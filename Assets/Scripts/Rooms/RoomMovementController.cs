using System;
using System.Collections;
using System.Collections.Generic;
using Contaquest.Metaverse.Robot;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.AI;

namespace Contaquest.Metaverse.Rooms
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class RoomMovementController : GenericSingleton<RoomMovementController>
    {
        [TabGroup("Properties")] [SerializeField] private string isWalkingName;
        [TabGroup("Properties")] [SerializeField] private float lerpSpeed = 0.2f;

        [TabGroup("References")] [SerializeField, ReadOnly] private NavMeshAgent agent;
        [TabGroup("References")] [SerializeField, ReadOnly] private Animator animator;
        [TabGroup("References")] [SerializeField] private ElevatorController elevatorController;
        [TabGroup("References")] [SerializeField] private BoolVariable isOnElevator;
        [TabGroup("References")] [SerializeField] private BoolVariable isAtActiveInteraction;
        [TabGroup("State")] [ShowInInspector, ReadOnly] private bool isWalking = false;

        private void OnValidate()
        {
            if (agent == null)
                agent = GetComponent<NavMeshAgent>();
            if (animator == null)
                animator = GetComponentInChildren<Animator>();
        }

        public override void Awake()
        {
            base.Awake();
            agent.updatePosition = false;
            agent.updateRotation = false;
        }

        public void StartMoveToRoomController(RoomController roomController, Action callback = null)
        {
            if (!isOnElevator.Value)
            {
                // Debug.Log("Is not on elevator, moving towards elevator, then to next roomcontroller");
                StartMoveToElevator(() => MoveToRoomController(roomController, callback));
            }
            else
            {
                // Debug.Log("Is on elevator, moving to next roomcontroller");
                MoveToRoomController(roomController, callback);
            }
        }

        private void MoveToRoomController(RoomController roomController, Action callback = null)
        {
            agent.enabled = false;
            callback += () => agent.enabled = true;
            elevatorController?.StartMoveToPosition(transform, roomController.elevatorTargetPoint, callback);
        }
        public void SetPosition(Vector3 position)
        {
            Debug.Log("Set Position to " + position);

            StopAllCoroutines();
            agent.updatePosition = false;
            agent.updateRotation = false;
            agent.enabled = false;
            animator.SetBool(isWalkingName, false);

            transform.position = position;
        }
        public void SetRotation(Quaternion rotation)
        {
            transform.rotation = rotation;
        }
        public void SetElevatorPosition(Vector3 position)
        {
            elevatorController.SetPosition(position);
        }
        public void SetIsOnElevator(bool value)
        {
            isOnElevator.Value = value;
        }

        public void StartMoveToElevator(Action callback = null)
        {
            // Debug.Log("Moving towards Elevator");
            callback += () => isOnElevator.Value = true;
            if(isOnElevator.Value)
            {
                // Debug.Log("Player was already on elevator, no need to go to the elevator. calling callback now");
                callback?.Invoke();
                return;
            }

            StopAllCoroutines();
            StartCoroutine(MoveRobotToTransform(elevatorController.robotTargetTransform, callback));
        }

        public void StartMoveToTransform(Transform transform, Action callback = null)
        {
            // Debug.Log("Moving towards Interaction");

            isOnElevator.Value = false;

            StopAllCoroutines();
            StartCoroutine(MoveRobotToTransform(transform, callback));
            isOnElevator.Value = false;
        }

        public void RotateRobot(Vector3 amount)
        {
            transform.Rotate(amount);
        }

        private IEnumerator MoveRobotToTransform(Transform targetTransform, Action callback = null)
        {
            isWalking = true;
            Debug.Log("Moving towards " + targetTransform.name, targetTransform.gameObject);
            animator.SetBool(isWalkingName, true);

            //if(!agent.isOnNavMesh)
            //{
            //    NavMesh navMesh = agent.n. //SamplePosition(Vector3 sourcePosition, out AI.NavMeshHit hit, float maxDistance, int areaMask);
            //}
            agent.enabled = true;
            agent.destination = targetTransform.position;
            agent.updatePosition = true;
            agent.updateRotation = true;


            float timeElapsed = 0f;
            //Debug.Log("Starting to walk to destination.");

            //walk into place
            while (true)
            {
                yield return null;
                float distance = agent.remainingDistance;

                timeElapsed += Time.deltaTime;

                if(agent.remainingDistance < 0.1f)
                {
                    Debug.Log($"Arrived at destination. walking took {timeElapsed} seconds");
                    agent.updatePosition = false;
                    agent.updateRotation = false;
                    agent.enabled = false;
                    animator.SetBool(isWalkingName, false);
                    break;
                }
            }

            timeElapsed = 0f;
            //rotate into place
            while (true)
            {
                transform.position = Vector3.Lerp(transform.position, targetTransform.position, lerpSpeed);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetTransform.rotation, lerpSpeed);
                // Debug.Log("Rotating");
                if (Quaternion.Angle(transform.rotation, targetTransform.rotation) < 2 || timeElapsed > 0.4f)
                {
                    Debug.Log($"Finished Rotating " + timeElapsed);
                    transform.position = targetTransform.position;
                    transform.rotation = targetTransform.rotation;
                    break;
                }
                yield return null;
                timeElapsed += Time.deltaTime;
            }
            isWalking = false;
            callback?.Invoke();
        }
    }
}
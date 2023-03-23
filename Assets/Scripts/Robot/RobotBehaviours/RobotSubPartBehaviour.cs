using System.Collections;
using System.Collections.Generic;
using Contaquest.Metaverse.Robot;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

namespace Contaquest.Metaverse.Behaviours
{
    public class RobotSubPartBehaviour : MonoBehaviour
    {
        [TabGroup("Properties")] public string parentAttachmentPointName;
        [TabGroup("Properties")] [SerializeField] private string copyAnimationAttachmentPointName;

        [Tooltip("The strength with which the subpart is propelled towards the gravitation Target")]
        [TabGroup("Properties")] [SerializeField] private float forceStrength;
        [TabGroup("Properties")] [SerializeField] private float gravitateMinDistance = 0.3f;
        [TabGroup("Properties")] [SerializeField] private float gravitateTimeOut = 1f;
        [TabGroup("Properties")] [SerializeField] private float explosionForce;

        [TabGroup("References")] [SerializeField] private Rigidbody rBody;

        private AttachmentPoint parentAttachmentPoint;
        private AttachmentPoint copyBoneAttachmentPoint;

        [TabGroup("State")] [SerializeField, ReadOnly] private bool isAnimated = true;
        [TabGroup("State")] [SerializeField, ReadOnly] private bool IsGravitatingTowardsTransform = false;
        [TabGroup("State")] [SerializeField, ReadOnly] private RobotDefinition robotDefinition;


        [SerializeField][TabGroup("Events")]
        private UnityEvent onInitialized, onAssembled, onDisassembled, onAttachpointReached;


        public void Initialize(RobotDefinition robotDefinition)
        {
            this.robotDefinition = robotDefinition;

            if (robotDefinition.attachmentPoints.TryGetValue(copyAnimationAttachmentPointName.GetHashCode(), out AttachmentPoint attachmentPoint))
            {
                copyBoneAttachmentPoint = attachmentPoint;
            }
            else
            {
                Debug.LogError($"Attachment point for animation not found: '{copyAnimationAttachmentPointName}'", gameObject);
            }
            
            onInitialized.Invoke();
        }

        public void InitializeSide(string sideSuffix)
        {
            if (!string.IsNullOrEmpty(sideSuffix))
            {
                parentAttachmentPointName += sideSuffix;
                copyAnimationAttachmentPointName += sideSuffix;
            }
        }

        public void Assemble()
        {
            StopAllCoroutines();
            SwitchToAnimated(true);

            if (robotDefinition.attachmentPoints.TryGetValue(parentAttachmentPointName.GetHashCode(), out AttachmentPoint pAttachmentPoint))
            {
                parentAttachmentPoint = pAttachmentPoint;
            }
            else
            {
                Debug.LogError($"{gameObject.name} could not find Attachment point : '{parentAttachmentPointName}'");
                return;
            }
            transform.parent = parentAttachmentPoint.transform;
            UpdateAnimationTransform();
            transform.localScale = Vector3.one;

            onAssembled.Invoke();
        }

        public void Disassemble()
        {
            transform.parent = null;
            SwitchToAnimated(false);
            rBody.useGravity = true;

            Vector3 randomDirection = new Vector3(Random.Range(-1, 1), 0, Random.Range(-1, 1)).normalized;
            rBody.AddForce(randomDirection * explosionForce);
            StartCoroutine(ScaleDownAndDestroy());
        }

        public void OnUpdate()
        {

        }

        public void OnLateUpdate()
        {
            if(isAnimated)
                UpdateAnimationTransform();
        }

        private void UpdateAnimationTransform()
        {
            if (copyBoneAttachmentPoint != null)
            {
                Vector3 deltaPos = copyBoneAttachmentPoint.GetDeltaPosition();
                deltaPos = transform.parent.InverseTransformVector(deltaPos);

                transform.localPosition = deltaPos;
                transform.rotation = copyBoneAttachmentPoint.GetDeltaRotation(); 
            }
            else
                Debug.LogError("No Bone assigned " + copyAnimationAttachmentPointName);
        }

        public void StartGravitateTowardsTransform(Transform targetTransform, bool stopWhenTargetReached, System.Action callBack)
        {
            if (!gameObject.activeInHierarchy)
                return;
            StopAllCoroutines();

            IsGravitatingTowardsTransform = true;
            StartCoroutine(GravitateTowardsTransform(targetTransform, stopWhenTargetReached, callBack));
        }

        public void StartGravitateTowardsAttachpoints(System.Action callBack = null)
        {
            StopAllCoroutines();
            System.Action newCallback = OnPartReachedAttachmentPoint;
            newCallback += callBack;

            if (robotDefinition.attachmentPoints.TryGetValue(parentAttachmentPointName.GetHashCode(), out AttachmentPoint pAttachmentPoint))
                parentAttachmentPoint = pAttachmentPoint;
            else
                Debug.LogError($"Attachment point not found: '{parentAttachmentPointName}'");

            StartGravitateTowardsTransform(parentAttachmentPoint.transform, true, newCallback);
        }

        public void OnPartReachedAttachmentPoint()
        {
            robotDefinition.ReAssemble();
            SwitchToAnimated(true);
            onAttachpointReached?.Invoke();
        }

        public void StopGravitateTowardsTransform()
        {
            IsGravitatingTowardsTransform = false;
        }

        private IEnumerator ScaleDownAndDestroy()
        {
            yield return null;
            float duration = 0.5f;
            Vector3 scaleToDestroy = new Vector3(0.05f, 0.05f, 0.05f), initScale = transform.localScale;
            for(float time = 0; time < duration; time += Time.deltaTime)
            {
                float progress = time / duration;
                transform.localScale = Vector3.Lerp(initScale, scaleToDestroy, progress);
                yield return null;
            }
            Destroy(gameObject);
        }

        private IEnumerator GravitateTowardsTransform(Transform targetTransform, bool stopWhenTargetReached, System.Action callBack)
        {
            if (rBody == null)
                StopAllCoroutines();

            SwitchToAnimated(false);
            rBody.useGravity = false;

            float time = 0f;

            while (IsGravitatingTowardsTransform)
            {
                Vector3 direction = targetTransform.position - transform.position;
                float directionMagnitude = direction.magnitude;
                if (directionMagnitude < 1)
                    direction += (1 - directionMagnitude) * direction * 3;

                direction *= forceStrength;
                rBody.AddForce(direction);

                yield return null;

                if (stopWhenTargetReached)
                {
                    IsGravitatingTowardsTransform = (targetTransform.position - transform.position).sqrMagnitude > (gravitateMinDistance * gravitateMinDistance);

                    time += Time.deltaTime;
                    if (time > gravitateTimeOut)
                    {
                        IsGravitatingTowardsTransform = false;
                        transform.position = targetTransform.position;
                    }
                }
            }

            if (callBack != null)
                callBack.Invoke();

            SwitchToAnimated(true);
        }

        private void SwitchToAnimated(bool isAnimated)
        {
            rBody.isKinematic = isAnimated;
            this.isAnimated = isAnimated;
        }
    }
}
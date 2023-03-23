using System.Collections;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

namespace Contaquest.Metaverse.Combat2
{
    [RequireComponent(typeof(Rigidbody))]
    public class RigidBodyProjectile : MonoBehaviour
    {
        [SerializeField, ReadOnly] private Rigidbody rBody;
        [SerializeField] private float acceleration;
        [SerializeField] private UnityEvent onHit;
        [SerializeField] private float invincibleTime = 0.1f;
        private Transform targetTransform;
        private float timeElapsed = 0f;

#if UNITY_EDITOR
        protected void OnValidate()
        {
            rBody = GetComponent<Rigidbody>();
        }
#endif
        private void FixedUpdate()
        {
            timeElapsed += Time.fixedDeltaTime;
            if(targetTransform != null)
                transform.LookAt(targetTransform);
            rBody.AddForce(transform.forward * acceleration);
        }

        public void SetTarget(Transform transform)
        {
            targetTransform = transform;
        }

        private void OnTriggerEnter(Collider collider)
        {
            if(timeElapsed > invincibleTime)
            {
                onHit?.Invoke();
                Destroy(gameObject);
            }
        }
    }
}

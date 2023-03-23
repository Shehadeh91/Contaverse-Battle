using Contaquest.Metaverse.Robot;
using UnityEngine;

namespace Contaquest.Metaverse.Combat2
{
    public class CombatParticleEffect : MonoBehaviour
    {
        [SerializeField] private bool destroyOnParticlePlayed;
        [SerializeField] private ParticleSystem myParticleSystem;
        [SerializeField] private TransformUnityEvent onInitialized = new TransformUnityEvent();

        private Transform targetPoint;

        private void Start()
        {
            if(destroyOnParticlePlayed)
                Destroy(gameObject, myParticleSystem.main.duration + 0.5f);
        }

        public virtual void Initialize(CombatActionContext context, string targetAttachmentpoint)
        {
            context.Target.robotDefinition.attachmentPoints.TryGetValue(targetAttachmentpoint.GetHashCode(), out AttachmentPoint attachmentPoint);
            if(targetPoint == null)
                return;
            targetPoint = attachmentPoint.transform;
            transform.LookAt(targetPoint);
            onInitialized?.Invoke(targetPoint);
        }

        public void Update()
        {

        }
    }
}

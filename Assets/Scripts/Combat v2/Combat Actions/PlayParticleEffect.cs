using Contaquest.Metaverse.Robot;
using UnityEngine;
using Sirenix.OdinInspector;

namespace Contaquest.Metaverse.Combat2.Effects
{
    [CreateAssetMenu(fileName = "Play Particle", menuName = "Combat2/Effects/Play Particle")]
    public class PlayParticleEffect : TimedEffect
    {
        [Tooltip("This will try and use the ParticleSystem specified in the RobotPart Prefab this attack is being set from." +
        	"\n The <particleSystem variable will only be used if no override was found>")]
        [SerializeField] private bool allowRobotPartOverride = true;
        [Tooltip("The particle system used when no override was found or is not allowed.")]
        [SerializeField] private CombatParticleEffect particleSystem;
        [Tooltip("This is the position the Prefab will be created at")]
        [SerializeField] private string attachmentPointName;
        [Tooltip("This is the position the Prefab will travel towards if it is a projectile")]
        [SerializeField] private string targetAttachmentPointName;

        protected override void EffectTrigger(CombatActionContext context)
        {
            string attachmentPointString = attachmentPointName;

            if (context.sourceEquipSlot == EquipSlot.LeftArm)
                attachmentPointString += "L";
            else if (context.sourceEquipSlot == EquipSlot.RightArm)
                attachmentPointString += "R";

            context.Source.robotDefinition.attachmentPoints.TryGetValue(attachmentPointString.GetHashCode(), out AttachmentPoint attachmentPoint);
            if(attachmentPoint == null)
            {
                Debug.LogError($"{name}: The Attachmentpoint {attachmentPointString} could not be found in controller {context.Source.name}", this);
                return;
            }

            CombatParticleEffect pSystem = particleSystem;

            if (allowRobotPartOverride)
            {
                CombatParticleEffect combatActionParticleSystem = context.SourceRobotPart.combatActionParticleSystem;
                if (combatActionParticleSystem != null)
                    pSystem = combatActionParticleSystem;
            }

            CombatParticleEffect combatParticle = Instantiate(pSystem, attachmentPoint.transform.position, Quaternion.identity);

            if(combatParticle != null)
            {
                combatParticle.Initialize(context, targetAttachmentPointName);
            }
        }
    }
}

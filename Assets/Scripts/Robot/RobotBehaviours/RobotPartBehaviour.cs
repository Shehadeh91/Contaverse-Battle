using System.Collections.Generic;
using System.Linq;
using Contaquest.Metaverse.Combat2;
using Contaquest.Metaverse.Data;
using Contaquest.Metaverse.Robot;
using Contaquest.Metaverse.Robot.Customization;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

namespace Contaquest.Metaverse.Behaviours
{
    public class RobotPartBehaviour : MonoBehaviour
    {
        [TabGroup("Properties")] public RobotStats robotStats;
        [TabGroup("Properties")] [ReadOnly] [SerializeField] protected string robotName;
        [TabGroup("Properties")] [SerializeField] [InlineEditor] private CombatAction combatAction;
        [Tooltip("The Amount of Units this part will push the center of the body upwards")]
        [TabGroup("Properties")] public float floorDistance;

        [TabGroup("References")] [SerializeField] protected RobotSubPartBehaviour[] subParts;
        [TabGroup("References")] [SerializeField] protected AttachmentPoint[] attachmentPoints;
        [TabGroup("References")] [SerializeField] protected MaterialChanger[] materialChangers;
        [TabGroup("References")] [SerializeField] protected RigidBodyController rigidBodyController;
        [TabGroup("References")] [SerializeField] protected RobotPartDraggable robotPartDraggable;
        [TabGroup("References")] public CombatParticleEffect combatActionParticleSystem;

        [TabGroup("Events")] [SerializeField] protected RobotDefinitionUnityEvent onInitialized;
        [TabGroup("Events")] [SerializeField] protected UnityEvent onAssembled, onDisassembled, onActionPerformed;

        [TabGroup("State")] [SerializeField, ReadOnly] private bool isActivated;
        [TabGroup("State")] public RobotDefinition robotDefinition;
        [TabGroup("State")] public EquippableData equippableData = new EquippableData();
        [TabGroup("State")] public iOwner owner;

        public virtual void InitializeRobotData(RobotPart robotPartData)
        {
            equippableData.robotPartData = robotPartData;
            equippableData.nftID = robotPartData.nftData.nftID;

            robotStats = robotPartData.stats;
            robotStats.Initialize();
            foreach (var materialChanger in materialChangers)
            {
                materialChanger?.Initialize(robotPartData);
            }
            robotName = gameObject.name.Substring(0, gameObject.name.IndexOf('_'));
        }

        public virtual void InitializeRobotDefinition(RobotDefinition robotDefinition)
        {
            this.robotDefinition = robotDefinition;

            foreach (var subPart in subParts)
            {
                subPart.Initialize(robotDefinition);
            }

            robotDefinition.AddAttachmentPoints(attachmentPoints);

            onInitialized?.Invoke(robotDefinition);
        }

        public virtual void Remove()
        {
            robotDefinition.RemoveAttachmentPoints(attachmentPoints);
        }

        public virtual void Assemble()
        {
            foreach (var subPart in subParts)
            {
                subPart.Assemble();
            }
            onAssembled?.Invoke();
        }
        public void ReAssemble()
        {
            foreach (var subPart in subParts)
            {
                subPart.Assemble();
            }
        }
        public void Disassemble()
        {
            foreach (var subPart in subParts)
            {
                subPart.Disassemble();
            }
            owner = null;
            onDisassembled?.Invoke();
        }
        #region Activate and deactivate part

        public void ActivateRobotPart()
        {
            if (isActivated)
                return;
            robotPartDraggable.EnableInteractability();
            isActivated = true;
        }
        public void DeactivateRobotPart()
        {
            if (!isActivated)
                return;
            robotPartDraggable.DisableInteractability();
            isActivated = false;
        }
        #endregion

        public virtual void OnUpdate()
        {
            foreach (var subPart in subParts)
            {
                subPart.OnUpdate();
            }
        }
        public virtual void OnLateUpdate()
        {
            foreach (var subPart in subParts)
            {
                subPart.OnLateUpdate();
            }
        }
        public virtual void OnCombatActionPerformed()
        {
            onActionPerformed.Invoke();
        }

        public void StartGravitateTowardsTransform(Transform targetTransform, bool stopWhenTargetReached, System.Action callBack)
        {
            foreach (var subPart in subParts)
            {
                subPart.StartGravitateTowardsTransform(targetTransform, stopWhenTargetReached, callBack);
            }
        }
        public void StopGravitateTowardsTransform()
        {
            foreach (var subPart in subParts)
            {
                subPart.StopGravitateTowardsTransform();
            }
        }
        public void StartGravitateTowardsAttachpoints(System.Action callBack)
        {
            foreach (var subPart in subParts)
            {
                subPart.StartGravitateTowardsAttachpoints(callBack);
            }
        }

        public virtual CombatAction GetCombatAction()
        {
            return combatAction;
        }

        public RobotSubPartBehaviour GetRobotSubPartBehaviour(string parentAttachmentPointName)
        {
            return subParts.First((subPartBehaviour) => subPartBehaviour.parentAttachmentPointName == parentAttachmentPointName);
        }

        public void SetDirtiness(float value)
        {
            foreach (var materialChanger in materialChangers)
            {
                materialChanger?.SetDirtiness(value);
            }
        }
    }
}
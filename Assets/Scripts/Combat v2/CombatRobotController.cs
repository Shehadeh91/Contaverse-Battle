using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Sirenix.OdinInspector;
using Contaquest.Metaverse.Robot;
using System.Linq;

namespace Contaquest.Metaverse.Combat2
{
    public class CombatRobotController : MonoBehaviour
    {
        [HideInInspector] public RobotDefinition robotDefinition;
        
        [TabGroup("Properties")] public FloatRangeReference health;
        [TabGroup("Properties")] public FloatRangeReference dirtiness;
        [TabGroup("Properties")] public IntVariable laneIndex;
        [TabGroup("Properties")] public FloatVariable lanePosition;
        [TabGroup("Properties")] public IntReference energy;
        
        [TabGroup("References")] public RobotController robotController;
        [TabGroup("References")] public CombatAnimator combatAnimator;
        [TabGroup("References")] [SerializeField] private CombatUser combatUser;
        [TabGroup("References")] [System.NonSerialized, ShowInInspector, ReadOnly] public CombatLobby combatLobby;
        
        [TabGroup("Events")] [SerializeField] private RobotDefinitionUnityEvent onInitialized = new RobotDefinitionUnityEvent();
        [TabGroup("Events")] [SerializeField] private UnityEvent onReset, onDeath, onRevived, onNotEnoughEnergy = new UnityEvent();
        [TabGroup("Events")] public UnityEvent onDodged, onDamageBlocked, onDamaged, onFallingDown = new UnityEvent();

        [TabGroup("State")] [System.NonSerialized, ShowInInspector, ReadOnly] public bool isAlive, isBlocking, hasDodged = true;
        [TabGroup("State")] [System.NonSerialized, ShowInInspector, ReadOnly] public float receivedDamageReduction, receivedDamageMultiplier, dealtDamageAddition, dealtDamageMultiplier;
        [TabGroup("State")] [System.NonSerialized, ShowInInspector, ReadOnly] public List<CombatAction> combatActions;
        [TabGroup("State")] [System.NonSerialized, ShowInInspector, ReadOnly] public int turnPriorityBoost;
        [TabGroup("State")] [ShowInInspector, ReadOnly] private Vector3 frameDeltaPosition;
        [TabGroup("State")] [ShowInInspector, ReadOnly] private Quaternion frameDeltaRotation;

        #region Initializing
        void OnEnable()
        {
            laneIndex.OnChanged += SetPositionInArena;
            lanePosition.OnChanged += SetPositionInArena;
        }
        void OnDisable()
        {
            laneIndex.OnChanged -= SetPositionInArena;
            lanePosition.OnChanged -= SetPositionInArena;
        }
        public void InitializeRobot(CombatLobby combatLobby, ArenaPosition arenaPosition)
        {
            this.combatLobby = combatLobby;

            robotDefinition = robotController.CreateAndGetRobot();
            InitializeValues(robotDefinition);

            InitializeStartPosition(arenaPosition);
            onInitialized.Invoke(robotDefinition);
        }
        private void InitializeValues(RobotDefinition robotDefinition)
        {
            health.Value.MaxValue = robotDefinition.robotStatsCollection.GetStatValue("HP");
            health.Value.Value = health.Value.MaxValue;
            List<CombatActionContext> combatActionContexts = robotDefinition.GetAllCombatActions(this, GetOpponent());
            combatActions = combatActionContexts.Select((c) => c.CombatAction).ToList();
        }

        public void InitializeStartPosition(ArenaPosition arenaPosition)
        {
            lanePosition.Value = arenaPosition.lanePosition;
            laneIndex.Value = arenaPosition.laneIndex;
            Vector3 forward = combatLobby.arenaDefinition.GetForwardInArena(laneIndex.Value, lanePosition.Value);

            transform.position = combatLobby.arenaDefinition.GetPositionInArena(laneIndex.Value, lanePosition.Value);
            transform.LookAt(forward * arenaPosition.LookDirection);
        }
        #endregion


        private void LateUpdate()
        {
            transform.position += frameDeltaPosition;
            transform.rotation = transform.rotation * frameDeltaRotation;

            frameDeltaPosition = Vector3.zero;
            frameDeltaRotation = Quaternion.identity;
        }
        public void Move(Vector3 frameDelta)
        {
            frameDeltaPosition += frameDelta;
        }
        public void Rotate(Quaternion frameDelta)
        {
            frameDeltaRotation *= frameDelta;
        }
        public void ApplyDamage(float value)
        {
            health.Value.Value -= value;
            
            if (health.Value.Value <= 0)
            {
                health.Value.Value = 0;
                // Death stuff etc
            }
        }
        public void ApplyKnockBack(float value)
        {
            // TODO: set lane Position and apply weight calculation
        }
        public void SetPositionInArena()
        {
            frameDeltaPosition = Vector3.zero;
            transform.position = combatLobby.arenaDefinition.GetPositionInArena(laneIndex.Value, lanePosition.Value);
        }
        public void SetPositionInArena(ArenaPosition arenaPosition)
        {
            frameDeltaPosition = Vector3.zero;
            transform.position = combatLobby.arenaDefinition.GetPositionInArena(arenaPosition);
        }

        public CombatAction GetCombatActionByIndex(int index)
        {
            return combatActions[index];
        }
        public CombatAction[] GetCombatActionsByType(ActionType actionType)
        {
            return combatActions.Where((combatAction) => combatAction.actionType == actionType).ToArray();
        }

        public ArenaPosition GetArenaPosition()
        {
            return new ArenaPosition(laneIndex.Value, lanePosition.Value);
        }
        public int GetEnergy()
        {
            return energy.Value;
        }

        public int GetOpponentUserID()
        {
            // TODO: Proper implementation
            return 0;
        }
        public CombatRobotController GetOpponent()
        {
            return combatLobby.combatUsers.FirstOrDefault((combatUser) => combatUser != this.combatUser).combatRobotController;
        }
        public void RegisterUserIpnut(CombatAction combatAction)
        {
            // TODO: verify and Send to network 
        }
        public void RegisterUserIpnut(CombatActionContext combatActionContext)
        {
            // if(combatActionContext.CombatAction.VerifyAction(combatActionContext))
            // {
                
            // }
            // TODO: verify and Send to network 
        }
        public void ResetTurnValues()
        {
            receivedDamageReduction = 0f;
            receivedDamageMultiplier = 1f;
        }
    }
}
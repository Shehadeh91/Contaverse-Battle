using System;
using Contaquest.Metaverse.Data;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Contaquest.Metaverse.Robot
{
    public class RobotController : MonoBehaviour, iOwner
    {
        public RobotDefinition robotDefinition;

        [TabGroup("Properties")] [SerializeField] private bool loadRobotOnStart = true;
        //[TabGroup("Properties")] [SerializeField] private string animatorStartState = "Idle Defense";
        [TabGroup("References")] [SerializeField] private AttachmentPoint[] attachmentPoints;
        [TabGroup("References")] public Animator animator;
        [TabGroup("References")] [SerializeField] private RobotDataSource robotDataSource;

        [TabGroup("Events")] public RobotDefinitionUnityEvent onInitialized = new RobotDefinitionUnityEvent();
        private bool isInitialized = false;

        private void Start()
        {
            if(loadRobotOnStart)
                robotDataSource.LoadRobotData();
        }
        private void Initialize(RobotData robotData)
        {
            robotDefinition = new RobotDefinition(robotData);

            robotDefinition.AddAttachmentPoints(attachmentPoints);
            onInitialized.Invoke(robotDefinition);
            isInitialized = true;
        }

        public RobotDefinition CreateAndGetRobot()
        {
            CreateRobot();
            return robotDefinition;
        }
        [Button][HideInEditorMode]
        public void CreateRobot()
        {
            Debug.Log("Creating Robot");
            robotDataSource.LoadRobotData();
            RobotData robotData = robotDataSource.GetRobotData();
            Initialize(robotData);
        }
        protected void Update()
        {
            if(isInitialized)
                robotDefinition?.OnUpdate();
        }

        protected void LateUpdate()
        {
            if (isInitialized)
                robotDefinition?.OnLateUpdate();
        }

        public void OnPartEquipped()
        {

        }
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using Contaquest.Metaverse.Behaviours;
using Contaquest.Metaverse.Data;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Events;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Contaquest.Metaverse.Robot
{
    public class RobotAssembler : MonoBehaviour
    {
        RobotDefinition robotDefinition;
        private int unloadedPrefabCount = 0;
        private  AsyncOperationHandle<GameObject>[] asyncOperationHandles;

        [SerializeField]
        private RobotDefinitionUnityEvent onAssembled;

        public void Initialize(RobotDefinition robotDefinition)
        {
            this.robotDefinition = robotDefinition;
            TryCreateParts();
        }

        #region Waiting for Part Download

        private void TryCreateParts()
        {
            var equippableDatas = robotDefinition.robotData.equippableDatas;
            unloadedPrefabCount = equippableDatas.Length;
            foreach (var equippableData in equippableDatas)
            {
                if(equippableData == null || equippableData.robotPartData == null)
                {
                    Debug.LogWarning("Cannot equip this Equippable" + equippableData);
                    return;
                }
                ItemManager.Instance.InstantiateItem(equippableData.robotPartData, (go) => OnPrefabInstantiated(equippableData, go));
            }
        }
        private void OnPrefabInstantiated(EquippableData equippableData, GameObject go)
        {
            RobotPartBehaviour robotPartBehaviour = go.GetComponent<RobotPartBehaviour>();
            robotPartBehaviour.equippableData = equippableData;
            robotPartBehaviour.InitializeRobotData(equippableData.robotPartData);
            robotPartBehaviour.InitializeRobotDefinition(robotDefinition);

            robotDefinition.EquipPart(robotPartBehaviour);

            unloadedPrefabCount--;
            if (unloadedPrefabCount <= 0)
                AssembleRobot();

        }
        #endregion

        public void AssembleRobot()
        {
            foreach (var robotPartBehaviour in robotDefinition.robotPartBehaviours)
            {
                robotPartBehaviour.Value.Assemble();
            }
            onAssembled.Invoke(robotDefinition);
        }
    }
}
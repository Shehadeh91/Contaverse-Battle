using System.Collections.Generic;
using Contaquest.Metaverse.Behaviours;
using Contaquest.Metaverse.Data;
using Contaquest.Metaverse.Rooms;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

namespace Contaquest.Metaverse.Robot.Customization
{
    public class CustomizationSelector : MonoBehaviour, iOwner
    {
        [TabGroup("References")] [SerializeField] private GameObject buyMorePartsPrefab;
        [TabGroup("References")] [SerializeField] private CustomizationFolderSO[] customizationFolders;
        [TabGroup("References")] [SerializeField] private CustomizationConveyor conveyor;
        [TabGroup("References")] [SerializeField] private FloatVariable selectedPartIndex;

        [TabGroup("Events")] [SerializeField] private UnityEvent onSelectedPartChanged = new UnityEvent();
        [TabGroup("Events")] [SerializeField] private UnityEvent onNoParts = new UnityEvent();

        [TabGroup("State")] [ReadOnly] public RobotPartBehaviour selectedPart;
        [TabGroup("State")] [ReadOnly] public CustomizationFolderSO selectedFolder;

        [TabGroup("State")] [ReadOnly] public RobotPartBehaviour[] robotPartBehaviours;
        [TabGroup("State")]  [ShowInInspector] private List<Transform> transforms = new List<Transform>();
        [TabGroup("State")] [SerializeField] private GameObject buyMorePartsObject;

        private void OnEnable()
        {
            selectedPartIndex.OnChanged += OnSelectedPartChanged;
            for (int i = 0; i < customizationFolders.Length; i++)
            {
                customizationFolders[i].Initialize(this);
            }
        }
        private void OnDisable()
        {
            selectedPartIndex.OnChanged -= OnSelectedPartChanged;
            //for (int i = 0; i < customizationFolders.Length; i++)
            //{
            //    customizationFolders[i].onSelected.RemoveListener(() => OnFolderSelected(i));
            //    customizationFolders[i].onDeselected.RemoveListener(() => OnFolderDeselected(i));
            //}
        }

        public void OnSelectedPartChanged()
        {
            if (robotPartBehaviours.Length > 0)
            {
                if (selectedPartIndex.ClampValue(0, robotPartBehaviours.Length))
                    return;
            }
            else
            {
                selectedPartIndex.Value = 0f;
                return;
            }

            int index = (int) selectedPartIndex.Value;
            if(index >= robotPartBehaviours.Length || index < 0)
                selectedPart = null;
            else
                selectedPart = robotPartBehaviours[index];
            onSelectedPartChanged?.Invoke();
        }

        public void OnFolderSelected(CustomizationFolderSO newFolder)
        {
            if (selectedFolder == newFolder)
            {
                // Debug.Log($"The folder {selectedFolder.name} is already selected, returning.");
                return;
            }
            
            // Debug.Log($"Selected the folder {newFolder.name}.");

            selectedFolder = newFolder;

            RobotPart[] robotParts = selectedFolder.GetRobotPartDatas();

            //Match number of transforms to number of robotparts
            while (robotParts.Length + 1 != transforms.Count)
            {
                if (robotParts.Length + 1 > transforms.Count)
                {
                    transforms.Add(new GameObject().transform);
                }
                else
                {
                    //Removes the last transform in the list
                    Transform oldTransform = transforms[transforms.Count - 1];
                    transforms.RemoveAt(transforms.Count - 1);
                    Destroy(oldTransform.gameObject);
                }
            }

            if(buyMorePartsObject == null)
                buyMorePartsObject = Instantiate(buyMorePartsPrefab, new Vector3(1000f, 1000f, 1000f), Quaternion.identity);
            AddTransformToConveyor(buyMorePartsObject.transform, transforms.Count - 1);

            if (robotParts == null || robotParts.Length == 0)
            {
                // Debug.Log("There are no RobotParts to Display for this Folder");
                onNoParts?.Invoke();
            }
            else
            {
                // Debug.Log("creating new robot parts");

                robotPartBehaviours = new RobotPartBehaviour[robotParts.Length];
                for (int i = 0; i < robotParts.Length; i++)
                {
                    RobotPart robotPart = robotParts[i];
                    ItemManager.Instance.InstantiateItem(robotPart, (go) => OnPrefabInstantiated(robotPart, go, i));
                }
            }

            conveyor.UpdateConveyorItems(transforms);
            selectedPartIndex.SetValueLoud(0f);
        }
        private void OnPrefabInstantiated(RobotPart robotPart, GameObject go, int index)
        {
            RobotPartBehaviour robotPartBehaviour = go.GetComponent<RobotPartBehaviour>();
            go.GetComponent<RobotPartDraggable>().EnableInteractability();

            robotPartBehaviour.InitializeRobotData(robotPart);
            robotPartBehaviours[index] = robotPartBehaviour;
            robotPartBehaviour.owner = this;
            AddTransformToConveyor(robotPartBehaviour.transform, index);
        }

        public void AddTransformToConveyor(Transform newTransform, int index)
        {
            newTransform.parent = transforms[index];
            newTransform.localPosition = Vector3.zero;
            newTransform.localScale = Vector3.one;
            newTransform.localRotation = Quaternion.identity;
        }

        public void OnFolderDeselected(CustomizationFolderSO oldFolder)
        {
            selectedFolder = null;
            
            foreach (var robotPartBehaviour in robotPartBehaviours)
            {
                if((object)robotPartBehaviour.owner == this)
                {
                    robotPartBehaviour.Disassemble();
                    Destroy(robotPartBehaviour.gameObject);
                }
            }
        }

        public void SaveRobotCustomization()
        {
            RoomManager.Instance.robotController.robotDefinition.SaveRobotDataToSlot(1);
        }
    }
}

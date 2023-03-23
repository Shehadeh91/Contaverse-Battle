// using System;
// using System.Collections;
// using System.Collections.Generic;
// using Contaquest.Metaverse.Behaviours;
// using Contaquest.Metaverse.Data;
// using Contaquest.UI;
// using Sirenix.OdinInspector;
// using UnityEngine;
// using UnityEngine.Events;

// namespace Contaquest.Metaverse.Robot.Customization
// {
//     public class PartListController : MonoBehaviour
//     {
//         [TabGroup("Properties")] [SerializeField] private float interpolationSpeed = 0.1f;
//         [TabGroup("Properties")] [SerializeField] private FloatVariable slideProgress;

//         [TabGroup("References")] [SerializeField] private CustomizationFolderSO[] customizationFolders;
//         [TabGroup("References")] [SerializeField] private List<Transform> partListTransforms;
//         [TabGroup("References")] [SerializeField] private RobotPartBehaviour[] robotPartBehaviours;

//         [TabGroup("Events")] [SerializeField] private UnityEvent onSelectedPartChanged = new UnityEvent();
//         [TabGroup("Events")] [SerializeField] private UnityEvent onPartsEmpty = new UnityEvent();

//         [TabGroup("State")] [SerializeField, ReadOnly] private float currentProgress;
//         [TabGroup("State")] [ReadOnly] public RobotPartBehaviour selectedPart;

//         [TabGroup("State")] [ReadOnly] public CustomizationFolderSO currentFolder;
//         private int unloadedParts = 0;

//         public void OnSlideProgressChanged()
//         {
//             if (slideProgress.Value > robotPartBehaviours.Length)
//                 slideProgress.SetValueSilent(robotPartBehaviours.Length);
//             if(slideProgress.Value < 0)
//                 slideProgress.SetValueSilent(0);
//             StopAllCoroutines();
//             StartCoroutine(MoveToSlideProgress());
//         }

//         private void OnEnable()
//         {
//             slideProgress.OnChanged += OnSlideProgressChanged;

//             if (partListTransforms.Count != 5)
//             {
//                 Debug.LogError("This stupid script needs to have exactly 5 transforms to work", gameObject);
//                 DestroyImmediate(this);
//                 return;
//             }
//             foreach (var customizationFolder in customizationFolders)
//             {
//                 customizationFolder.onSelected.AddListener(() => OnFolderSelected(customizationFolder));
//             }
//         }
//         private void OnDisable()
//         {
//             slideProgress.OnChanged -= OnSlideProgressChanged;

//             foreach (var customizationFolder in customizationFolders)
//             {
//                 customizationFolder.onSelected.RemoveListener(() => OnFolderSelected(customizationFolder));
//             }
//         }

//         public void OnFolderSelected(CustomizationFolderSO customizationFolder)
//         {
//             if (currentFolder == customizationFolder)
//                 return;

//             for (int i = 0; i < robotPartBehaviours.Length; i++)
//             {
//                 Destroy(robotPartBehaviours[i].gameObject);
//             }

//             currentFolder = customizationFolder;
//             RobotPart[] robotPartDatas = customizationFolder.GetRobotPartDatas();
//             if (robotPartDatas is null || robotPartDatas.Length == 0)
//             {
//                 onPartsEmpty?.Invoke();
//                 Debug.LogError("robotPartDatas are null");
//                 return;
//             }

//             robotPartBehaviours = new RobotPartBehaviour[robotPartDatas.Length];

//             unloadedParts = robotPartDatas.Length;

//             for (int i = 0; i < robotPartDatas.Length; i++)
//             {
//                 RobotPart robotPartData = robotPartDatas[i];
//                 //Debug.Log(robotPartData.itemAddress);
//                 if (!robotPartData.isDownloaded)
//                 {
//                     Debug.LogWarning($"Prefab was not yet downloaded {robotPartData.itemAddress}");
//                     robotPartData.onAssetDownloaded.AddListener(() => OnPartLoaded(robotPartData));
//                     robotPartData.StartAssetDownload();
//                 }
//                 else
//                 {
//                     OnPartLoaded(robotPartDatas[i]);
//                 }
//             }
//         }

//         private void OnPartLoaded(RobotPart robotPartData)
//         {
//             unloadedParts--;
//             GameObject go = Instantiate(robotPartData.prefab);

//             RobotPartBehaviour robotPartBehaviour = go.GetComponent<RobotPartBehaviour>();

//             robotPartBehaviour.InitializeRobotData(robotPartData);
//             robotPartBehaviours[unloadedParts] = robotPartBehaviour;

//             if(unloadedParts == 0)
//             {
//                 OnAllPartsLoaded();
//             }
//         }

//         private void OnAllPartsLoaded()
//         {
//             currentProgress = 0f;
//             slideProgress.SetValueSilent(0f);
//             UpdatePartTransforms();
//         }

//         private void UpdatePartTransforms()
//         {
//             for (int i = 0; i < robotPartBehaviours.Length; i++)
//             {
//                 Transform currentTransform = robotPartBehaviours[i].transform;

//                 float partProgress = currentProgress - i;

//                 if(partProgress <= -2 || partProgress >= 2)
//                 {
//                     robotPartBehaviours[i].DeactivateRobotPart();
//                     currentTransform.gameObject.SetActive(false);
//                     continue;
//                 }

//                 if(partProgress <= 0.5f || partProgress >= 0.5f)
//                 {
//                     if(selectedPart != robotPartBehaviours[i])
//                     {
//                         selectedPart = robotPartBehaviours[i];
//                         onSelectedPartChanged?.Invoke();
//                     }
//                     //Debug.Log("Value of I: " + i + ", Value of PartProgress: " + partProgress + ", Value of current: " + currentProgress + ", GO: " + gameObject.name);
//                 }

//                 robotPartBehaviours[i].ActivateRobotPart();
//                 currentTransform.gameObject.SetActive(true);

//                 int floor = (int)Math.Floor(partProgress);
//                 int ceiling = floor + 1;
//                 float lerpProgress = partProgress - floor;

//                 Transform lastTransform = partListTransforms[floor + 2];
//                 Transform nextTransform = partListTransforms[ceiling + 2];

//                 //Debug.Log($"Lerping position: {lerpProgress} \n" +
//                 	      //$"floor: {floor} \n" +
//                 	      //$"parprogress: {partProgress}");
//                 currentTransform.position = Vector3.Lerp(lastTransform.position, nextTransform.position, lerpProgress);
//                 currentTransform.localScale = Vector3.Lerp(lastTransform.localScale, nextTransform.localScale, lerpProgress);
//             }
//         }

//         private IEnumerator MoveToSlideProgress()
//         {
//             bool isUpdating = true;
//             while(isUpdating)
//             {
//                 currentProgress = Mathf.Lerp(currentProgress, slideProgress.Value, interpolationSpeed);
//                 if(Mathf.Abs(currentProgress - slideProgress.Value) < 0.01f)
//                 {
//                     currentProgress = slideProgress.Value;
//                     isUpdating = false;
//                 }
//                 UpdatePartTransforms();
//                 yield return null;
//             }
//         }
//     }
// }

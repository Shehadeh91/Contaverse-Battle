using System.Collections.Generic;
using UnityEngine;
using Contaquest.Metaverse.Robot.Customization;

namespace Contaquest.UI.Customization
{
    public class CustomizePartColor : MonoBehaviour
    {        
//         public static CustomizePartColor instance;        
//         private string currentPart;
//         [SerializeField]
//         private List<MaterialChanger> partList;
//         [SerializeField]
//         private GameObject CustomColorUI;
//         private bool isUIVisible = false;

//         [HideInInspector]
//         public Color currentColor;

//         private void OnEnable()
//         {
//             if (instance == null)
//                 instance = this;
//             else
//                 Destroy(this);
//             if (CustomColorUI != null)
//                 CustomColorUI.SetActive(isUIVisible);
//         }

//         private void OnDisable()
//         {
//             instance = null;
//         }

//         public void SetSelectedPart(ListItemData listItemData)
//         {
//             currentPart = listItemData.title;
//         }

//         public void ChangeColor(Color color, ColorType type)
//         {
//             foreach (var p in partList.FindAll(x => x.identity.Contains(currentPart)))
//             {
//                 Debug.Log(p.identity);
//             }
//             // partList.FindAll(x => x.identity.Contains(currentPart)).ForEach(y => y.ChangeMaterialColor(color, type));            
//         }

//         public void AcceptChanges()
//         {
//             // partList.FindAll(x => x.identity.Contains(currentPart)).ForEach(y => y.AcceptMaterialChanges());
//             ToggleUI();
//         }

//         public void DiscardChanges()
//         {
//             // partList.FindAll(x => x.identity.Contains(currentPart)).ForEach(y => y.DiscardMaterialChanges());
//             //ToggleUI();
//         }

//         public void RegisterPart(MaterialChanger partName)
//         {
//             partList.Add(partName);
//         }

//         public void RemovePart(MaterialChanger partName)
//         {
//             partList.Remove(partName);
//         }

//         public void ToggleUI()
//         {
//             isUIVisible = !isUIVisible;
//             if (!isUIVisible)
//                 DiscardChanges();
//             CustomColorUI.SetActive(isUIVisible);
//         }
    }
}

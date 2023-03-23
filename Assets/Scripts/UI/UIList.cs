using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Contaquest.UI
{
    public class UIList : MonoBehaviour
    {
        [TabGroup("References")] [SerializeField] private bool initializeOnStart;
        [TabGroup("References")] [SerializeField] private List<ListItemData> listItems;
        [TabGroup("References")] [SerializeField] private Transform listParent;
        [TabGroup("References")] [SerializeField] private UIListItem UIListItemPrefab;

        [TabGroup("State")] [SerializeField] [InlineEditor] private ListItemData selectedListItem;

        private void Start()
        {
            if(initializeOnStart)
                Initialize();
            selectedListItem?.onSelected?.Invoke();
        }

        private void Initialize()
        {
            for (int i = 0; i < listItems.Count; i++)
            {
                UIListItem listItem = Instantiate(UIListItemPrefab, listParent);
                listItem.Initialize(this, listItems[i], i);
            }
        }

        public void OnItemSelected(int listIndex)
        {
            Debug.Log("Deselecting ListItem " + selectedListItem.name);
            selectedListItem?.onDeselected?.Invoke();

            selectedListItem = listItems[listIndex];

            Debug.Log("Selecting ListItem " + selectedListItem.name);
            selectedListItem?.onSelected?.Invoke();
        }
    }
}

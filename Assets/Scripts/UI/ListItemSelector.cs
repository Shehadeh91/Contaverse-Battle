using UnityEngine;

namespace Contaquest.UI
{
    public class ListItemSelector : MonoBehaviour
    {
        [SerializeField] private ListItemData listItemData;
        [SerializeField] private ListItemData currentItemData;

        public void SelectListItem(bool value)
        {
            if (value)
                SelectListItem();
            else
                DeselectListItem();
        }
        public void SelectListItem()
        {
            Debug.Log("Selecting List item " + listItemData.name);
            listItemData?.onSelected?.Invoke();
            currentItemData?.onDeselected?.Invoke();
        }
        public void DeselectListItem()
        {
            Debug.Log("Deselecting List item " + listItemData.name);
            listItemData?.onDeselected?.Invoke();
        }
    }
}

using UnityEngine;
using TMPro;
using UnityEngine.UI;

namespace Contaquest.UI
{
    public class UIListItem : MonoBehaviour
    {
        [Header("References")]
        [SerializeField]
        private TextMeshProUGUI text;
        [SerializeField]
        private Image image;

        private int listIndex;

        private UIList uIList;
        private ListItemData listItemData;

        public void Initialize(UIList uIList, ListItemData listItemData, int listIndex)
        {
            this.uIList = uIList;
            this.listItemData = listItemData;

            if(text != null)
                text.text = listItemData.title;
            if (image != null)
                image.sprite = listItemData.sprite;
            this.listIndex = listIndex;
        }

        public void Select()
        {
            uIList.OnItemSelected(listIndex);
        }
    }
}

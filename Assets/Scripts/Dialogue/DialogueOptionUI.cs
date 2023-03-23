using UnityEngine;
using TMPro;
using Sirenix.OdinInspector;

namespace Contaquest.Metaverse.Dialogue
{
    public class DialogueOptionUI : MonoBehaviour
    {
        [TabGroup("References")] [SerializeField] private TextMeshProUGUI text;
        private DialogueOption currentDialogueOption;

        public void ShowOption(DialogueOption dialogueOption)
        {
            currentDialogueOption = dialogueOption;
            text.text = dialogueOption.optionName;
        }

        public void SelectOption()
        {
            currentDialogueOption.SelectOption();
            DialogueManager.Instance.SelectDialogueOption(currentDialogueOption.targetNodeId);
        }
    }
}
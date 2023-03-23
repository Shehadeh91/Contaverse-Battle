using UnityEngine;

namespace Contaquest.Metaverse.Dialogue
{
    [System.Serializable]
    public class DialogueOption
    {
        public string optionName;
        public int targetNodeId = -1;

        public GameEvent onSelectEvent;
        public DialogueOption(string optionName, int targetNodeId)
        {
            this.optionName = optionName;
            this.targetNodeId = targetNodeId;
        }

        public void SelectOption()
        {
            onSelectEvent?.Raise();
        }
    }
}
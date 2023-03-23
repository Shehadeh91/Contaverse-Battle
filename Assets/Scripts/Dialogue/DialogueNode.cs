
using UnityEngine;

namespace Contaquest.Metaverse.Dialogue
{
    [System.Serializable]
    public class DialogueNode
    {
        public int nodeId = -1;
        [TextArea] public string dialogueText;
        public DialogueEmotion dialogueEmotion = DialogueEmotion.Neutral;
        public DialogueOption[] dialogueOptions;

#if UNITY_EDITOR
        public void OnValidate(DialogueEncounter encounter, int index)
        {
            if(nodeId == -1)
                nodeId = Random.Range(1, int.MaxValue);
            if(dialogueOptions.Length == 0)
            {
                
            }
            else
            {
                if(index + 1 < encounter.dialogueNodes.Count)
                {
                    foreach (DialogueOption dialogueOption in dialogueOptions)
                    {
                        if(string.IsNullOrEmpty(dialogueOption.optionName))
                            dialogueOption.optionName = "continue";
                        if(dialogueOption.targetNodeId == -1)
                            dialogueOption.targetNodeId = encounter.dialogueNodes[index+1].nodeId;
                    }
                }
            }
        }
#endif
    }
}
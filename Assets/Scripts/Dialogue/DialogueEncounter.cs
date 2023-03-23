using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Contaquest.Metaverse.Dialogue
{
    [CreateAssetMenu(menuName = "Dailogue/Dialogue Encounter")]
    public class DialogueEncounter : ScriptableObject
    {
        [SerializeField] private int startNodeId = -1;
        // public DialogueCharacter dialogueCharacter;
        public DialogueRole dialogueRole;
        public List<DialogueNode> dialogueNodes;

        private void OnValidate()
        {
            if(startNodeId == -1 && dialogueNodes.Count != 0)
                startNodeId = dialogueNodes[0].nodeId;
#if UNITY_EDITOR
            for (var i = 0; i < dialogueNodes.Count; i++)
            {
                dialogueNodes[i].OnValidate(this, i);
            }
#endif
        }

        public DialogueCharacter GetCharacter(Faction faction)
        {
            // if(dialogueCharacter != null)
            //     return dialogueCharacter;
            if(dialogueRole == null)
                return null;
                
            return dialogueRole.GetCharacter(faction);
        }
        public DialogueNode GetStartNode()
        {
            return GetNodeById(startNodeId);
        }
        public DialogueNode GetNodeById(int nodeId)
        {
            return dialogueNodes.FirstOrDefault(x => x.nodeId == nodeId);
        }
    }
}
using System.Net.Mail;
using UnityEngine;
using Sirenix.OdinInspector;
using Contaquest.Server;

namespace Contaquest.Metaverse.Dialogue
{
    public class DialogueManager : PersistentGenericSingleton<DialogueManager>
    {
        [TabGroup("References")] [SerializeField] private DialogueEncounterUI dialogueEncounterUI;
        [TabGroup("State")] [ShowInInspector, ReadOnly] private DialogueEncounter currentEncounter;
        [TabGroup("State")] [ShowInInspector, ReadOnly] private DialogueNode currentNode;
        
        public bool StartEncounter(DialogueEncounter encounter)
        {
            if(currentEncounter != null)
                return false;

            if(SaveManager.Instance.GetBool(encounter.name))
                return false;

            dialogueEncounterUI.StartDialogueEncounter();
            currentEncounter = encounter;
            SetCurrentDialogueNode(encounter.GetStartNode());
            return true;
        }
        public void SetCurrentDialogueNode(DialogueNode nextNode)
        {
            if(nextNode == null)
            {
                Debug.LogError("This dialogueNode is null!");
                return;
            }
            currentNode = nextNode;
            Faction userFaction = UserManager.Instance.GetFaction();
            DialogueCharacter dialogueCharacter = currentEncounter.GetCharacter(userFaction);

            dialogueEncounterUI?.ShowDialogue(currentEncounter, currentNode, dialogueCharacter);
        }

        public void SelectDialogueOption(int nodeId)
        {
            if(currentEncounter == null)
                return;

            if(nodeId == -1)
            {
                FinishEncounter();
                return;
            }
            DialogueNode nextNode = currentEncounter.GetNodeById(nodeId);

            if(nextNode != null)
                SetCurrentDialogueNode(nextNode);
            else
                FinishEncounter();
        }

        public void ContinueDialogue()
        {
            if(currentEncounter == null)
                return;

            int index = currentEncounter.dialogueNodes.IndexOf(currentNode);

            if(index + 1 < currentEncounter.dialogueNodes.Count)
            {
                DialogueNode nextNode = currentEncounter.dialogueNodes[index + 1];
                SetCurrentDialogueNode(nextNode);
            }
            else
            {
                FinishEncounter();
            }
        }

        public void FinishEncounter()
        {
            SaveManager.Instance.SetBool(currentEncounter.name, true);
            currentEncounter = null;

            dialogueEncounterUI.FinishDialogueEncounter();
        }
    }
}
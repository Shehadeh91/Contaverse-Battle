using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Sirenix.OdinInspector;
using UnityEngine.Events;
using System.Collections.Generic;
using Contaquest.Server;

namespace Contaquest.Metaverse.Dialogue
{
    public class DialogueEncounterUI : MonoBehaviour
    {
        [TabGroup("References")] [SerializeField] private TextMeshProUGUI text;
        [TabGroup("References")] [SerializeField] private TextMeshProUGUI characterNameText;
        [TabGroup("References")] [SerializeField] private Image characterImage;
        [TabGroup("References")] [SerializeField] private GameObject continueButton;
        [TabGroup("References")] [SerializeField] private DialogueOptionUI dialogueOptionPrefab;
        [TabGroup("References")] [SerializeField] private Transform dialogueOptionParent;
        [TabGroup("Events")] [SerializeField] private UnityEvent onEncounterStarted, onNextDialogueNode, onEncounterFinished;
        [TabGroup("State")] [ShowInInspector, ReadOnly] private List<DialogueOptionUI> dialogueOptionInstances = new List<DialogueOptionUI>();


        public void StartDialogueEncounter()
        {
            onEncounterStarted?.Invoke();
        }

        public void FinishDialogueEncounter()
        {
            onEncounterFinished?.Invoke();
        }

        public void ShowDialogue(DialogueEncounter dialogueEncounter, DialogueNode dialogueNode, DialogueCharacter dialogueCharacter)
        {
            string dialogueText = dialogueNode.dialogueText;
            string userName = UserManager.Instance.GetUserName();
            dialogueText = dialogueText.Replace("-user-", userName);
            text.text = dialogueText;

            if(dialogueCharacter == null)
            {
                characterNameText.gameObject.SetActive(false);
                characterImage.gameObject.SetActive(false);
            }
            else
            {
                characterNameText.gameObject.SetActive(true);
                characterImage.gameObject.SetActive(true);

                DialogueEmotion currentEmotion = dialogueNode.dialogueEmotion;
                DialogueSprite dialogueSprite = dialogueCharacter.GetSprite(currentEmotion);

                characterNameText.text = dialogueCharacter.characterName + " - "  + dialogueCharacter.faction.ToString() + " " + dialogueEncounter.dialogueRole.roleName;
                characterImage.sprite = dialogueSprite.sprite;
            }

            if(dialogueNode.dialogueOptions.Length == 0)
            {
                continueButton.SetActive(true);
                for (var i = 0; i < dialogueOptionInstances.Count; i++)
                {
                    dialogueOptionInstances[i].gameObject.SetActive(false);
                }
            }
            else
            {
                continueButton.SetActive(false);
                while(dialogueNode.dialogueOptions.Length != dialogueOptionInstances.Count)
                {
                    if(dialogueNode.dialogueOptions.Length < dialogueOptionInstances.Count)
                    {
                        var ui = dialogueOptionInstances[dialogueOptionInstances.Count - 1];
                        dialogueOptionInstances.RemoveAt(dialogueOptionInstances.Count - 1);
                        Destroy(ui.gameObject);
                    }
                    else
                    {
                        dialogueOptionInstances.Add(Instantiate(dialogueOptionPrefab, dialogueOptionParent));
                    }
                }

                for (var i = 0; i < dialogueOptionInstances.Count; i++)
                {
                    dialogueOptionInstances[i].gameObject.SetActive(true);
                    dialogueOptionInstances[i].ShowOption(dialogueNode.dialogueOptions[i]);
                }
            }
            onNextDialogueNode?.Invoke();
        }

        public void ContinueDialogue()
        {
            DialogueManager.Instance.ContinueDialogue();
        }
    }
}
using Contaquest.Server;
using UnityEngine;
using UnityEngine.Events;

namespace Contaquest.Metaverse.Dialogue
{
    public class DialogueEncounterTrigger : MonoBehaviour
    {
        [SerializeField] private DialogueEncounter encounter;
        [SerializeField] private DialogueEncounter[] factionEncounters;
        [SerializeField] private UnityEvent onDialogueTriggered;

        public void StartDialogueEncounter()
        {
            StartDialogueEncounter(encounter);
        }

        public void StartEncounterUserFaction()
        {
            Faction faction = UserManager.Instance.GetFaction();
            DialogueEncounter selectedEncounter = factionEncounters[(int) faction];
            StartDialogueEncounter(encounter);
        }

        public void StartDialogueEncounter(DialogueEncounter encounter)
        {
            if(DialogueManager.Instance.StartEncounter(encounter))
                onDialogueTriggered?.Invoke();
        }
    }
}
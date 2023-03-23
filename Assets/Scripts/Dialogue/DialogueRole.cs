using UnityEngine;
using System.Collections.Generic;
using System.Linq;

namespace Contaquest.Metaverse.Dialogue
{
    [CreateAssetMenu(menuName = "Dailogue/Dialogue Role")]
    public class DialogueRole : ScriptableObject
    {
        public string roleName;
        public List<DialogueCharacter> dialogueCharacters;

        public DialogueCharacter GetCharacter(Faction faction)
        {
            if (faction == Faction.NoFaction)
                return null;

            return dialogueCharacters.FirstOrDefault(x => x.faction == faction);
        }
    }
}
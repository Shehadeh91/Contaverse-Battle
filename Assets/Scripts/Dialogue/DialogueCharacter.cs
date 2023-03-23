using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Contaquest.Metaverse.Dialogue
{
    [CreateAssetMenu(menuName = "Dailogue/Dialogue Character")]
    public class DialogueCharacter : ScriptableObject
    {
        public string characterName;
        public string characterDescription;
        public Faction faction = Faction.NoFaction;
        [SerializeField] private List<DialogueSprite> dialogueSprites;
        public DialogueSprite GetSprite(DialogueEmotion emotion)
        {
            DialogueSprite dialogueSprite = dialogueSprites.FirstOrDefault((x) => x.emotion == emotion);

            if(dialogueSprite == null)
            {
                dialogueSprite = dialogueSprites[0];
            }

            return dialogueSprite;
        }
    }

    [System.Serializable]
    public class DialogueSprite
    {
        public Sprite sprite;
        public DialogueEmotion emotion;
    }
}
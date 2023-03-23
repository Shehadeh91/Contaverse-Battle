using Contaquest.Metaverse.Data;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Contaquest.Metaverse.Robot
{
    [System.Serializable]
    public class RobotPartCollectionItem
    {
        [SerializeField] [LabelWidth(130)] [HorizontalGroup("Group")] private bool useTextAsset = true;
        [SerializeField] [LabelWidth(80)] [HorizontalGroup("Group")] private EquipSlot equipSlot;
        [SerializeField] [ShowIf("useTextAsset")] private TextAsset jsonFiles;
        [SerializeField] [HideIf("useTextAsset")] private EquippableData equippableData;

        public EquippableData GetEquippableData()
        {
            if(useTextAsset)
            {
                equippableData = new EquippableData();
                MetaData metaData = MetadataSerializer.Instance.Deserialize(jsonFiles.text);
                ItemManager.Instance.LoadItem(metaData.itemData);
                equippableData.robotPartData = metaData.itemData as RobotPart;

                if (equippableData.robotPartData is null)
                    return null;
                equippableData.equipSlot = equipSlot;
                return equippableData;
            }
            else
            {
                equippableData.equipSlot = equipSlot;
                return equippableData;
            }
        }
    }
}

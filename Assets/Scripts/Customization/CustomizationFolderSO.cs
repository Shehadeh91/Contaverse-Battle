using System.Collections.Generic;
using Contaquest.Metaverse.Data;
using Contaquest.UI;
using UnityEngine;
using System.Linq;

namespace Contaquest.Metaverse.Robot.Customization
{
    [CreateAssetMenu(menuName ="Data/Customization Folder")]
    public class CustomizationFolderSO : ListItemData
    {
        public EquipSlot equipSlot;
        public RobotPart[] GetRobotPartDatas()
        {
            RobotPart[] robotParts = WalletController.Instance.GetRobotPartsInWallet().Where((part) => part.equipSlot == equipSlot).ToArray();

            return robotParts;
        }
        public void Initialize(CustomizationSelector customizationSelector)
        {
            onSelected += ()=> customizationSelector.OnFolderSelected(this);
            onDeselected += ()=> customizationSelector.OnFolderDeselected(this);
        }
    }
}

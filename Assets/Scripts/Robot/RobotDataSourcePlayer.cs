using Contaquest.Metaverse.Data;
using Contaquest.Metaverse.Robot;
using UnityEngine;

namespace Contaquest.Metaverse
{
    public class RobotDataSourcePlayer : RobotDataSource
    {
        public override RobotData GetRobotData()
        {
            return robotData;
        }

        public override void LoadRobotData()
        {
            base.LoadRobotData();
            AssignRobotPartsFromWallet();
        }

        private void AssignRobotPartsFromWallet()
        {
            //WalletController.Instance.onLoadingFinished?.RemoveListener(AssignRobotPartsFromWallet);

            robotData = SaveManager.Instance.LoadRobotData(1);
            foreach (var equippableData in robotData.equippableDatas)
            {
                if (string.IsNullOrEmpty(equippableData.nftID) || equippableData == null)
                {
                    equippableData.robotPartData = ItemManager.Instance.GetDefaultPart(equippableData.equipSlot);
                    continue;
                }

                if(WalletController.Instance.walletItems.TryGetValue(equippableData.nftID, out WalletItem walletItem))
                {
                    RobotPart robotPart = walletItem.metaData.itemData as RobotPart;
                    if (robotPart != null)
                        equippableData.robotPartData = robotPart;
                    else
                    {
                        equippableData.robotPartData = ItemManager.Instance.GetDefaultPart(equippableData.equipSlot);
                        continue;
                    }
                }
                else
                {
                    equippableData.robotPartData = ItemManager.Instance.GetDefaultPart(equippableData.equipSlot);
                    continue;
                }
            }
        }
    }
}

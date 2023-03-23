using Contaquest.Metaverse.Data;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Contaquest.Metaverse.Robot
{
    [CreateAssetMenu(menuName = "Data/Robot Part Collection")]
    public class RobotPartCollection : ScriptableObject
    {
        public RobotPartCollectionItem[] robotPartCollectionItems;

        public RobotData GetRobotData()
        {
            EquippableData[] equippableDatas = new EquippableData[robotPartCollectionItems.Length];

            for (int i = 0; i < equippableDatas.Length; i++)
            {
                equippableDatas[i] = robotPartCollectionItems[i].GetEquippableData();
            }
            RobotData robotData = new RobotData(equippableDatas);
            return robotData;
        }
    }
}

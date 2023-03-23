using System.Linq;
namespace Contaquest.Metaverse.Data
{
    [System.Serializable]
    public class RobotData
    {
        public EquippableData[] equippableDatas;

        public RobotData(EquippableData[] equippableDatas)
        {
            this.equippableDatas = equippableDatas;
        }
    }
}

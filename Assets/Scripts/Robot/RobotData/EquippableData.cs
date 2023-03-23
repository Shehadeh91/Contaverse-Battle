namespace Contaquest.Metaverse.Data
{
    [System.Serializable]
    public class EquippableData
    {
        public EquipSlot equipSlot;
        public string nftID;
        public RobotPart robotPartData;

        public EquippableData()
        {
        }
        public EquippableData(EquipSlot equipSlot, string nftID)
        {
            this.equipSlot = equipSlot;
            this.nftID = nftID;
        }
    }
}
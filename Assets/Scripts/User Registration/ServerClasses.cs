using System;
using Contaquest.Metaverse.Data;
using JetBrains.Annotations;
using Newtonsoft.Json;

namespace Contaquest.Server
{
    [System.Serializable]
    public class UserAccountData
    {
        [JsonProperty("userName")] public string UserName;

        [JsonProperty("displayName", NullValueHandling = NullValueHandling.Ignore)]
        public string DisplayName { get; set; }

        [JsonProperty("email")] public string Email { get; set; }

        [JsonProperty("acceptTerms", NullValueHandling = NullValueHandling.Ignore)]
        public bool AcceptTerms { get; set; }

        // public DateTime UserCreated { get; set; }

        [JsonProperty("role", NullValueHandling = NullValueHandling.Ignore)]
        public Role Role { get; set; }

        [JsonProperty("faction", NullValueHandling = NullValueHandling.Ignore)]
        public Faction Faction;

        [JsonProperty("level", NullValueHandling = NullValueHandling.Ignore)]
        public int Level { get; set; }

        [JsonProperty("experience", NullValueHandling = NullValueHandling.Ignore)]
        public int XP { get; set; }


        [JsonProperty("enjinData")] public EnjinData EnjinData { get; set; }
        [JsonProperty("chargeData")] public ChargeData ChargeData { get; set; }
        [JsonProperty("parts")] public UserParts Parts { get; set; }

        public UserAccountData()
        {
        }

        public UserAccountData(string userName, Faction faction, EnjinData enjinData)
        {
            UserName = userName;
            Faction = faction;
            EnjinData = enjinData;

            XP = 0;
            Level = 0;
            this.ChargeData = new ChargeData(false, 4);
        }
    }

    public enum Role
    {
        User,
        Admin
    }
    public class ChargeData
    {

        [JsonProperty("isCharging", NullValueHandling = NullValueHandling.Ignore)] public bool isCharging;
        [JsonProperty("startedCharging", NullValueHandling = NullValueHandling.Ignore)] public object startedCharging;
        [JsonProperty("chargeAmount", NullValueHandling = NullValueHandling.Ignore)] public int chargeAmount;
        public ChargeData(bool isCharging, int chargeAmount)
        {
            this.isCharging = isCharging;
            this.chargeAmount = chargeAmount;
        }
    }

    public class UserParts
    {
        public string head;
        public string torso;
        public string leftArm;
        public string rightArm;
        public string legs;
        public UserParts(RobotData robotData)
        {
            if(robotData == null)
                return;
                
            foreach (var equippableData in robotData.equippableDatas)
            {
                switch (equippableData.equipSlot)
                {
                    case EquipSlot.Head:
                        head = equippableData.nftID;
                        break;
                    case EquipSlot.Body:
                        torso = equippableData.nftID;
                        break;
                    case EquipSlot.LeftArm:
                        leftArm = equippableData.nftID;
                        break;
                    case EquipSlot.RightArm:
                        rightArm = equippableData.nftID;
                        break;
                    case EquipSlot.Legs:
                        legs = equippableData.nftID;
                        break;
                    default:
                        break;
                }
            }
        }
    }

    public class EnjinData
    {
        [JsonProperty("enjinUserName")] public string EnjinUserName { get; set; }

        [JsonProperty("enjinIdentityID")] public long EnjinIdentityID { get; set; }

        [JsonProperty("ethAddress")]
        public string EthAddress { get; set; }

        [JsonProperty("isWalletLinked")] public bool IsWalletLinked { get; set; }

        [JsonProperty("linkingInfo", NullValueHandling = NullValueHandling.Ignore)]
        public LinkingInfoClass LinkingInfo { get; set; }

        public EnjinData(string enjinUserName, int enjinUserID, int enjinIdentityID, string ethAddress = "",
            bool isWalletLinked = false)
        {
            EnjinUserName = enjinUserName;
            EnjinIdentityID = enjinIdentityID;
            IsWalletLinked = isWalletLinked;
        }

        public class Parts
        {
            [JsonProperty("head")] public string Head { get; set; }

            [JsonProperty("torso")] public string Torso { get; set; }

            [JsonProperty("leftArm")] public string LeftArm { get; set; }

            [JsonProperty("rightArm")] public string RightArm { get; set; }

            [JsonProperty("legs")] public string Legs { get; set; }
        }

        public class LinkingInfoClass
        {
            [JsonProperty("qr", NullValueHandling = NullValueHandling.Ignore)] public Uri Qr { get; set; }

            [JsonProperty("code", NullValueHandling = NullValueHandling.Ignore)] public string Code { get; set; }
        }
    }
}
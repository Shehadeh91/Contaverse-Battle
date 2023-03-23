using Sirenix.OdinInspector;
using System;
using UnityEngine;
using System.Linq;
using System.Collections.Generic;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.AddressableAssets;
using Newtonsoft.Json;
using System.Text.RegularExpressions;

namespace Contaquest.Metaverse.Data
{
    [System.Serializable][JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
    public class RobotPart : Item
    {
        #region Json Saving and Loading
        [JsonProperty(PropertyName = "Equip Slot")]
        /*[HideInInspector]*/ public string equipSlotValue;
        [JsonProperty(PropertyName = "Rarity")]
        [HideInInspector] public string rarityValue;

        [JsonProperty(PropertyName = "Health")]
        [HideInInspector] public float healthValue;
        [JsonProperty(PropertyName = "Speed")]
        [HideInInspector] public float speedValue;
        [JsonProperty(PropertyName = "Damage")]
        [HideInInspector] public float damageValue;
        [JsonProperty(PropertyName = "Defense")]
        [HideInInspector] public float defenseValue;
        [JsonProperty(PropertyName = "Weight")]
        [HideInInspector] public float weightValue;
        [JsonProperty(PropertyName = "Agility")]
        [HideInInspector] public float agilityValue;

        [JsonProperty(PropertyName = "Primary Color")]
        [HideInInspector] public string primaryColorValue;
        [JsonProperty(PropertyName = "Secondary Color")]
        [HideInInspector] public string secondaryColorValue;
        [JsonProperty(PropertyName = "Tertiary Color")]
        [HideInInspector] public string tertiaryColorValue;
        #endregion

        [BoxGroup("Item Properties")] [JsonIgnore] public ItemRarity itemRarity;
        [BoxGroup("Item Properties")] [JsonIgnore] public EquipSlot equipSlot;
        [BoxGroup("Item Properties")] [JsonIgnore] public Color primaryColor;
        [BoxGroup("Item Properties")] [JsonIgnore] public Color secondaryColor;
        [BoxGroup("Item Properties")] [JsonIgnore] public Color tertiaryColor;

        [TabGroup("Base Stats")] [JsonIgnore] public RobotStats stats = new RobotStats();
        [JsonProperty(PropertyName = "Behaviours")]
        [TabGroup("Behaviours")] public BehaviourData[] behaviours = new BehaviourData[0];
        [JsonProperty(PropertyName = "Upgrades")]
        [TabGroup("Upgrades")] public UpgradeData[] upgrades = new UpgradeData[0];

        #region Constructors
        //[JsonConstructor]
        //public RobotPart(string itemType, string itemAddress, string rarityValue, float healthPointValue, float speedValue, float damageValue, float defenseValue, float weightValue, float agilityValue, string primaryColorValue, string secondaryColorValue, string equipSlotValue, BehaviourData[] behaviours, UpgradeData[] upgrades)
        //{
        //    this.itemType = itemType;
        //    this.itemAddress = itemAddress;
        //    this.rarityValue = rarityValue;
        //    this.healthPointValue = healthPointValue;
        //    this.speedValue = speedValue;
        //    this.damageValue = damageValue;
        //    this.defenseValue = defenseValue;
        //    this.weightValue = weightValue;
        //    this.agilityValue = agilityValue;
        //    this.primaryColorValue = primaryColorValue;
        //    this.secondaryColorValue = secondaryColorValue;
        //    this.equipSlotValue = equipSlotValue;
        //    this.behaviours = behaviours;
        //    this.upgrades = upgrades;
        //}

        public RobotPart(string itemType, string itemAddress, ItemRarity itemRarity, string equipSlotValue, Color col1, Color col2, Color col3, RobotStats stats, BehaviourData[] behaviours, UpgradeData[] upgrades)
        {
            this.itemType = itemType;
            this.address = itemAddress;
            this.itemRarity = itemRarity;
            this.stats = stats;
            this.equipSlotValue = equipSlotValue.Replace(" ", string.Empty);
            if(this.equipSlotValue == "Arm")
                equipSlot = EquipSlot.LeftArm;
            else
            {
                Enum.TryParse(equipSlotValue, out EquipSlot slot);
                equipSlot = slot;
            }
            primaryColor = col1;
            secondaryColor = col2;
            tertiaryColor = col3;
            this.behaviours = behaviours;
            this.upgrades = upgrades;
        }
        #endregion

        public override void Initialize()
        {
            base.Initialize();
            // Debug.Log(equipSlotValue);
            if(equipSlotValue == "Arm")
                equipSlot = EquipSlot.LeftArm;
            else
            {
                Enum.TryParse(equipSlotValue/*.Replace(" ", string.Empty)*/, out EquipSlot slot);
                equipSlot = slot;
            }
            
            rarityValue = rarityValue.Replace(" ", string.Empty);
            Enum.TryParse(rarityValue, out ItemRarity rarity);
            itemRarity = rarity;

            stats = new RobotStats();
            stats.stats.Add(new Stat("HP", healthValue));
            stats.stats.Add(new Stat("DMG", damageValue));
            stats.stats.Add(new Stat("DEF", defenseValue));
            stats.stats.Add(new Stat("WGT", weightValue));
            stats.stats.Add(new Stat("SPD", speedValue));
            stats.stats.Add(new Stat("AGL", agilityValue));
            stats.Initialize();

            ColorUtility.TryParseHtmlString('#' + primaryColorValue, out Color col1);
            primaryColor = col1;
            ColorUtility.TryParseHtmlString('#' + secondaryColorValue, out Color col2);
            secondaryColor = col2;
            ColorUtility.TryParseHtmlString('#' + tertiaryColorValue, out Color col3);
            tertiaryColor = col3;
        }

        public override void InitializeValuesBeforeSerialization()
        {
            base.InitializeValuesBeforeSerialization();

            if(equipSlot == EquipSlot.LeftArm || equipSlot == EquipSlot.RightArm)
                equipSlotValue = "Arm";
            else
                equipSlotValue = equipSlot.ToString().FromCamelCase();
            Debug.Log(equipSlotValue);
            rarityValue = itemRarity.ToString();

            stats.Initialize();
            healthValue = stats.GetStat("HP") != null ? stats.GetStat("HP").Value : 0;
            damageValue = stats.GetStat("DMG") != null ? stats.GetStat("DMG").Value : 0;
            defenseValue = stats.GetStat("DEF") != null ? stats.GetStat("DEF").Value : 0;
            weightValue = stats.GetStat("WGT") != null ? stats.GetStat("WGT").Value : 0;
            speedValue = stats.GetStat("SPD") != null ? stats.GetStat("SPD").Value : 0;
            agilityValue = stats.GetStat("AGL") != null ? stats.GetStat("AGL").Value : 0;

            primaryColorValue = ColorUtility.ToHtmlStringRGB(primaryColor);
            secondaryColorValue = ColorUtility.ToHtmlStringRGB(secondaryColor);
            tertiaryColorValue = ColorUtility.ToHtmlStringRGB(tertiaryColor);
            if(tertiaryColorValue == "FFFFFF")
                tertiaryColorValue = null;
        }

        public MetaDataSimple GetMetaDataSimple()
        {
            InitializeValuesBeforeSerialization();

            MetaDataSimple metaDataSimple = new MetaDataSimple(itemType, itemRarity,
                healthValue, damageValue, defenseValue, weightValue, speedValue, agilityValue,
                equipSlot, primaryColorValue, secondaryColorValue);
            return metaDataSimple;
        }

        #region Asset Downloading

        public override void OnAssetDownloaded(GameObject prefab)
        {
            base.OnAssetDownloaded(prefab);
            foreach (var upgrade in upgrades)
            {
                upgrade.StartAssetDownload();
            }
        }
        #endregion
    }
}
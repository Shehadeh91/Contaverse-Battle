using System;
using Contaquest.Metaverse;
using Contaquest.Metaverse.Data;
using UnityEngine;
public class MetaDataSimple
{
    public string Name { get; set; }
    public string Description { get; set; }
    public string Image { get; set; }

    public string ItemType { get; set; }
    public string ItemRarity { get; set; }

    public float HP { get; set; }
    public float DMG { get; set; }
    public float DEF { get; set; }
    public float WGT { get; set; }
    public float SPD { get; set; }
    public float AGL { get; set; }

    public string EquipSlot { get; set; }

    public string PrimaryColor { get; set; }
    public string SecondaryColor{ get; set; }
    public string TertiaryColor{ get; set; }

    public MetaDataSimple()
    {
    }

    public MetaDataSimple(string itemType, ItemRarity itemRarity, float hP, float dMG, float dEF, float wGT, float sPD, float aGL, EquipSlot equipSlot, string primaryColor, string secondaryColor)
    {
        // this.Name = name;
        // this.Description = description;
        // this.Image = image;
        this.ItemType = itemType;
        this.ItemRarity = itemRarity.ToString();
        this.HP = hP;
        this.DMG = dMG;
        this.DEF = dEF;
        this.WGT = wGT;
        this.SPD = sPD;
        this.AGL = aGL;
        this.EquipSlot = equipSlot.ToString();
        this.PrimaryColor = primaryColor;
        this.SecondaryColor = secondaryColor;
    }

    public RobotPart GetRobotPartData()
    {
        RobotStats stats = new RobotStats();
        stats.stats.Add(new Stat("HP", HP));
        stats.stats.Add(new Stat("DMG", DMG));
        stats.stats.Add(new Stat("DEF", DEF));
        stats.stats.Add(new Stat("WGT", WGT));
        stats.stats.Add(new Stat("SPD", SPD));
        stats.stats.Add(new Stat("AGL", AGL));
        stats.Initialize();

        Enum.TryParse(ItemRarity, out ItemRarity rarity);

        ColorUtility.TryParseHtmlString(PrimaryColor, out UnityEngine.Color col1);
        ColorUtility.TryParseHtmlString(SecondaryColor, out UnityEngine.Color col2);
        ColorUtility.TryParseHtmlString(TertiaryColor, out UnityEngine.Color col3);

        RobotPart robotPartData = new RobotPart(ItemType.Replace(" ", ""), Name, rarity, EquipSlot, col1, col2, col3, stats, new BehaviourData[0], new UpgradeData[0]);
        return robotPartData;
    }
    public NFTData GetNFTData()
    {
        return new NFTData(Name, Description, Image);
    }

    public MetaData GetMetaData()
    {
        RobotPart robotPartData = GetRobotPartData();
        NFTData nFTData = GetNFTData();
        return new MetaData(nFTData, robotPartData);

    }
}
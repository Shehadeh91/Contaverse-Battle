using Contaquest.Metaverse.Data;
using CsvHelper;
using CsvHelper.Configuration;

public class MetaDataSimpleMap : ClassMap<MetaDataSimple>
{
    public MetaDataSimpleMap()
    {
        Map(m => m.Name).Index(0).Name("name");
        Map(m => m.Description).Index(1).Name("description");
        Map(m => m.Image).Index(2).Name("image");
        Map(m => m.ItemType).Index(3).Name("ItemType");
        Map(m => m.ItemRarity).Index(5).Name("ItemTier");
        Map(m => m.HP).Index(6).Name("HP");
        Map(m => m.DMG).Index(7).Name("DMG");
        Map(m => m.DEF).Index(8).Name("DEF");
        Map(m => m.WGT).Index(9).Name("WGT");
        Map(m => m.SPD).Index(10).Name("SPD");
        Map(m => m.AGL).Index(11).Name("AGL");
        Map(m => m.EquipSlot).Index(12).Name("EquipSlot");
        Map(m => m.PrimaryColor).Index(13).Name("PrimaryColor");
        Map(m => m.SecondaryColor).Index(14).Name("SecondaryColor");
    }
}
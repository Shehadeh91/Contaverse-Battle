// using Contaquest.Metaverse.Data;
// using UnityEngine;

// namespace Contaquest.Metaverse.Shop
// {
//     [System.Serializable]
//     public class ShopItemData
//     {
//         [SerializeField] private TextAsset textAsset;
//         private MetaData metaData;

//         public string title;
//         public float  price;
//         [Range(1, 5)]
//         public int    tier;

//         public MetaData GetMetaData()
//         {
//             if(metaData == null)
//                 metaData = MetadataSerializer.Instance.Deserialize(textAsset.text);

//             return metaData;
//         }
//     }
// }

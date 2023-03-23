// using Contaquest.Metaverse.Data;
// using UnityEngine;

// namespace Contaquest.Metaverse.Shop
// {
//     [CreateAssetMenu(menuName ="Data/Shop Data")]
//     public class ShopData : ScriptableObject
//     {
//         public ShopItemData[] shopItemDatas;

//         //Call this function before accessing the prefab variable inside the itemdata
//         public void StartDownloadItemDataAssets()
//         {
//             foreach (var shopItemData in shopItemDatas)
//             {
//                 shopItemData.GetMetaData().itemData.StartAssetDownload();
//             }
//         }
//     }
// }

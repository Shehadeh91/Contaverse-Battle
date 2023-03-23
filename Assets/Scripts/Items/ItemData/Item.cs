using System;
using UnityEngine;
using Sirenix.OdinInspector;
using UnityEngine.Events;
using Newtonsoft.Json;
namespace Contaquest.Metaverse.Data
{
    [System.Serializable]
    public class Item
    {
        [JsonProperty(PropertyName = "Item Type", Order = -2)]
        [BoxGroup("Item Properties")] public string itemType;
        [JsonIgnore] public string fullItemType => "Contaquest.Metaverse.Data." + itemType.ToCamelCase();

        protected string address = "";
        [JsonIgnore] public string itemAddress => string.IsNullOrEmpty(address)? nftData.name : address;
        [JsonIgnore] public NFTData nftData;
        [NonSerialized] private bool isInitialized = false;
        [NonSerialized] public bool isDownloaded = false;
        [NonSerialized] [HideInInspector] public GameObject prefab;

        public  virtual void InitializeValuesBeforeSerialization()
        {
            itemType = GetType().FullName.Remove(0, "Contaquest.Metaverse.Data.".Length).FromCamelCase();
        }

        public virtual void Initialize()
        {
            if (isInitialized)
                return;

            // StartAssetDownload();

            isInitialized = true;
        }
        public virtual void OnAssetDownloaded(GameObject prefab)
        {
            isDownloaded = true;
            this.prefab = prefab;
        }
    }
}
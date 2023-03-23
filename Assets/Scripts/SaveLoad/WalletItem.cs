using System;
using UnityEngine;

namespace Contaquest.Metaverse.Data
{
    [System.Serializable]
    public class WalletItem
    {
        public string metadataURL;
        public string NFTIndex;
        public string NFTId;
        public string metaDataString;
        public MetaData metaData;
        public int amount;
        public WalletItem()
        {
            
        }
        public WalletItem(string metadataURL, string NFTId, string NFTIndex, int amount)
        {
            this.NFTIndex = NFTIndex;
            this.metadataURL = metadataURL;
            this.NFTId = NFTId;
            this.amount = amount;
        }
        public virtual string GetMetadataString()
        {
            return metaDataString;
        }
    }

    [System.Serializable]
    public class DebugWalletItem : WalletItem
    {
        public TextAsset metaDataTextAsset;

        public override string GetMetadataString()
        {
            return metaDataTextAsset.text;
        }
    }
}

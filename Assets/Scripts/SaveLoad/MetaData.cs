using System;
using UnityEngine;
using Sirenix.OdinInspector;

namespace Contaquest.Metaverse.Data
{
    public class MetaData
    {
        public NFTData nftData;
        public Item itemData;

        public MetaData(NFTData nftData, Item itemData)
        {
            this.nftData = nftData;
            this.itemData = itemData;
        }
    }
}
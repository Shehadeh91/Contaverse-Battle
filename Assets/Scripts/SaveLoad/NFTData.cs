using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Contaquest.Metaverse.Data
{
    [System.Serializable]
    public class NFTData
    {
        public string name;
        [TextArea] public string description;
        public string image;
        [NonSerialized] [ShowInInspector] public string nftID;
        [NonSerialized] [ShowInInspector] public string nftIndex;
        [NonSerialized] public Item itemData;

        public NFTData(string name, string description, string image)
        {
            this.name = name;
            this.description = description;
            this.image = image;
        }
    }
}
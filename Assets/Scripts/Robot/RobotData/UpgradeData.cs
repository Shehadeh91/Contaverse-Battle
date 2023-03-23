using System;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;


namespace Contaquest.Metaverse.Data
{
    [System.Serializable]
    public class UpgradeData
    {
        public string upgradeAddress;
        [ListDrawerSettings(Expanded = true)] public float[] upgradeValues;

        public void StartAssetDownload()
        {
            ItemManager.Instance.LoadAssetByAddress(upgradeAddress);
        }
    }
}
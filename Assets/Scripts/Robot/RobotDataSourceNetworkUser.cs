using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Sirenix.OdinInspector;
using Contaquest.Metaverse.Robot;
using Contaquest.Metaverse.Data;
using Contaquest.Server;

namespace Contaquest.Metaverse
{
    public class RobotDataSourceNetworkUser : RobotDataSource
    {
        private int unloadedParts = 0;
        private Action onFinishedCallback;

        public void LoadUserRobotData(string userName, string headID, string bodyID, string armLeftID, string armRightID, string legsID, Action callback = null)
        {
            LoadRobotData();
            onFinishedCallback = callback;

            // TODO: deserialize metadata and assign addressables

            EquippableData[] equippableDatas = new EquippableData[5];
            equippableDatas[0] = new EquippableData(EquipSlot.Head, headID);
            equippableDatas[1] = new EquippableData(EquipSlot.Body, bodyID);
            equippableDatas[2] = new EquippableData(EquipSlot.LeftArm, armLeftID);
            equippableDatas[3] = new EquippableData(EquipSlot.RightArm, armRightID);
            equippableDatas[4] = new EquippableData(EquipSlot.Legs, legsID);
            robotData = new RobotData(equippableDatas);

            CombatGraphQLManager.Instance.GetWalletItems(userName, (walletItems) => AssignRobotParts(walletItems));
        }
        [Button]
        public void DebugLoadData()
        {
            LoadUserRobotData("Username", "", "", "", "", "");
        }

        // Cross reference supplied NFTIds with the NFTs in the user's wallet
        private void AssignRobotParts(WalletItem[] walletItems)
        {
            EquippableData[] equippableDatas = robotData.equippableDatas;

            unloadedParts = 0;
            bool usingOwnParts = false;
            for (int i = 0; i < equippableDatas.Length; i++)
            {
                WalletItem walletItem = GetWalletItemFromWallet(equippableDatas[i], walletItems);
                if (walletItem == null)
                {
                    Debug.Log($"Using Default Part for slot {equippableDatas[i].equipSlot.ToString()}");
                    equippableDatas[i].robotPartData = ItemManager.Instance.GetDefaultPart(equippableDatas[i].equipSlot);
                    continue;
                }

                unloadedParts++;
                usingOwnParts = true;

                // TODO: get the metadata URIs 
                Debug.Log($"Loading NFT metadata {walletItem.NFTId}");

                UnityWebRequest webRequest = UnityWebRequest.Get(walletItem.metadataURL);
                UnityWebRequestAsyncOperation asyncOperation = webRequest.SendWebRequest();
                asyncOperation.completed += (operation) => OnMetadataDownloaded(asyncOperation, equippableDatas[i]);
                
            }

            // for the case that all parts are default parts it will still call the callback
            if (!usingOwnParts)
                onFinishedCallback?.Invoke();
        }

        private WalletItem GetWalletItemFromWallet(EquippableData equippableData, WalletItem[] walletItems)
        {
            if(string.IsNullOrEmpty(equippableData.nftID))
                return null;

            return walletItems.FirstOrDefault((walletItem) => equippableData.nftID == walletItem.NFTId);
        }

        public void OnMetadataDownloaded(UnityWebRequestAsyncOperation asyncOperation, EquippableData equippableData)
        {
            byte[] results = asyncOperation.webRequest.downloadHandler.data;
            string metaDataString = System.Text.Encoding.Default.GetString(results);
            ItemManager.Instance.StartLoadingMetadata(metaDataString, (metaData) => OnItemLoaded(metaData, equippableData));
        }

        public void OnItemLoaded(MetaData metaData, EquippableData equippableData)
        {
            unloadedParts--;
            equippableData.robotPartData = metaData.itemData as RobotPart;
            if(unloadedParts <= 0)
            {
                onFinishedCallback?.Invoke();
            }
        }
    }
}

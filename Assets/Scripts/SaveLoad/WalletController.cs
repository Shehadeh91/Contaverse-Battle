using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Contaquest.Server;
using Contaquest.UI;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;

namespace Contaquest.Metaverse.Data
{
    [DefaultExecutionOrder(-70)]
    public class WalletController : PersistentGenericSingleton<WalletController>
    {
        [SerializeField] private BoolReference isDebugModeEnabled;

        [TabGroup("References")] [SerializeField] private FloatReference cogAmount;
        [TabGroup("References")] [SerializeField] private string cogNftId = "3040000000000979";
        [TabGroup("References")] [SerializeField] private IntReference batteryAmount;
        [TabGroup("References")] [SerializeField] private string batteryNftId = "3040000000000b09";
        private Action onLoadingFinishedAction;

        [TabGroup("State")]public Dictionary<string, WalletItem> walletItems = new Dictionary<string, WalletItem>();
        [TabGroup("State")] [ReadOnly] public float lastUpdateTime = -4;
        [TabGroup("State")] [ReadOnly] [ShowInInspector] private List<RobotPart> robotParts;
        [TabGroup("State")] [ReadOnly] [ShowInInspector] private int unloadedMetadataURIs = 0;
        [SerializeField] private DebugWalletItem[] debugWalletItems;

#if UNITY_EDITOR
        [TabGroup("Debug")] [ShowInInspector] [ReadOnly] private List<WalletItem> walletItemsList = new List<WalletItem>();
#endif

        public void UpdateWalletContents(bool forceUpdate = false, Action callback = null)
        {
            if(lastUpdateTime < 0 || Time.time - lastUpdateTime > 300f || forceUpdate)
            {
                lastUpdateTime = Time.time;

                // Debug.Log("Updating Wallet Contents. Time: " + Time.time);

                robotParts = new List<RobotPart>();
                walletItems = new Dictionary<string, WalletItem>();
                unloadedMetadataURIs = 0;

                #if UNITY_EDITOR
                walletItemsList = new List<WalletItem>();
                #endif

                if(isDebugModeEnabled == null || isDebugModeEnabled.Value)
                {
                    DebugLoadWalletNFTs(debugWalletItems);
                    callback?.Invoke();
                }
                else
                {
                    LoadWalletNFTs(callback);
                }
            }
            else
            {
                callback?.Invoke();
            }
        }

        private async void LoadWalletNFTs(Action callback = null)
        {
            WalletItem[] newWalletItems = await UserManager.Instance.GetWalletItems();
            if(newWalletItems == null || newWalletItems.Length == 0)
            {
                callback?.Invoke();
                return;
            }
            foreach (WalletItem newWalletItem in newWalletItems)
            {
                #if UNITY_EDITOR
                walletItemsList.Add(newWalletItem);
                #endif
                if (walletItems.ContainsKey(newWalletItem.NFTId))
                {
                    // Debug.Log($"The wallet item {newWalletItem.NFTId} already exists in the dictionary.");
                    continue;
                }

                walletItems.Add(newWalletItem.NFTId, newWalletItem);
                DownloadMetadata(newWalletItem, callback);
                if(newWalletItem.NFTId == cogNftId)
                    cogAmount.Value = newWalletItem.amount;
                if(newWalletItem.NFTId == batteryNftId)
                    batteryAmount.Value = newWalletItem.amount;
            }
        }

        private void DownloadMetadata(WalletItem walletItem, Action callback = null)
        {
            unloadedMetadataURIs++;

            LoadingUI.Instance.Enable();
            LoadingUI.Instance.UpdateText("Loading Metadata URIs");
            UnityWebRequest webRequest = UnityWebRequest.Get(walletItem.metadataURL);
            UnityWebRequestAsyncOperation asyncOperation = webRequest.SendWebRequest();
            asyncOperation.completed += (operation) => OnMetadataDownloaded(walletItem, asyncOperation, callback);
        }

        public void OnMetadataDownloaded(WalletItem walletItem, UnityWebRequestAsyncOperation asyncOperation, Action callback = null)
        {
            unloadedMetadataURIs--;
            byte[] results = asyncOperation.webRequest.downloadHandler.data;
            walletItem.metaDataString = System.Text.Encoding.Default.GetString(results);

            if (unloadedMetadataURIs <= 0)
            {
                LoadingUI.Instance.Disable();
                callback?.Invoke();
            }
        }

        public void LoadItems(Action callback = null)
        {
            // Debug.Log("Loading Items: " + Time.time);

            foreach (var walletItem in walletItems)
            {
                if(walletItem.Value.metaData != null)
                {
                    continue;
                }

                MetaData metaData = MetadataSerializer.Instance.Deserialize(walletItem.Value.GetMetadataString());
                ItemManager.Instance.LoadItem(metaData.itemData);
                metaData.nftData.nftID = walletItem.Value.NFTId;
                metaData.nftData.nftIndex = walletItem.Value.NFTIndex;
                walletItem.Value.metaData = metaData;
                walletItem.Value.metaDataString = "";
            }
            
            ItemManager.Instance.WaitForAllDownloads(callback);
        }

        public List<RobotPart> GetRobotPartsInWallet()
        {
            if (robotParts == null || robotParts.Count == 0)
                robotParts = walletItems.Select((arg) => arg.Value.metaData.itemData).OfType<RobotPart>().Where((arg) => arg.prefab != null).ToList();
            return robotParts;
        }
        public void AddDebugWalletItems(DebugWalletItem[] newWalletItems)
        {
            var items = debugWalletItems.ToList();
            items.AddRange(newWalletItems);
            debugWalletItems = items.ToArray();
        }
        public void DebugLoadWalletNFTs(WalletItem[] newWalletItems)
        {
            if (newWalletItems == null || newWalletItems.Length == 0)
                return;

            foreach (WalletItem newWalletItem in newWalletItems)
            {
                #if UNITY_EDITOR
                walletItemsList.Add(newWalletItem);
                #endif
                if (walletItems.ContainsKey(newWalletItem.NFTId))
                    continue;

                walletItems.Add(newWalletItem.NFTId, newWalletItem);
            }
        }
    }
}
using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using Sirenix.OdinInspector;

namespace Contaquest.Metaverse.Data
{
    [DefaultExecutionOrder(-100)]
    public class ItemManager : PersistentGenericSingleton<ItemManager>
    {
        [TabGroup("References")] [SerializeField] private bool loadAssetsOnStart;
        [TabGroup("References")] [SerializeField] [ShowIf("loadAssetsOnStart")] private List<string> assetAddressesToLoad = new List<string>();
        [TabGroup("References")] [ShowInInspector, ReadOnly] private List<Item> defaultItems = new List<Item>();
        [TabGroup("Events")] [SerializeField] private UnityEvent onLoadAssetFailed;
        private Action onAllAssetsLoaded;
        private int unloadedItems
        {
            get
            {
                return startedDownloads.Count - finishedDownloads.Count;
            }
        }
        public Dictionary<string, GameObject> prefabDictionary = new Dictionary<string, GameObject>();
        public HashSet<string> startedDownloads = new HashSet<string>();
        public HashSet<string> finishedDownloads = new HashSet<string>();
        public Dictionary<string, AsyncOperationHandle<GameObject>> asyncOperations = new Dictionary<string, AsyncOperationHandle<GameObject>>();

        protected override void InitializeSingleton()
        {
            var asyncHandler =  Addressables.InitializeAsync();
            asyncHandler.Completed += (arg) => LoadDefaultItems();
            asyncHandler.Completed += (arg) => LoadAssets();
        }

        public MetaData StartLoadingMetadata(string metadataText)
        {
            MetaData metaData = MetadataSerializer.Instance.Deserialize(metadataText);
            if (metaData == null || metaData.itemData == null)
            {
                return null;
            }
            LoadItem(metaData.itemData);
            return metaData;
        }
        public void StartLoadingMetadata(string metadataText, Action<MetaData> callback = null)
        {
            MetaData metaData = MetadataSerializer.Instance.Deserialize(metadataText);
            if (metaData == null || metaData.itemData == null)
            {
                callback?.Invoke(metaData);
                return;
            }
            LoadItem(metaData.itemData, ()=>callback(metaData));
        }

        public void LoadItem(Item item, Action callback = null)
        {
            string itemAddress = item.itemAddress;
            if (finishedDownloads.Contains(itemAddress))
            {
                prefabDictionary.TryGetValue(itemAddress, out GameObject prefab);
                item.OnAssetDownloaded(prefab);
                callback?.Invoke();
                return;
            }
            if(string.IsNullOrEmpty(itemAddress))
            {
                item.OnAssetDownloaded(null);
                callback?.Invoke();
                return;
            }
            
            startedDownloads.Add(itemAddress);

            LoadAssetByAddress(itemAddress, item.OnAssetDownloaded);
            return;
        }

        public void LoadAssetByAddress(string address, Action<GameObject> callback = null)
        {
            if(asyncOperations.TryGetValue(address, out AsyncOperationHandle<GameObject> asyncOperationHandle))
            {
                asyncOperationHandle.Completed += (operationHandle) => OnAssetLoaded(address, operationHandle, callback);
            }
            else
            {
                try
                {
                    if(!AddressablesUtility.AssetExists(address))
                    {
                        Debug.LogWarning("Asset could not be found. Address: " + address);
                        callback?.Invoke(null);
                        return;
                    }
                    asyncOperationHandle = Addressables.LoadAssetAsync<GameObject>(address);
                    asyncOperations.Add(address, asyncOperationHandle);
                    asyncOperationHandle.Completed += (operationHandle) => OnAssetLoaded(address, operationHandle, callback);
                }
                catch (Exception ex)
                {
                    Debug.LogError(ex.Message);
                }
            }
        }
        private void OnAssetLoaded(string address, AsyncOperationHandle<GameObject> operationHandle, Action<GameObject> callback = null)
        {
            asyncOperations.Remove(address);

            if (operationHandle.Status == AsyncOperationStatus.Failed || operationHandle.Result == null)
            {
                //TODO: UI Popup for connection issues, stop whatever process is running
                onLoadAssetFailed?.Invoke();
                callback?.Invoke(null);
                return;
            }
            GameObject prefab = operationHandle.Result;
            finishedDownloads.Add(address);
            if(!prefabDictionary.ContainsKey(address))
                prefabDictionary.Add(address, prefab);
            callback?.Invoke(prefab);
            if(unloadedItems == 0)
                onAllAssetsLoaded?.Invoke();
        }

        public void InstantiateItem(Item item, Action<GameObject> callback = null)
        {
            if(!item.isDownloaded)
            {
                LoadItem(item, () => InstantiateItem(item, callback));
                return;
            }
            GameObject go = Instantiate(item.prefab, new Vector3(1000f, 1000f, 1000f), Quaternion.identity);

            callback?.Invoke(go);
            return;
        }
        public void InstantiateAsset(string address, Action<GameObject> callback = null)
        {
            if(!finishedDownloads.Contains(address))
            {
                LoadAssetByAddress(address, (prefab) => 
                {
                    GameObject go = Instantiate(prefab, new Vector3(1000f, 1000f, 1000f), Quaternion.identity);
                    callback?.Invoke(go);
                });
                return;
            }
            else if(prefabDictionary.TryGetValue(address, out GameObject prefab))
            {
                GameObject go = Instantiate(prefab, new Vector3(1000f, 1000f, 1000f), Quaternion.identity);
                callback?.Invoke(go);
            }
            else
            {
                Debug.LogError("WTF Something weird happened... " + gameObject.name, gameObject);
            }
        }
        public void WaitForAllDownloads(Action callback = null)
        {
            Debug.Log("Waiting for all assets to download.");
            if(unloadedItems == 0)
                callback?.Invoke();
            else
                onAllAssetsLoaded += callback;
        }

        private void LoadAssets()
        {
            foreach (var itemAddress in assetAddressesToLoad)
            {
                LoadAssetByAddress(itemAddress);
            }
        }

        private void LoadDefaultItems()
        {
            Debug.Log("Loading default robot parts from the resources folder");
            var ressources = Resources.LoadAll<TextAsset>("BaseParts");
            foreach (var ressource in ressources)
            {
                MetaData metaData = StartLoadingMetadata(ressource.text);
                if (metaData != null)
                {
                    defaultItems.Add(metaData.itemData);
                }
                else
                    Debug.LogError("returned metadata is null");
            }
        }
        public RobotPart GetDefaultPart(EquipSlot equipSlot)
        {
            return defaultItems.FirstOrDefault((item) => item is RobotPart && ((RobotPart)item).equipSlot == equipSlot) as RobotPart;
        }
    }
}

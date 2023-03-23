using System;
using UnityEngine;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.AddressableAssets;
using Sirenix.OdinInspector;

namespace Contaquest.Metaverse.Combat2
{
    [CreateAssetMenu(fileName = "Arena Data", menuName = "Combat2/Arena Data")]
    public class ArenaData : ScriptableObject
    {
        [ReadOnly] public string arenaName;
        [TabGroup("Properties")] public int maxUserCount = 2;

        [TabGroup("References")] public AssetReference arenaVisualAsset;
        [TabGroup("References")] public AssetReference arenaDefinitionAsset;
        [System.NonSerialized, HideInInspector] public GameObject arenaVisualPrefab; 
        [System.NonSerialized, HideInInspector] public GameObject arenaDefinitionPrefab;

        public void StartLoadArenaVisual(Action callback)
        {
            var asyncOperation  = Addressables.LoadAssetAsync<GameObject>(arenaVisualAsset);
            asyncOperation.Completed += (obj) => OnVisualLoaded(obj, callback); 
        }
        private void OnVisualLoaded(AsyncOperationHandle<GameObject> obj, Action callback)
        {
            if(obj.Status == AsyncOperationStatus.Succeeded)
            {
                arenaVisualPrefab = obj.Result;
                Debug.Log("Successfully loaded arena visual " + arenaName);
                callback?.Invoke();
            }
        }

        public void StartLoadArenaDefinition(Action callback)
        {
            var asyncOperation  = Addressables.LoadAssetAsync<GameObject>(arenaDefinitionAsset);
            asyncOperation.Completed += (obj) => OnDefinitionLoaded(obj, callback); 
        }
        private void OnDefinitionLoaded(AsyncOperationHandle<GameObject> obj, Action callback)
        {
            if(obj.Status == AsyncOperationStatus.Succeeded)
            {
                arenaDefinitionPrefab = obj.Result;
                Debug.Log("Successfully loaded arena definition " + arenaName);
                callback?.Invoke();
            }
        }

        void OnValidate()
        {
            arenaName = this.name;
        }
    }
}
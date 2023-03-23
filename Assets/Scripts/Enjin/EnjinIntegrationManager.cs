//using System.Collections;
//using System.Collections.Generic;
//using Enjin.SDK.Core;
//using EnjinSDK = Enjin.SDK.Core.Enjin;
//using Enjin.SDK.DataTypes;
//using UnityEngine;
//using System.IO;
//using Sirenix.OdinInspector;
//using System;

//namespace Contaquest.Metaverse.EnjinIntegration
//{
//    public class EnjinIntegrationManager : PersistentGenericSingleton<EnjinIntegrationManager>
//    {
//        public EnjinConfiguration config;
//        [SerializeField] private TextAsset enjinConfigurationTextAsset;

//        public User currentEnjinUser = null;

//        public CryptoItem currentCryptoItem = null;

//        protected override void InitializeSingleton()
//        {
//            base.InitializeSingleton();

//#if UNITY_EDITOR
//            EnjinSDK.IsDebugLogActive = true;
//#endif

//            config = JsonUtility.FromJson<EnjinConfiguration>(enjinConfigurationTextAsset.text);

//            Action callbackAction = StartPlatform;
//            StartCoroutine(GetAccessToken(callbackAction));
//        }

//        private void StartPlatform()
//        {
//            Debug.Log("Started Platform with access token");
//            EnjinSDK.StartPlatformWithToken(config.EnjinPlatformURL, config.AppID, config.AccessToken);
//        }

//        private IEnumerator GetAccessToken(Action callback)
//        {

//#if UNITY_STANDALONE
//            //TODO: Get access token from server and assign it to the enjinconfig
//            //config.AccessToken = 
//#endif
//            callback?.Invoke();
//            yield return null;
//        }

//        [Button]
//        public void SerializeConfiguration()
//        {
//            string serializedString = JsonUtility.ToJson(config);
//            File.WriteAllText(Application.dataPath + $"/Data/EnjinIntegration/EnjinConfiguration.json", serializedString);
//        }

//        public void RegisterUserLogin(string userName)
//        {
//            if (EnjinSDK.LoginState != LoginState.VALID)
//            {
//                return;
//            }

//            currentEnjinUser = EnjinSDK.GetUser(userName);

//            Debug.Log($"[Logined User ID] {currentEnjinUser.id}");
//            Debug.Log($"[Logined User name] {currentEnjinUser.name}");
//        }
//        public void RegisterUserSignUp(string userName)
//        {
//            EnjinSDK.CreatePlayer(userName);
//        }

//        public void GetCryptoItem(string id)
//        {
//            currentCryptoItem = EnjinSDK.GetToken(id);
//        }
//    }
//}

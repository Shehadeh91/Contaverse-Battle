using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using Newtonsoft.Json;
using GraphQlClient.Core;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;
using Contaquest.Metaverse.Data;
using User_Registration.Serialization;

namespace Contaquest.Server
{
    public class UserManager : PersistentGenericSingleton<UserManager>
    {
        [TabGroup("Properties")] [SerializeField] private bool attemptLoginOnStart = true;

        [TabGroup("References")] [SerializeField] private BoolReference isDebugModeEnabled;
        [TabGroup("References")] [SerializeField] private GraphApi graphQlApi;

        [TabGroup("References")] [SerializeField] private FloatReference playerLevel;
        [TabGroup("References")] [SerializeField] private FloatRangeReference chargeAmount;
        [TabGroup("References")] [SerializeField] private FloatRangeReference playerXP;

        [TabGroup("Events")] [SerializeField] private UnityEvent onNoNetwork;
        [TabGroup("Events")] [SerializeField] private UnityEvent onLoggedIn;
        [TabGroup("Events")] [SerializeField] private UnityEvent onFailedLogin;

        private const string UserNameVariableName = "UserName";
        private const string TokenVariableName = "UserToken";
        private const string UserPWVariableName = "UserPW";
        private const string UserEmailVariableName = "UserEmail";

        public string userName;
        public string email;


        public UserAccountData userAccountData;

        private bool isFactionCheckSuccessful = false;

        private void Start()
        {
            if(!attemptLoginOnStart)
                return;
                
            StartLogin();
        }
        public void StartLogin()
        {
            if(graphQlApi == null)
                return;
            if(Application.internetReachability == NetworkReachability.NotReachable)
            {
                onNoNetwork?.Invoke();
                return;
            }
            //Automatically try to login when the user has a username and password stored on the device
            if (!PlayerPrefs.HasKey(TokenVariableName))
            {
                Debug.Log("No Login Token available.");
                onFailedLogin?.Invoke();
                return;
            }

            userName = PlayerPrefs.GetString(UserNameVariableName);
            email = PlayerPrefs.GetString(UserEmailVariableName);

            string token = PlayerPrefs.GetString(TokenVariableName);
            string password = PlayerPrefs.GetString(UserPWVariableName);
            
            if (!string.IsNullOrEmpty(password) && !string.IsNullOrEmpty(email))
            {
                SendLoginRequest(email, password);
            }
            else
            {
                Debug.Log("No Login Token available.");
                onFailedLogin?.Invoke();
            }
        }

        public async void SendRegisterRequest(string userName, string email, string password)
        {
            Debug.Log($"Sending register request with\nUsername: {userName}\nPassword: {password}\nEmail: {email}");

            var output = new { data = new { register = new { token = "", expiresIn = long.MaxValue } }, errors = new GraphqlError[0] };
            var args = new { email, password, userName };

            var result = await graphQlApi.GetMutationResult("RegisterMutation", args, output);

            if (result == null || result.data == null || result.data.register == null || string.IsNullOrEmpty(result.data.register.token))
            {
                string errorMessage = "Failed to register new User:";
                foreach (var error in result.errors)
                {
                    if(error.message.Contains("The specified user name"))
                        errorMessage += "\nThe specified user name already exists for this app.";
                    else
                        errorMessage += "\n" + error.message;
                }
                ErrorManager.Instance.ShowError(errorMessage);
                return;
            }

            string token = result.data.register.token;
            SaveUserInfo(userName, token, password, email);
            SetToken(token);
        }

        public async void SendLoginRequest(string email, string password)
        {
            Debug.Log($"Sending login request with\nEmail: {email}\nPassword: {password}");

            var output = new { data = new { login = new { token = "", expiresIn = long.MaxValue }}, errors = new GraphqlError[0]};
            var args = new { email, password };

            var result = await graphQlApi.GetQueryResult("LoginQuery", args, output);

            if(result == null || result.data == null || result.data.login == null || string.IsNullOrEmpty(result.data.login.token))
            {
                string errorMessage = "Failed to log in User:";
                foreach (var error in result.errors)
                    errorMessage += "\n" + error.message;
                ErrorManager.Instance.ShowError(errorMessage);
                onFailedLogin?.Invoke();
                return;
            }
            
            string token = result.data.login.token;
            SaveUserInfo("", token, password, email);
            SetToken(token);
        }

        public async void SendPasswordResetRequest(string email, Action<string> callback)
        {
            Debug.Log($"SendPasswordResetRequest with\nEmail: {email}");

            var output = new { data = new { login = new { token = "", expiresIn = long.MaxValue }}, errors = new GraphqlError[0]};
            var args = new { email };

            var result = await graphQlApi.GetQueryResult("ResetQuery", args, output);

            if(result == null || result.data == null || result.data.login == null || string.IsNullOrEmpty(result.data.login.token))
            {
                string errorMessage = "Failed to reset password for email: " + email;
                foreach (var error in result.errors)
                    errorMessage += "\n" + error.message;
                ErrorManager.Instance.ShowError(errorMessage);
                onFailedLogin?.Invoke();
                return;
            }
        }

        private void SaveUserInfo(string userName = "", string token = "", string password = "", string email = "")
        {
            this.userName = userName;
            this.email = email;

            if(!string.IsNullOrEmpty(userName))
                PlayerPrefs.SetString(UserNameVariableName, userName);
            if(!string.IsNullOrEmpty(email))
                PlayerPrefs.SetString(UserEmailVariableName, email);
            if(!string.IsNullOrEmpty(token))
                PlayerPrefs.SetString(TokenVariableName, token);
            if(!string.IsNullOrEmpty(password))
                PlayerPrefs.SetString(UserPWVariableName, password);

            PlayerPrefs.Save();
        }
        private void SetToken(string token)
        {
            graphQlApi.SetAuthToken(token);
            CompleteLogin();
        }

        public async void CompleteLogin()
        {
            await GetUserAccountDataAsync();

            if (userAccountData == null || userAccountData.UserName == null)
            {
                onFailedLogin?.Invoke();
                return;
            }

            onLoggedIn?.Invoke();
        }

        public bool HasFaction()
        {
            if (userAccountData == null || userAccountData.Faction == Faction.NoFaction)
            {
                return false;
            }

            return true;
        }
        public Faction GetFaction()
        {
            if(userAccountData == null)
            {
                if(isDebugModeEnabled.Value)
                    return Faction.Droidz;
                return Faction.NoFaction;
            }
            return userAccountData.Faction;
        }
        public string GetUserName()
        {
            if(userAccountData == null)
            {
                if(string.IsNullOrEmpty(userName))
                    return "No Username";
                return userName;
            }
            return userAccountData.UserName;
        }

        public async void TrySetUserFaction(int factionIndex, Action callback = null)
        {
            if (factionIndex == 0)
                return;

            var output = new { data = new { update = new { success = true } }, errors = new GraphqlError[0] };
            var args = new { faction = factionIndex };

            var result = await graphQlApi.GetMutationResult("SetUserFaction", args, output);

            if(result == null || result.data == null || result.data.update == null)
            {
                string errorMessage = "Failed to set User's Faction:";
                foreach (var error in result.errors)
                    errorMessage += "\n" + error.message;
                ErrorManager.Instance.ShowError(errorMessage);
                return;
            }

            userAccountData.Faction = (Faction)factionIndex;
            callback?.Invoke();
        }

        public bool IsWalletLinked()
        {
            return userAccountData?.EnjinData?.IsWalletLinked ?? false;
        }

        public async void TryLinkWallet(WalletConnectUI walletConnectUI, Action callback = null)
        {
            if(userAccountData == null)
            {
                await GetUserAccountDataAsync();
            }
            if (userAccountData.EnjinData.IsWalletLinked)
            {
                Debug.Log("Wallet already Connected");
                callback?.Invoke();
                return;
            }

            string code = userAccountData.EnjinData.LinkingInfo?.Code;
            string URL = userAccountData.EnjinData.LinkingInfo?.Qr?.ToString();
            walletConnectUI.UpdateWalletUI(code, URL);

            while (true)
            {
                #if UNITY_EDITOR
                if(UnityEditor.EditorApplication.isPlaying == false)
                    return;
                #endif
                
                await Task.Delay(5000);

                var output = new { data = new { refreshEnjinLink = new UserAccountData() }, errors = new GraphqlError[0] };

                var result = await graphQlApi.GetMutationResult("RefreshLink", null, output);

                if(result == null || result.data == null || result.data.refreshEnjinLink == null)
                {
                    string errorMessage = "Failed to get check wallet linking progress:";
                    foreach (var error in result.errors)
                    {
                        if(error.message.Contains("already linked"))
                        {
                            Debug.Log("Wallet already Linked");
                            callback?.Invoke();
                            return;
                        }
                        errorMessage += "\n" + error.message;
                    }
                    ErrorManager.Instance.ShowError(errorMessage);
                    continue;
                }

                // Check if it's connected
                bool isConnected = result.data.refreshEnjinLink.EnjinData.IsWalletLinked;
                if (isConnected)
                {
                    Debug.Log("Connected successfully to wallet");
                    callback?.Invoke();
                    return;
                }
            }
        }

        public async Task GetUserAccountDataAsync()
        {
            if(isDebugModeEnabled.Value)
            {
                if(userAccountData == null)
                    userAccountData = new UserAccountData("Debug User", Faction.Droidz, null);
                UpdateVariablesFromUserData();
                return;
            }
            
            var output = new { data = new { getUserInfo = new UserAccountData() }, errors = new GraphqlError[0] };

            var result = await graphQlApi.GetQueryResult("GetAllUserInfoQuery", null, output);

            if(result == null || result.data == null || result.data.getUserInfo == null)
            {
                string errorMessage = "Failed to get User Data:";
                foreach (var error in result.errors)
                    errorMessage += "\n" + error.message;
                ErrorManager.Instance.ShowError(errorMessage);
                return;
            }

            userAccountData = result.data.getUserInfo;

            UpdateVariablesFromUserData();
            return;
        }

        public void UpdateVariablesFromUserData()
        {
            playerXP.Value.Value = (float)userAccountData.XP;
            playerLevel.Value = (float)userAccountData.Level;
            if(userAccountData.ChargeData != null)
            {
                chargeAmount.Value.Value = (float) userAccountData.ChargeData.chargeAmount;
                Debug.Log("Setting charge to " + chargeAmount.Value.Value);
            }
        }

        public async Task<WalletItem[]> GetWalletItems(Action callback = null)
        {
            if(isDebugModeEnabled.Value)
                return null;

            var output = new { data = new { walletContents = new WalletContent[1]}, errors = new GraphqlError[0] };

            var result = await graphQlApi.GetQueryResult("GetWalletContents", null, output);

            if(result == null || result.data == null || result.data.walletContents == null)
            {
                string errorMessage = "Failed to get Wallet Contents:";
                foreach (var error in result.errors)
                    errorMessage += "\n" + error.message;
                ErrorManager.Instance.ShowError(errorMessage);
                return null;
            }
            // List<WalletItem> walletItems = new List<WalletItem>();
            // foreach(var walletContent in result.data.walletContents)
            // {
            //     walletItems.Add(new WalletItem(walletContent.token.itemURI, walletContent.token.id, walletContent.index, walletContent.value));
            // }
            return result.data.walletContents.Select((content) => new WalletItem(content.token.itemURI, content.token.id, content.index, content.value)).ToArray();
        }

        public async Task<bool> StartCharging()
        {
            if(isDebugModeEnabled.Value)
            {
                return true;
            }
            var output = new { data = new { startCharging = new {chargeAmount = 1} }, errors = new GraphqlError[0] };

            var result = await graphQlApi.GetMutationResult("StartCharging", null, output);

            if(result == null || result.data == null || result.data.startCharging == null)
            {
                string errorMessage = "Failed to get Start Charging:";
                foreach (var error in result.errors)
                    errorMessage += "\n" + error.message;
                ErrorManager.Instance.ShowError(errorMessage);
                return false;
            }
            userAccountData.ChargeData.chargeAmount = result.data.startCharging.chargeAmount;
            UpdateVariablesFromUserData();
            
            return true;
        }
        public async Task<bool> StopCharging()
        {
            if(isDebugModeEnabled.Value)
            {
                chargeAmount.Value.Value = 5;
                return true;
            }
            var output = new { data = new { stopCharging = new {chargeAmount = 1} }, errors = new GraphqlError[0] };

            var result = await graphQlApi.GetMutationResult("StopCharging", null, output);

            if(result == null || result.data == null || result.data.stopCharging == null)
            {
                string errorMessage = "Failed to get Stop Charging:";
                foreach (var error in result.errors)
                    errorMessage += "\n" + error.message;
                ErrorManager.Instance.ShowError(errorMessage);
                return false;
            }
            userAccountData.ChargeData.chargeAmount = result.data.stopCharging.chargeAmount;
            UpdateVariablesFromUserData();

            return true;
        }
        // TODO: proper graphql query
        public async Task<bool> UseBattery()
        {
            if(isDebugModeEnabled.Value)
            {
                chargeAmount.Value.Value = 5;
                return true;
            }
            var output = new { data = new { startCharging = new {chargeAmount = 1} }, errors = new GraphqlError[0] };

            var result = await graphQlApi.GetMutationResult("StartCharging", null, output);

            if(result == null || result.data == null || result.data.startCharging == null)
            {
                string errorMessage = "Failed to use Battery:";
                foreach (var error in result.errors)
                    errorMessage += "\n" + error.message;
                ErrorManager.Instance.ShowError(errorMessage);
                return false;
            }
            userAccountData.ChargeData.chargeAmount = result.data.startCharging.chargeAmount;
            UpdateVariablesFromUserData();

            return true;
        }

        public async void SetPlayerParts(RobotData robotData)
        {
            var output = new { data = new { setPlayerParts = new UserParts(null)}, errors = new GraphqlError[0] };
            var args = new { setPlayerParts = new UserParts(robotData) };

            var result = await graphQlApi.GetMutationResult("SetPlayerParts", args, output);

            if(result == null || result.data == null || result.data.setPlayerParts == null)
            {
                string errorMessage = "Failed to set Player Parts:";
                foreach (var error in result.errors)
                    errorMessage += "\n" + error.message;
                ErrorManager.Instance.ShowError(errorMessage);
                return;
            }
        }
    }
}
using System;
using System.Collections;
using GraphQlClient.Core;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using Newtonsoft.Json;
using UnityEngine.Networking;

namespace Contaquest.Server
{
    public class UserRegistrationUI : MonoBehaviour
    {
        [SerializeField] private TMP_InputField userNameInputField;
        [SerializeField] private TMP_InputField passwordInputField;
        [SerializeField] private TMP_InputField emailInputField;

        public void TryRegister()
        {
            string userName = userNameInputField.text;
            string email = emailInputField.text;
            string password = passwordInputField.text;

            UserManager.Instance.SendRegisterRequest(userName, email, password);
        }

        public void TryLogin()
        {
            string email = emailInputField.text;
            string password = passwordInputField.text;

            UserManager.Instance.SendLoginRequest(email, password);
        }

        public void LogOut()
        {
            PlayerPrefs.DeleteAll();
            PlayerPrefs.Save();
            GenericSingletonManager.OnDestroyAllSingletons();
        }

        public void ForgetPassword()
        {

        }
    }
}
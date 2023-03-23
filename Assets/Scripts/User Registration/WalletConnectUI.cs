using System;
using Contaquest.Metaverse.Data;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using Sirenix.OdinInspector;

namespace Contaquest.Server
{
    public class WalletConnectUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI codeText;
        [SerializeField] private StringUnityEvent onUpdateWallet;
        [SerializeField] private UnityEvent onWalletLinked, onFoundersTokenMissing, onFinished;
        [SerializeField] private string[] foundersTokenNftIds;
        private bool isWalletLoaded = false;

        public void TryLinkWallet()
        {
            if(UserManager.Instance.IsWalletLinked())
            {
                OnWalletLinked();
            }
            else
            {
                UserManager.Instance.TryLinkWallet(this, OnWalletLinked);
            }
        }
        [Button]
        public void OnWalletLinked()
        {
            onWalletLinked?.Invoke();
            WalletController.Instance.UpdateWalletContents(false, WaitForWalletItemLoaded);
        }

        private void WaitForWalletItemLoaded()
        {
            // Debug.Log("WaitForWalletItemLoaded");
            WalletController.Instance.LoadItems(CheckFoundersTokens);
        }

        public void CheckFoundersTokens()
        {
            if(isWalletLoaded)
                return;
            isWalletLoaded = true;
            
            bool hasFoundersToken = false;
            foreach (var walletItem in WalletController.Instance.walletItems)
            {
                foreach (string nftId in foundersTokenNftIds)
                {
                    if(walletItem.Value.NFTId == nftId)
                    {
                        hasFoundersToken = true;
                        break;
                    }
                }
            }
            
            if(!hasFoundersToken)
            {
                onFoundersTokenMissing?.Invoke();
                return;
            }
            else
            {
                OnFinished();
            }
        }

        [Button]
        private void OnFinished()
        {
            onFinished?.Invoke();
        }

        public void UpdateWalletUI(string code, string URL)
        {
            codeText.text = code;
            onUpdateWallet?.Invoke(URL);
        }
    }
}

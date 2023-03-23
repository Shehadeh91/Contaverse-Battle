using Contaquest.Metaverse.Data;
using UnityEngine;
using UnityEngine.Events;

public class WalletCheck : MonoBehaviour
{
    [SerializeField] private UnityEvent onWalletLoaded;
    private bool isWalletLoaded = false;
    private void Start()
    {
        WalletController.Instance.UpdateWalletContents(false, WaitForWalletItemLoaded);
    }

    private void WaitForWalletItemLoaded()
    {
        Debug.Log("WaitForWalletItemLoaded");
        WalletController.Instance.LoadItems(OnWalletLoaded);
    }

    private void OnWalletLoaded()
    {
        if(isWalletLoaded)
            return;
        isWalletLoaded = true;
        Debug.Log("Wallet Loaded");
        onWalletLoaded?.Invoke();
    }
}

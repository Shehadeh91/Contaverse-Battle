using UnityEngine;
using UnityEngine.Events;
using Contaquest.Metaverse.Data;
using Contaquest.Server;
using Sirenix.OdinInspector;
public class DebugUserLogin : MonoBehaviour
{
    [SerializeField] [ListDrawerSettings(AddCopiesLastElement = true)] private DebugWalletItem[] debugWalletItems;
    [SerializeField] private BoolReference isDebugModeEnabled;
    [SerializeField] private UnityEvent onFinished;
    bool isFinished;
    public void EnableDebugUser()
    {
        Debug.Log("Entering Debug Mode");
        isDebugModeEnabled.Value = true;
        WalletController.Instance.AddDebugWalletItems(debugWalletItems);
        UserManager.Instance.userAccountData = new UserAccountData("Debug User", Faction.Droidz, null);
        WalletController.Instance.LoadItems(OnFinished);
    }

    public void OnFinished()
    {
        if(isFinished)
            return;
        isFinished = true;
        onFinished?.Invoke();
    }
}

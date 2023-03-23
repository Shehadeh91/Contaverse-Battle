using UnityEngine;
using TMPro;
using Contaquest.Server;
public class SetUserName : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private bool setOnStart = true;

    void Start()
    {
        if(setOnStart)
            UpdateUserName();
    }
    public void UpdateUserName()
    {
        if(UserManager.Instance.userAccountData != null)
            text.text = UserManager.Instance.userAccountData.UserName;
        else
            text.text = "Failed to get Username";
    }
}
using UnityEngine;
using Contaquest.Server;

public class FactionLogoSelector : MonoBehaviour
{
    [SerializeField] private bool onStart = true;
    [SerializeField] private bool useUserAccount = true;
    [SerializeField] private GameObject[] factionObjects;
    private void Start()
    {
        if(onStart == true)
        {
            if(useUserAccount)
                SelectUserFactionLogo();
            else
                SelectRandomFactionLogo();
        }
    }
    public void SelectUserFactionLogo()
    {
        if(UserManager.Instance.userAccountData != null)
            SelectFactionLogo(UserManager.Instance.userAccountData.Faction);
        else
            SelectRandomFactionLogo();
    }
    public void SelectRandomFactionLogo()
    {
        SelectFactionLogo((Faction)Random.Range(1, 3));
    }

    public void SelectFactionLogo(Faction faction)
    {
        int factionIndex = (int) faction;
        for (var i = 0; i < factionObjects.Length; i++)
        {
            if(factionObjects[i] != null)
                factionObjects[i].SetActive(factionIndex == i);
        }
    }
}
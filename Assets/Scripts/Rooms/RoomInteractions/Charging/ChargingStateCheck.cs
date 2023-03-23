using UnityEngine;
using Sirenix.OdinInspector;
using Contaquest.Server;
using UnityEngine.Events;

public class ChargingStateCheck : MonoBehaviour
{
    [TabGroup("References")] [SerializeField] private IntVariable batteryAmount;
    [TabGroup("References")] [SerializeField] private GameObject[] batteries;
    [TabGroup("Events")] [SerializeField] private UnityEvent onIsCharging;
    [TabGroup("State")] [ShowInInspector] private ChargeData chargeData;

    private void Start()
    {
        chargeData = UserManager.Instance.userAccountData.ChargeData;
        if(chargeData == null)
            return;
        if(chargeData.isCharging)
        {
            onIsCharging?.Invoke();
        }
    }

    void OnEnable()
    {
        batteryAmount.OnChanged += UpdateBatteryCount;
    }

    public void UpdateBatteryCount()
    {
        for (int i = 0; i < batteries.Length; i++)
        {
            bool isBelowBatteryAmount = i >= batteryAmount.Value;
            batteries[i].SetActive(isBelowBatteryAmount);
        }
    }
}
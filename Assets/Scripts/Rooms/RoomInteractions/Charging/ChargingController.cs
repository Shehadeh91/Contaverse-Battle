using UnityEngine;
using Sirenix.OdinInspector;
using UnityEngine.Events;
using Contaquest.Metaverse.Rooms;
using Contaquest.Mobile.Input;
using Contaquest.Server;

public class ChargingController : MonoBehaviour
{
    [TabGroup("References")] [SerializeField] private ButtonInteractable buttonInteractable;
    [TabGroup("References")] [SerializeField] private FloatRangeVariable chargeAmount;
    [TabGroup("References")] [SerializeField] private Transform targetTransform, interactionTransform;
    [TabGroup("Events")] [SerializeField] private UnityEvent onChamberEntered, onChamberExited;
    [TabGroup("Events")] [SerializeField] private UnityEvent onChargingStarted, onChargingStopped, onStartGameAtCharger;
    [TabGroup("Events")] [SerializeField] private UnityEvent onActionFailed;
    [TabGroup("State")] [ShowInInspector, ReadOnly] private bool isPowered = false;
    [TabGroup("State")] [ShowInInspector, ReadOnly] private bool isCharging = false;
    [TabGroup("State")] [ShowInInspector, ReadOnly] private bool isUsingBattery = false;
    [TabGroup("State")] [ShowInInspector, ReadOnly] private bool isInChamber, isRobotMoving = false;

    void Start()
    {
        onActionFailed.AddListener(()=> Debug.Log("Action Failed"));
    }
    public void SetEnergySourceCable()
    {
        isPowered = true;
        isUsingBattery = false;
    }
    public void SetEnergySourceBattery()
    {
        isPowered = true;
        isUsingBattery = true;
    }
    public void ResetEnergySource()
    {
        isPowered = false;
    }
    public void EnterChamber()
    {
        if(isInChamber || isRobotMoving)
            return;

        if(!isPowered)
        {
            onActionFailed?.Invoke();
            return;
        }
        isRobotMoving = true;
        RoomMovementController.Instance.StartMoveToTransform(targetTransform, OnChamberEntered);
    }

    public void OnChamberEntered()
    {
        isRobotMoving = false;

        Debug.Log("robot Entered Chamber");
        isInChamber = true;
        onChamberEntered?.Invoke();

        StartCharging();
    }
    public void ExitChamber()
    {
        // Debug.Log("test");
        if(!isInChamber || isRobotMoving)
        {
            return;
        }
        StopCharging();
        onChamberExited?.Invoke();

        RoomMovementController.Instance.StartMoveToTransform(interactionTransform, OnChamberExited);
    }
    public void OnChamberExited()
    {
        isRobotMoving = false;

        Debug.Log("robot Exited Chamber");
        isInChamber = false;
    }
    public void StartCharging()
    {
        if(!isPowered || !isInChamber)
        {
            onActionFailed?.Invoke();
            return;
        }

        if(isUsingBattery)
        {
            ChargeWithBattery();
        }
        else
        {
            ChargeWithCable();
        }
    }

    private async void ChargeWithBattery()
    {
        if(isCharging)
            return;
        isCharging = true;

        onChargingStarted?.Invoke();
        await UserManager.Instance.UseBattery();

        chargeAmount.Value.SetProgress(1);
        chargeAmount.Save();
    }

    private async void ChargeWithCable()
    {
        if(isCharging)
            return;
        isCharging = true;
        bool startedSuccessfully = await UserManager.Instance.StartCharging();

        if(startedSuccessfully)
            onChargingStarted?.Invoke();
        else
            onActionFailed?.Invoke();
    }

    public void StartGameAtCharger()
    {
        isPowered = true;
        isUsingBattery = false;
        isInChamber = false;
        isRobotMoving = false;

        onStartGameAtCharger?.Invoke();
        RoomMovementController.Instance.SetPosition(targetTransform.position);
        RoomMovementController.Instance.SetRotation(Quaternion.identity);
        buttonInteractable.ActivateButton();
    }

    public async void StopCharging()
    {
        isCharging = false;

        bool stoppedSuccessfully = await UserManager.Instance.StopCharging();

        if(stoppedSuccessfully)
            onChargingStopped?.Invoke();
        else
            onActionFailed?.Invoke();

        Debug.Log("Setting charge to 5");
        chargeAmount.Value.Value = 5;
    }
}
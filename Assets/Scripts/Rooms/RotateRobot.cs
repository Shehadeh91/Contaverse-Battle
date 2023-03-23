using Contaquest.Metaverse.Rooms;
using UnityEngine;

public class RotateRobot : MonoBehaviour
{
    [SerializeField] private Vector3 axis;

    public void Rotate(float amount)
    {
        RoomMovementController.Instance.RotateRobot(axis * amount);
    }
}

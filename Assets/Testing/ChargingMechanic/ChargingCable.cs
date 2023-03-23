using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChargingCable : MonoBehaviour
{
    [SerializeField] private Transform movableTransform, targetTransform;
    private Vector3 initialMovableTransformPos, diffPos;

    private float movementRate;

    #region MonobehaviourMethods
    private void Start()
    {
        Init();
    }

    private void Update()
    {
        ExecuteZMovement();
    }
    #endregion

    #region ChargingCableMethods
    private void Init()
    {
        initialMovableTransformPos = movableTransform.position;
        CalculateMovementRate();
    }

    private void CalculateMovementRate()
    {
        float xDiff = Mathf.Abs(movableTransform.position.x - targetTransform.position.x);
        float yDiff = Mathf.Abs(movableTransform.position.y - targetTransform.position.y);
        float zDiff = Mathf.Abs(movableTransform.position.z - targetTransform.position.z);
        movementRate = zDiff / (xDiff + yDiff);
    }

    private void ExecuteZMovement()
    {
        diffPos = initialMovableTransformPos - movableTransform.position;

        CalculateZMovement();
    }

    private void CalculateZMovement()
    {
        float zPos = (Mathf.Abs(diffPos.x) + Mathf.Abs(diffPos.y)) * movementRate;

        movableTransform.position = new Vector3(movableTransform.position.x, movableTransform.position.y, 
            initialMovableTransformPos.z + zPos);
    }
    #endregion
}

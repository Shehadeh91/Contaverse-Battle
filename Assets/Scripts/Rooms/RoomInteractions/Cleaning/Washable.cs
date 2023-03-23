using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Washable : MonoBehaviour
{
    [SerializeField] Transform parentTransform;

    public Transform GetParentTransform()
    {
        if (parentTransform != null)
            return parentTransform;

        return transform;
    }
}

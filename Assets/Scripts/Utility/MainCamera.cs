using System;
using UnityEngine;

[DefaultExecutionOrder(-102)][RequireComponent(typeof(Camera))]
public class MainCamera : MonoBehaviour
{
    private void Awake()
    {
        Camera thisCamera = GetComponent<Camera>();

        if(GetComponent<Camera>() != null)
            ReferencesManager.Instance.MainCamera = thisCamera;
    }
}

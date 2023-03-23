using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReferencesManager : GenericSingleton<ReferencesManager>
{
    [SerializeField]
    private Camera mainCamera;

    public Camera MainCamera
    {
        get
        {
            if (mainCamera == null)
            {
                Debug.LogError("No Main Camera found in scene, please add the MainCamera script to your camera or assign one manually", this.gameObject);
                mainCamera = Camera.main;
            }
            return mainCamera;
        }
        set
        {
            mainCamera = value;
        }
    }
}

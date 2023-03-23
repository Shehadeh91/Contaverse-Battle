using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalGameSettings : MonoBehaviour
{
    public int MaxFramesPerSecond=30;

    private void Awake()
    {
        Application.targetFrameRate = MaxFramesPerSecond;//this limits the FPS and processor usage and therefore battery lifetime, 30FPS is mobile standard
       
    }


}

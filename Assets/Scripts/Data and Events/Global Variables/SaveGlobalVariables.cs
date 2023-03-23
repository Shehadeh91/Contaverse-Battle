using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveGlobalVariables : MonoBehaviour
{
    [SerializeField]
    private GlobalVariable[] variables;

    public void SaveVariables()
    {
        foreach (GlobalVariable variable in variables)
        {
            variable?.Save();
        }
    }
}
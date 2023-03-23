using UnityEngine;
using TMPro;
using System.Collections.Generic;
using Sirenix.OdinInspector;

public class SetRandomUsername : MonoBehaviour
{    
    [SerializeField] private StringVariable stringVariable;
    [SerializeField] private List<string> prefix;
    [SerializeField] private List<string> suffix;

    [SerializeField] private bool setOnStart = true;

    void Start()
    {
        if(setOnStart)
            UpdateUserName();
    }
    [Button]
    public void UpdateUserName()
    {
        string name = "";

        if(Random.Range(0, 100) > 10)
            name += prefix[Random.Range(0, prefix.Count)];

        name += suffix[Random.Range(0, suffix.Count)];

        if(Random.Range(0, 100) > 60)
        {
            name += (int)Random.Range(0, 10);
        }
        if(Random.Range(0, 100) > 60)
        {
            name += (int)Random.Range(0, 10);
        }
        stringVariable.Value = name;
    }
}
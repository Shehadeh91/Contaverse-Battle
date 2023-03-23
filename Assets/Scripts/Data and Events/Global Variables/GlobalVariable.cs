using Sirenix.OdinInspector;
using UnityEngine;

public class GlobalVariable : ScriptableObject
{
    [ShowIf("isSaveable")]
    public string variableName = "unassigned";

    public delegate void Event();

    [SerializeField]
    protected bool isSaveable = false;

    [System.NonSerialized]
    protected bool isInitialized = false;

    protected virtual void Awake()
    {
        InitializeVariable();
    }
    protected virtual void OnEnable()
    {
        InitializeVariable();
        if(isSaveable)
        {
            SaveManager.Instance.RegisterSaveableVariable(this);
        }
    }
    protected virtual void OnDisable()
    {
        InitializeVariable();
    }

    public virtual void Save()
    {
        SaveManager.Instance.VariableSaveRequested();
    }
    public virtual void InitializeVariable()
    {
        if(isInitialized)
            return;
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Sirenix.OdinInspector;
using Contaquest.Metaverse.Robot;

public class RobotMaintenance : MonoBehaviour
{
    public RobotDefinition robotDefinition;

    [TabGroup("Cleaning")] [SerializeField] private FloatRangeVariable dirtiness;
    [TabGroup("Cleaning")] public UnityEvent onFinishedCleaning;
    [TabGroup("Cleaning")] private bool alreadyCleaned = false;
    [TabGroup("Cleaning")] [SerializeField] private float materialMaxDirtiness;
    [TabGroup("Cleaning")] [SerializeField] private bool resetDirtinessOnStart = false;

    [TabGroup("Charging")] [SerializeField] private FloatRangeReference robotCharge;
    [TabGroup("Charging")] public UnityEvent onFinishedCharging;


    private void OnEnable()
    {
        SubscribeToVariableChange();
    }
    [Button]
    private void SubscribeToVariableChange()
    {
        // Debug.Log($"Dirtiness ID: {dirtiness.Value.ID}");
        dirtiness.Value.OnChanged += OnDirtinessChanged;
    }

    private void OnDisable()
    {
        dirtiness.Value.OnChanged -= OnDirtinessChanged;
    }

    public void Initialize(RobotDefinition robotDefinition)
    {
        this.robotDefinition = robotDefinition;
        if (resetDirtinessOnStart)
            dirtiness.Value.Value = 0;
        OnDirtinessChanged();
    }

    public void OnDirtinessChanged()
    {
        // Debug.Log($"dirtiness {dirtiness.Value.ID} changed to {dirtiness.Value.Value}");
        if (dirtiness.Value.Value <= 5)
        {
            if (!alreadyCleaned)
            {
                alreadyCleaned = true;
                onFinishedCleaning.Invoke();
                dirtiness.SetValueSilent(0);
            }
        }
        else
        {
            alreadyCleaned = false;
        }
        float materialDirtiness = Mathf.Lerp(0, materialMaxDirtiness, dirtiness.Value.Value * 0.01f);
        robotDefinition?.SetDirtiness(materialDirtiness);
    }
    [Button]
    public void Test()
    {
        // Debug.Log(dirtiness.Value.Value);
    }
    [Button]
    public void Test2()
    {
        dirtiness.Value.SetValueClamped(UnityEngine.Random.Range(0, 100f));
    }
}
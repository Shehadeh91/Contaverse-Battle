using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "Variable/Set Variable Event")]
public class SetVariableOnEvent : GameEvent
{
    [Header("Set Bool Variable")]
    [SerializeField]
    private BoolVariable boolVariableTarget;
    [SerializeField]
    private BoolReference boolVariableSource;
    [Header("Set Integer Variable")]
    [SerializeField]
    private IntVariable intVariableTarget;
    [SerializeField]
    private IntReference intVariableSource;
    [Header("Set Float Variable")]
    [SerializeField]
    private FloatVariable floatVariableTarget;
    [SerializeField]
    private FloatReference floatVariableSource;

    public override void Raise()
    {
        if(boolVariableTarget != null)
            boolVariableTarget.Value = boolVariableSource.Value;
        if(intVariableTarget != null)
            intVariableTarget.Value = intVariableSource.Value;
        if(floatVariableTarget != null)
            floatVariableTarget.Value = floatVariableSource.Value;
        base.Raise();
    }
}
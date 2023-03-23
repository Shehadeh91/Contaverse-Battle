using System.Collections;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

public class FloatVariableLerp : MonoBehaviour
{
    [SerializeField] private float interpolationSpeed = 0.1f;

    [SerializeField] private FloatVariable sourceFloatVariable;

    [SerializeField] private FloatVariable targetFloatVariable;

    //[SerializeField] private UnityEvent onTargetValueChanged;

    private void OnEnable()
    {
        sourceFloatVariable.OnChanged += UpdateTargetVariable;
    }
    private void OnDisable()
    {
        sourceFloatVariable.OnChanged -= UpdateTargetVariable;
    }

    private void UpdateTargetVariable()
    {
        targetFloatVariable.OnChange();
        StopAllCoroutines();
        StartCoroutine(UpdateTargetVariableCoroutine());
    }

    private IEnumerator UpdateTargetVariableCoroutine()
    {
        bool isUpdating = true;
        while (isUpdating)
        {
            targetFloatVariable.Value = Mathf.Lerp(targetFloatVariable.Value, sourceFloatVariable.Value, interpolationSpeed);
            if (Mathf.Abs(targetFloatVariable.Value - sourceFloatVariable.Value) < 0.005f)
            {
                targetFloatVariable.Value = sourceFloatVariable.Value;
                isUpdating = false;
            }
            yield return null;
        }
    }
}

using System.Collections;
using UnityEngine;

public class ChangeFloatVariable : MonoBehaviour
{
    [SerializeField] private FloatVariable floatVariable;

    public void Increase(float amount)
    {
        floatVariable.Value += amount;
    }
    public void Decrease(float amount)
    {
        floatVariable.Value -= amount;
    }
    public void Set(float amount)
    {
        floatVariable.Value = amount;
    }
}

using UnityEngine;

public class SetBoolVariable : MonoBehaviour
{
    [SerializeField]
    private BoolVariable variable;
    public void SetBoolVariableTo(bool value)
    {
        variable.Value = value;
    }
}
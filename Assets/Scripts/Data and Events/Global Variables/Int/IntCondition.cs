using UnityEngine;

[System.Serializable]
public class IntCondition
{
    public IntReference int1;
    public IntReference int2;
    [SerializeField]
    private NumberComparison comparisonMode;

    public bool Check()
    {
        switch (comparisonMode)
        {
            case NumberComparison.Equal:
                return int1.Value == int2.Value;
            case NumberComparison.NotEqual:
                return int1.Value != int2.Value;
            case NumberComparison.Greater:
                return int1.Value > int2.Value;
            case NumberComparison.Less:
                return int1.Value < int2.Value;
            case NumberComparison.GEqual:
                return int1.Value >= int2.Value;
            case NumberComparison.LEqual:
                return int1.Value <= int2.Value;
            default:
                return false;
        }
    }
}
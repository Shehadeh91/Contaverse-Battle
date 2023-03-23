using UnityEngine;

[System.Serializable]
public class BoolCondition
{
    public BoolReference bool1;
    public BoolReference bool2;
    [SerializeField]
    private BoolComparison comparisonMode;

    public bool Check()
    {
        switch (comparisonMode)
        {
            case BoolComparison.Equal:
                return bool1.Value == bool2.Value;
            case BoolComparison.NotEqual:
                return bool1.Value != bool2.Value;
            case BoolComparison.And:
                return bool1.Value && bool2.Value;
            case BoolComparison.Or:
                return bool1.Value || bool2.Value;
            case BoolComparison.True:
                return bool1.Value && bool2.Value;
            case BoolComparison.False:
                return (!bool1.Value) && (!bool2.Value);
            default:
                return false;
        }
    }
}
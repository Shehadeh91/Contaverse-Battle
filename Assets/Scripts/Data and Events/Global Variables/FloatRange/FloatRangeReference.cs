using UnityEngine;

[System.Serializable]
public class FloatRangeReference
{
    public bool useConstant = true;
    public FloatRangeValue constantValue;
    public FloatRangeVariable variable;

    public FloatRangeValue Value
    {
        get
        {
            if(useConstant)
                return constantValue;
            else
            {
                if(variable == null)
                {
                    Debug.LogError("The Variable was not set.");
                    useConstant = true;
                    return constantValue;
                }
                else
                {
                    return variable.Value;
                }
            }
        }
        set
        {
            if(useConstant)
                constantValue = value;
            else
            {
                if(variable == null)
                {
                    Debug.LogError("The Variable was not set.");
                    useConstant = true;
                }
                else
                {
                    variable.Value = value;
                }
            }
        }
    }
}
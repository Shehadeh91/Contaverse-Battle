using UnityEngine;
using System;

[System.Serializable]
public class FloatRangeValue : RangeValue
{
    #region Constructors
    public FloatRangeValue(float minValue, float maxValue, float defaultValue)
    {
        this.minValue = minValue;
        this.maxValue = maxValue;
        this.value = defaultValue;
        OnChanged?.Invoke();
    }
    public FloatRangeValue(float minValue, float maxValue)
    {
        this.minValue = minValue;
        this.maxValue = maxValue;
        this.value = minValue;
        OnChanged?.Invoke();
    }

    #endregion

    #region Values

    [SerializeField]
    private float minValue = 0;
    [SerializeField]
    private float maxValue = 1;
    [SerializeField]
    private float value = 1;

    public float MinValue
    {
        get
        {
            return minValue;
        }
        set
        {
            if(minValue != value)
            {
                minValue = value;
                OnChanged?.Invoke();
            }
        }
    }
    public float MaxValue
    {
        get
        {
            return maxValue;
        }
        set
        {
            if(maxValue != value)
            {
                maxValue = value;
                OnChanged?.Invoke();
            }
        }
    }
    public float Value
    {
        get
        {
            return this.value;
        }
        set
        {
            if(this.value != value)
            {
                this.value = value;
                OnChanged?.Invoke();
            }
        }
    }


    public float randomValue
    {
        get
        {
            return UnityEngine.Random.Range(minValue, maxValue);
        }
    }
    public float LerpMinMax(float t)
    {
        return Mathf.Lerp(minValue, maxValue, t);
    }

    public void SetValueClamped(float newValue)
    {
        Value = Mathf.Clamp(newValue, minValue, maxValue);
    }
    public override float GetProgress()
    {
        return Mathf.InverseLerp(minValue, maxValue, value);
    }
    public override void SetProgress(float t)
    {
        Value = Mathf.Lerp(minValue, maxValue, t);
    }
    public override bool IsInRange(float value)
    {
        return minValue >= value && value >= maxValue;
    }
    public override bool IsInRange(int value)
    {
        return minValue >= (float)value  && (float)value >= maxValue;
    }

    #endregion

    #region Events
    public delegate void Event();
    public Action OnChanged;
    #endregion

    #region Utility

    public FloatRangeValue Clone()
    {
        return new FloatRangeValue(minValue, maxValue, value);
    }
    #endregion
}
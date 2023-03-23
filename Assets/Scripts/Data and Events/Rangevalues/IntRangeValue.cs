
using UnityEngine;
using System;

[System.Serializable]
public class IntRangeValue : RangeValue
{
    #region Constructors
    public IntRangeValue(int minValue, int maxValue, int defaultValue)
    {
        this.minValue = minValue;
        this.maxValue = maxValue;
        this.value = defaultValue;
        OnChanged?.Invoke();
    }
    public IntRangeValue(int minValue, int maxValue)
    {
        this.minValue = minValue;
        this.maxValue = maxValue;
        this.value = minValue;
        OnChanged?.Invoke();
    }

    #endregion

    #region Values

    [SerializeField]
    private int minValue = 0;
    [SerializeField]
    private int maxValue = 1;
    [SerializeField]
    private int value = 1;

    public int MinValue
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
    public int MaxValue
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
    public int Value
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

    public int randomValue
    {
        get
        {
            return UnityEngine.Random.Range(minValue, maxValue);
        }
    }
    public int LerpMinMax(float t)
    {
        return (int)Mathf.Lerp(minValue, maxValue, t);
    }

    public void SetValueClamped(int newValue)
    {
        Value = Mathf.Clamp(newValue, minValue, maxValue);
    }
    public override float GetProgress()
    {
        return Mathf.InverseLerp(minValue, maxValue, value);
    }
    public override void SetProgress(float t)
    {
        Value = (int) Mathf.Lerp(minValue, maxValue, t);
    }

    public override bool IsInRange(float value)
    {
        return (float)minValue >= value && value  >= (float)maxValue;
    }
    public override bool IsInRange(int value)
    {
        return minValue >= value && value >= maxValue;
    }

    #endregion

    #region Events
    public delegate void Event();
    public event Event OnChanged;
    #endregion

    #region Utility

    public IntRangeValue Clone()
    {
        return new IntRangeValue(minValue, maxValue, value);
    }
    #endregion
}
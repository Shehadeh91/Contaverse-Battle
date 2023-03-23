using UnityEngine;

public abstract class RangeValue
{
    public abstract float GetProgress();
    public abstract void SetProgress(float progress);
    public abstract bool IsInRange(float value);
    public abstract bool IsInRange(int value);
}
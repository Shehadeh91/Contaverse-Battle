using UnityEngine;

public class ChangeIntVariable : MonoBehaviour
{
    [SerializeField]
    private IntVariable intVariable;

    public void Increase(int amount)
    {
        intVariable.Value += amount;
    }
    public void Decrease(int amount)
    {
        intVariable.Value -= amount;
    }
    public void Set(int amount)
    {
        intVariable.Value = amount;
    }
}
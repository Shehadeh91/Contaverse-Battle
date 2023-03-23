using UnityEngine;
using System;

[System.Serializable]
public class Weightable<T>
{
    public float weight = 1f;
    public T value;

    public Weightable(T value, float weight)
    {
        this.value = value;
        this.weight = weight;
    }
}
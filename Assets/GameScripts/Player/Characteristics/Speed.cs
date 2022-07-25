using System;
using UnityEngine;

public class Speed : MonoBehaviour, IProperty
{
    public const float MinSpeed = 0.1f;
    public float Amount { get; private set;}
    private float amount;
    public Speed(float amount = 1)
    {
        Amount = amount;
    }
    public void Decrease(int amount)
    {
        if (Amount - amount < MinSpeed)
            throw new InvalidOperationException();
           
        Amount -= amount;
    }

    public void Increase(int amount)
    {
        Amount += amount;
    }
}


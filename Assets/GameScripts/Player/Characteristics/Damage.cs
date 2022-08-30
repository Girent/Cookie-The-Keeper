using System;
using UnityEngine;


public class Damage : MonoBehaviour, IProperty
{
    public const float MaxDamage = 100;
    public const float MinDamage = 0;
    public float Amount
    {
        get
        {
            return amount;
        }
        private set
        {
            amount = value;
        }
    }
    [SerializeField] private float amount;

    public void Decrease(int amount)
    {
        if (Amount - amount < MinDamage)
            Amount = MinDamage;
       
        Amount -= amount;

    }

    public void Increase(int amount)
    {
        if (Amount + amount > MaxDamage)
            Amount = MaxDamage;

        Amount += amount;
    }
}


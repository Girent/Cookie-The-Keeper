using System;
using UnityEngine;


public class Health : MonoBehaviour, IProperty 
{   
    public Health()
    {
        Amount = 100;
    }

    public const float MaxHealth = 100;
    public const float MinHealth = 0;
    
    public float Amount{
        get 
        { 
            return amount; 
        }
        private set
        {
            amount = value;
            if (amount > MaxHealth)
                amount = MaxHealth;
        }
    }

    private float amount;

    public void Decrease(int amount)
    {
        if (Amount - amount < MinHealth)
            throw new InvalidOperationException();
        
        Amount -= amount;

    }

    public void Increase(int amount)
    {
        if(Amount + amount > MaxHealth)
            throw new InvalidOperationException();

        Amount += amount;
    }
}


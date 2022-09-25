using System;
using UnityEngine;
using TMPro;

public class Milk : MonoBehaviour, IProperty
{
    [SerializeField] private TextMeshProUGUI counterUI;
    
    public Milk()
    {
        Amout = 0;
    }

    public const int MinAmount = 0;
    public int Amout
    {
        get 
        { 
            return amout; 
        }
        private set
        {
            amout = value;
            counterUI.text = amout.ToString();
        }        
    }
    private int amout;

    public void Increase(int amount)
    {
        Amout += amount;
    }

    public void Decrease(int amount)
    {
        if (Amout - amount < MinAmount)
            throw new InvalidOperationException();

        Amout -= amount;

    }
}


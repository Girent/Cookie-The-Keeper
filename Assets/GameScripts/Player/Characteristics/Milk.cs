using System;
using UnityEngine;
using TMPro;
using Mirror;

public class Milk : NetworkBehaviour, IProperty
{
    [SerializeField] private TextMeshProUGUI counterUI;
    
    [SyncVar(hook = nameof(syncValue))] private int amount;

    public const int MinAmount = 0;


    private void syncValue(int oldValue, int newValue)
    {
        amount = newValue;
        counterUI.text = amount.ToString();
    }

    [Server]
    public void Increase(int amount)
    {
        this.amount += amount;
    }

    public void Decrease(int amount)
    {
        if (this.amount - amount < MinAmount)
            throw new InvalidOperationException();

        this.amount -= amount;

    }
}


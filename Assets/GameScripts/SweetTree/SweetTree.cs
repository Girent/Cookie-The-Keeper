using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class SweetTree : NetworkBehaviour, IHealth
{
    public SweetTree()
    {
        HealthAmount = MaxHealth;
    }

    public const float MaxHealth = 100;
    public const float MinHealth = 0;
    public float HealthAmount
    {
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

    [SerializeField][SyncVar(hook = nameof(syncValue))] private float amount;

    private void syncValue(float oldValue, float newValue)
    {
        amount = newValue;
        if (amount <= 0)
            destroyObject();
    }

    private void destroyObject()
    {
        NetworkServer.Destroy(gameObject);
    }

    [Command(requiresAuthority = false)]
    public void ApplyDamage(float amount, uint netId)
    {
        applyDamage(amount);
    }

    [Server]
    private void applyDamage(float amount)
    {
        HealthAmount -= amount;
    }
}

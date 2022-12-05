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
    public const int MilkPointDrop = 20;

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

    [SyncVar(hook = nameof(syncValue))] private float amount;

    [SerializeField] private GameObject popupText;

    private void syncValue(float oldValue, float newValue)
    {
        amount = newValue;
    }

    private void destroyObject()
    {
        NetworkServer.Destroy(gameObject);
    }

    [Command(requiresAuthority = false)]
    public void ApplyDamage(float amount, uint netId, GameObject player)
    {
        if (this.amount <= 0)
        {
            player.GetComponent<Milk>().Increase(MilkPointDrop);
            destroyObject();
        }

        applyDamage(amount, netId);
    }

    [Server]
    private void applyDamage(float amount, uint netid)
    {
        HealthAmount -= amount;
        spawnPopupDamage(amount);
    }

    [ClientRpc]
    private void spawnPopupDamage(float damage)
    {
        GameObject popup = Instantiate(popupText, gameObject.transform.position, Quaternion.identity);
        popup.GetComponent<DamagePopup>().SetPopupText(damage, Color.blue);
    }
}
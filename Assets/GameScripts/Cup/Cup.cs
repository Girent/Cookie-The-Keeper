using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class Cup : NetworkBehaviour, IHealth
{
    public Cup()
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
    public uint IdMaster;

    [SerializeField] private GameObject popupDamage;
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
    public void ApplyDamage(float amount, uint netId, GameObject player)
    {
        if (IdMaster != netId)
        {
            applyDamage(amount);
        }
    }

    [Server]
    private void applyDamage(float amount)
    {
        HealthAmount -= amount;
        spawnPopupDamage(amount);
    }

    [ClientRpc]
    private void spawnPopupDamage(float damage)
    {
        GameObject popup = Instantiate(popupDamage, gameObject.transform.position, Quaternion.identity);
        popup.GetComponent<DamagePopup>().SetPopupText(damage, Color.white);
    }
}

using Mirror;
using UnityEngine;

public class PlayerCombat : NetworkBehaviour
{
    [SerializeField] private Transform attackPoint;
    [SerializeField] private LayerMask enemyLayers;
    [SerializeField] UIJoystick joystickInput;

    private PlayerProperties playerProperties;
    private Damage damage;

    private void Awake()
    {
        playerProperties = gameObject.GetComponent<PlayerProperties>();
        damage = GetComponent<Damage>();
    }

    public void Attack ()
    {
        attackPoint.localPosition = joystickInput.GetLastDirection() * playerProperties.attackRange;

        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, playerProperties.attackRange / 2f, enemyLayers);

        foreach (var enemy in hitEnemies)
        {
            InflictDamage(enemy.GetComponent<Health>(), damage.Amount);
        }
    }

    [Command]
    public void InflictDamage(Health health, float damage)
    {
        health.ApplyDamage(damage);
    }
}

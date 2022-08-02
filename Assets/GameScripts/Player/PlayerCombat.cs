using Mirror;
using UnityEngine;

public class PlayerCombat : NetworkBehaviour
{
    [SerializeField] private float attackRange;
    [SerializeField] private Transform attackPoint;
    [SerializeField] private LayerMask enemyLayers;
    [SerializeField] UIJoystick joystickInput;

    private NetworkAnimator networkAnimator;
    private Damage damage;

    private void Awake()
    {
        damage = GetComponent<Damage>();
        networkAnimator = GetComponent<NetworkAnimator>();
    }

    public void Attack ()
    {
        attackPoint.localPosition = joystickInput.GetLastDirection() * attackRange;

        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange / 2f, enemyLayers);

        networkAnimator.animator.SetFloat("AttackVector", joystickInput.GetLastDirection().x);
        networkAnimator.SetTrigger("Attack");

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

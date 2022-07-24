using Mirror;
using UnityEngine;

public class PlayerCombat : NetworkBehaviour
{
    [SerializeField] private Transform attackPoint;
    [SerializeField] private LayerMask enemyLayers;
    [SerializeField] UIJoystick joystickInput;

    private PlayerProperties playerProperties;

    private void Awake()
    {
        playerProperties = gameObject.GetComponent<PlayerProperties>();
    }

    public void Attack ()
    {
        attackPoint.localPosition = joystickInput.GetLastDirection() * playerProperties.attackRange;

        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, playerProperties.attackRange / 2f, enemyLayers);

        foreach (var enemy in hitEnemies)
        {
            damageEnemy(enemy.GetComponent<PlayerProperties>());
        }
    }

    [Command]
    private void damageEnemy (PlayerProperties enemy)
    {
        enemy.GettingDamage(playerProperties.playerDamage);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(attackPoint.position, playerProperties.attackRange / 2f);
    }
}

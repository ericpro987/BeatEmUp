using UnityEngine;

public class Hitbox : MonoBehaviour, IAttack
{
    private int damage;
    public int Damage { set => damage = value; }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent<IDamageable>(out IDamageable objective))
        {
            objective.ReceiveDamage(damage);
        }
    }
}

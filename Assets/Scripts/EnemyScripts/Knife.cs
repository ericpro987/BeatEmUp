using uf2;
using UnityEngine;

public class Knife : MonoBehaviour
{
    [SerializeField] private int damage = 20;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        this.GetComponent<Rigidbody2D>().velocity = Vector3.right;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent<IDamageable>(out IDamageable objective))
        {
            objective.ReceiveDamage(damage);
            this.gameObject.SetActive(false);
        }
    }
}

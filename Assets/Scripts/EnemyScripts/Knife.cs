using System.Collections;
using System.Threading;
using uf2;
using UnityEngine;

public class Knife : MonoBehaviour, IAttack
{
    [SerializeField] private int damage;
    [SerializeField] public PJStateMachine personatge;
    public int Damage { set => damage = value; }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void OnEnable()
    {
        this.transform.up = personatge.gameObject.transform.position - this.transform.position;
        this.transform.right = personatge.gameObject.transform.position - this.transform.position;
        this.GetComponent<Rigidbody2D>().velocity = (personatge.gameObject.transform.position - this.transform.position).normalized;
        StartCoroutine(disable());
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent<IDamageable>(out IDamageable objective))
        {
            objective.ReceiveDamage(damage);
            this.gameObject.SetActive(false);
        }
    }
    IEnumerator disable()
    {
        yield return new WaitForSeconds(10);
        this.gameObject.SetActive(false);
    }
}

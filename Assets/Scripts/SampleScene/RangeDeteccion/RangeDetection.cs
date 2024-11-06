using System;
using UnityEngine;

public class RangeDetection : MonoBehaviour
{

    public event Action<GameObject> OnEnter;
    public event Action<GameObject> OnStay;
    public event Action<GameObject> OnExit;

    public void OnTriggerEnter2D(Collider2D collision)
    {
        OnEnter?.Invoke(collision.gameObject);
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        OnStay?.Invoke(collision.gameObject);
    }
    public void OnTriggerExit2D(Collider2D collision)
    {
        OnExit?.Invoke(collision.gameObject);
    }
}
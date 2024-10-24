using System;
using UnityEngine;

public interface IDamageable
{
    void ReceiveDamage(float damage);
}

public interface IAttack
{
    public int Damage { set; }
}


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stat : MonoBehaviour
{
    [SerializeField]
    protected int m_level;
    [SerializeField]
    protected int m_hp;
    [SerializeField]
    protected int m_maxHp;
    [SerializeField]
    protected int m_attack;
    [SerializeField]
    protected float m_attackRate;

    [SerializeField]
    protected float m_moveSpeed;
    [HideInInspector]
    public float m_damage;

    public int Level { get { return m_level; } set { m_level = value; } }
    public int Hp { get { return m_hp; } set { m_hp = value; } }
    public int MaxHp { get { return m_maxHp; } set { m_maxHp = value; } }
    public int Attack { get { return m_attack; } set { m_attack = value; } }
    public float AttackRate { get { return m_attackRate; } set { m_attackRate = value; } }
    public float MoveSpeed { get { return m_moveSpeed; } set { m_moveSpeed = value; } }

    public virtual void OnStart() { }

    public virtual void TakeDamage(float damage) { }

    public virtual void TakeDamage(float damage, bool isCritical) { }
}

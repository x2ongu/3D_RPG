using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimEvent : MonoBehaviour
{
    public TargetManager[] m_attackTarget;
    Animator m_animator;
    Stat m_stat;
    Enemy m_enemy;

    [Header("# Melee")]
    public Collider[] m_melee;
    public TrailRenderer[] m_trailRend;

    [Header("# Ranger")]
    public Transform m_launcher;

    private void Awake()
    {
        m_attackTarget = GetComponentsInChildren<TargetManager>();
        m_animator = GetComponent<Animator>();
        m_stat = GetComponentInParent<Stat>();
        m_enemy = GetComponentInParent<Enemy>();
    }

    public void DoAttack()
    {
        DoAttack(m_attackTarget[0].TargetList, m_stat.Attack);
        Managers.Sound.Play("Enemy/Slash", Define.Sound.Effect, 0.3f);
    }

    public void DoAttack(List<Collider> collList, float damage)
    {
        foreach (Collider coll in collList)
        {
            if (coll == null)
                continue;

            coll.GetComponentInParent<Player>().m_stat.TakeDamage(damage);
        }
    }

    public void ShotProjectile()
    {
        Managers.Sound.Play("Enemy/ShotArrow", Define.Sound.Effect, 0.3f);
        GameObject obj = Managers.Resource.Instantiate("Enemy/Arrow");
        obj.transform.position = m_launcher.position;
        obj.transform.rotation = m_launcher.rotation;
        obj.SetActive(true);

        obj.GetComponent<Bullet>().m_attack = m_stat.Attack;
        obj.GetComponent<Bullet>().Shoot();
    }

    public void SetAttackFalse()
    {
        m_animator.SetBool("isAttack", false);
    }

    public void EndAttack()
    {
        m_enemy.m_isAttackPlaying = false;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BossPattern
{
    JumpAttack, Downward, Roaring,
    Max
}

public class EnemySkeletonBoss : Enemy
{
    public FieldBossAttackIndicator m_attackIndicator;

    [Header("캐릭터 인식 범위")]
    public float m_searchRange = 50f;
    public float m_traceRange = 50f;

    private BossPattern m_bossPattern = BossPattern.JumpAttack;

    private int[] m_pattrnPerTable = { 10, 50, 40 };

    private void OnEnable()
    {
        m_stat.SetEnemyStat(3);
        SetInfo();
    }

    private void Update()
    {
        if (m_stateManager.m_isDead == false)
        {
            if (m_stateManager.IsCloseToTarget(m_stateManager.m_target.position, m_searchRange))
                m_hpBar.SetActive(true);
            else
                m_hpBar.SetActive(false);
        }
    }

    public override void Attack()
    {
        switch (m_bossPattern)
        {
            case BossPattern.JumpAttack:
                m_stateManager.m_anim.SetBool("JumpAttack", true);
                break;
            case BossPattern.Downward:
                m_stateManager.m_anim.SetBool("Downward", true);
                break;
            case BossPattern.Roaring:
                m_stateManager.m_anim.SetBool("Roaring", true);
                break;
        }

        RandomPattern();
    }

    public override void TakeDamage(int damage, bool isCritical, float coefficient = 1)
    {
        if (m_stateManager.m_isDead)
            return;

        m_stat.TakeDamage(damage * coefficient, isCritical);

        StartCoroutine(GetHit());

        IEnumerator GetHit()
        {
            if (m_stat.Hp <= 0)
            {
                m_hpBar.SetActive(false);
                m_stateManager.m_isDead = true;
            }

            Color color = m_meshRend.material.color;

            m_meshRend.material.color = Color.red;

            yield return new WaitForSeconds(0.1f);

            m_meshRend.material.color = color;

            if (m_stat.Hp <= 0)
            {
                m_stateManager.ChangeState(StateDie.Instance);
            }
        }
    }

    public override void Die()
    {
        base.Die();
        GameManager.Inst.m_player.m_stat.Exp += 2000;
        m_minimapIcon.SetActive(false);

        StartCoroutine(Die());
        IEnumerator Die()
        {
            RemoveTarget(m_coll);

            yield return new WaitForSeconds(60f);

            GameManager.Inst.Respawn(this);
            Managers.Resource.Destroy(gameObject);
        }
    }

    public override void SetInfo()
    {
        GetComponent<Collider>().enabled = true;
        m_minimapIcon.SetActive(true);

        m_stateManager.m_searchRange = m_searchRange;
        m_stateManager.m_traceRange = m_traceRange;

        m_stateManager.m_navEnemy.enabled = true;
        m_stateManager.m_anim.SetBool("isLive", true);

        m_stat.Hp = m_stat.MaxHp;
        m_hpBar.SetActive(true);
    }

    private void RandomPattern()
    {
        int index = Random.Range(0, 101);

        if (index < m_pattrnPerTable[0])
        {
            m_bossPattern = BossPattern.JumpAttack;
            m_stat.AttackRange = 5f;
        }
        else if (index < m_pattrnPerTable[0] + m_pattrnPerTable[1])
        {
            m_bossPattern = BossPattern.Downward;
            m_stat.AttackRange = 6f;
        }
        else
        {
            m_bossPattern = BossPattern.Roaring;
            m_stat.AttackRange = 7f;
        }
    }
}

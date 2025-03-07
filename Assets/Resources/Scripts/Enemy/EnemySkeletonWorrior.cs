using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySkeletonWorrior : Enemy
{
    private void OnEnable()
    {
        m_stat.SetEnemyStat(1);
        SetInfo();
    }

    public override void Die()
    {
        base.Die();
        GameManager.Inst.m_player.m_stat.Exp += 10;

        StartCoroutine(Die());
        IEnumerator Die()
        {
            RemoveTarget(m_coll);

            yield return new WaitForSeconds(4f);

            GameManager.Inst.Respawn(this);
            Managers.Resource.Destroy(gameObject);
        }
    }

    public override void SetInfo()
    {
        GetComponent<Collider>().enabled = true;
        m_minimapIcon.SetActive(true);
        
        m_stateManager.m_navEnemy.enabled = true;
        m_stateManager.m_anim.SetBool("isLive", true);

        m_stat.Hp = m_stat.MaxHp;
        m_hpBar.SetActive(true);
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class FieldBossAnimEvent : MonoBehaviour
{
    public TargetManager[] m_attackTarget;
    public NavMeshAgent m_fieldBoss;
    public Transform m_boss;
    public FieldBossAttackIndicator m_attackIndi;
    Stat m_stat;
    Animator m_anim;
    Enemy m_enemy;

    private void Awake()
    {
        m_anim = GetComponent<Animator>();
        m_fieldBoss = GetComponentInParent<NavMeshAgent>();

        m_attackTarget = GetComponentsInChildren<TargetManager>();
        m_boss = GetComponentInParent<Transform>();
        m_stat = GetComponentInParent<Stat>();
        m_enemy = GetComponentInParent<Enemy>();
    }

    public void StartJumpAttack()
    {
        m_attackIndi.StartJumpAttack();
    }

    public void StartDownwardLeftAttack()
    {
        m_attackIndi.StartDownwardLeftAttack();
    }

    public void StartDownwardRightAttack()
    {
        m_attackIndi.StartDownwardRightAttack();
    }

    public void StartRoaringAttack()
    {
        m_attackIndi.StartRoaring();
    }

    public void SetJumpAttackFalse()
    {
        m_anim.SetBool("JumpAttack", false);
        
    }    

    public void SetDownwardFalse()
    {
        m_anim.SetBool("Downward", false);
    }

    public void SetRoaringFalse()
    {
        m_anim.SetBool("Roaring", false);
    }

    public void EndAttack()
    {
        m_enemy.m_isAttackPlaying = false;
    }

    public void DoJumpAttack()
    {
        DoAttack(m_attackTarget[0].TargetList, m_stat.Attack + 10);
        Managers.Sound.Play("Enemy/JumpAttack", Define.Sound.Effect, 0.8f);
    }

    public void DoDownwardLeft()
    {
        DoAttack(m_attackTarget[1].TargetList, m_stat.Attack - 10);
        Managers.Sound.Play("Enemy/Downward", Define.Sound.Effect, 0.8f);
    }

    public void DoDownwardRight()
    {
        DoAttack(m_attackTarget[2].TargetList, m_stat.Attack);
        Managers.Sound.Play("Enemy/Downward", Define.Sound.Effect, 0.8f);
    }

    public void DoRoaring()
    {
        DoAttack(m_attackTarget[3].TargetList, 75f, m_stat.Attack + 30);
    }

    #region Effect
    public void Set_Jump_FX()
    {
        GameObject fx = LoadAndPoolSkillFX("BossJump_FX", 0.3f, 5f);
        fx.SetActive(true);
    }
    public void Set_LeftAttack_FX()
    {
        GameObject fx = LoadAndPoolSkillFX("LeftAttack_FX", 2, 5f);
        fx.SetActive(true);
    }
    public void Set_RightAttack_FX()
    {
        GameObject fx = LoadAndPoolSkillFX("RightAttack_FX", 2, 5f);
        fx.SetActive(true);
    }
    public void Set_Roaring_FX()
    {
        GameObject fx = LoadAndPoolSkillFX("Roaring_FX", 0.5f, 2f);
        fx.SetActive(true);

        Managers.Sound.Play("Enemy/Roaring", Define.Sound.Effect, 1f, 0.2f);
    }

    private GameObject LoadAndPoolSkillFX(string name, float height = 0, float foward = 0)
    {
        GameObject obj = Managers.Resource.Instantiate($"Effect/Enemy/{name}");
        obj.transform.position = m_boss.position + new Vector3(0, height, 0) + (m_boss.forward * foward);
        obj.transform.rotation = m_boss.rotation;

        return obj;
    }
    #endregion

    void DoAttack(List<Collider> collList, float damage)
    {
        foreach (Collider coll in collList)
        {
            if (coll == null)
                continue;

            coll.GetComponentInParent<Player>().m_stat.TakeDamage(m_boss, damage);
        }
    }

    void DoAttack(List<Collider> collList, float angle, float damage)
    {
        foreach (Collider coll in collList)
        {
            if (coll == null)
                continue;

            if (IsFacingTarget(coll, angle))
                coll.GetComponentInParent<Player>().m_stat.TakeDamage(m_boss, damage);

            bool IsFacingTarget(Collider targetPos, float maxAngle)
            {
                Vector3 dirToPlayer = targetPos.transform.position - transform.position;
                Vector3 forwardDir = transform.forward;

                float angle = Vector3.Angle(dirToPlayer, forwardDir);

                if (angle <= maxAngle)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }        
    }
}

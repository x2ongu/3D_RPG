using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AnimationEvent : MonoBehaviour
{
    public Animator m_anim;
    public TargetManager[] m_attackTarget;

    [HideInInspector]
    public Stat m_stat;
    [HideInInspector]
    public SkillData m_skillData;
    [HideInInspector]
    public Vector3 m_forwardDir;
    [HideInInspector]
    public bool m_isMove;
    [HideInInspector]
    public bool m_isAttackPlaying = false;  // 공격이나 스킬 사용중?
    [HideInInspector]
    public bool m_isAttackPos = false;
    [HideInInspector]
    public bool m_superArmor = false;

    private GameObject m_skillEffect;
    private Coroutine m_coroutine;

    private void Awake()
    {
        m_anim = GetComponent<Animator>();
        m_attackTarget = GetComponentsInChildren<TargetManager>();
        m_stat = GetComponentInParent<Stat>();

        m_anim.SetBool("IsLive", true);
        m_anim.SetBool("CanAttack", true);
        m_isMove = true;
    }

    private void LateUpdate()
    {
        if (m_anim.GetCurrentAnimatorStateInfo(1).normalizedTime > 0.7f)
        {
            m_anim.SetLayerWeight(1, 0);
        }
    }

    public void CheckStartComboAttack()
    {
        m_anim.SetBool("Combo", true);
    }

    public void CheckEndComboAttack()
    {
        GameManager.Inst.m_player.m_weapon.GetComponent<Weapon>().m_trailRenderer.SetActive(false);
        m_anim.SetBool("Combo", false);
    }

    public void CanMove()
    {
        m_isMove = true;
        m_isAttackPlaying = false;

        m_anim.SetBool("CanAttack", true);

        GameManager.Inst.m_player.m_weapon.GetComponent<Weapon>().m_trailRenderer.SetActive(false);

        if (GameManager.Inst.m_player.m_weapon != null)
            m_coroutine = StartCoroutine(IsReadyToAttack());

        IEnumerator IsReadyToAttack()
        {
            yield return new WaitForSeconds(10f);

            m_anim.SetBool("IsReadyToAttack", false);
            m_anim.Play("Disarm", 1, 0f);
            m_anim.SetLayerWeight(1, 1);

            yield return new WaitForSeconds(0.5f);

            // 무기 위치 변경
            m_isAttackPos = false;
            GameManager.Inst.m_player.SetWeaponPosition(m_isAttackPos);
        }
    }

    public void CanNotMove()
    {
        m_isMove = false;
        m_isAttackPlaying = true;
        m_isAttackPos = true;

        m_anim.SetBool("CanAttack", false);
        m_anim.SetBool("IsReadyToAttack", true);
        GameManager.Inst.m_player.SetWeaponPosition(m_isAttackPos);

        m_anim.SetLayerWeight(1, 0);

        LookAtForward();

        if (m_coroutine != null)
            StopCoroutine(m_coroutine);
    }

    public void Start_GetHit()
    {
        m_isMove = false;
        m_isAttackPlaying = true;
        m_isAttackPos = true;

        if (m_skillEffect != null)
            Managers.Resource.Destroy(m_skillEffect);

        m_anim.SetBool("CanAttack", false);
        m_anim.SetBool("IsReadyToAttack", true);
        GameManager.Inst.m_player.SetWeaponPosition(m_isAttackPos);

        m_anim.SetLayerWeight(1, 0);

        if (m_coroutine != null)
            StopCoroutine(m_coroutine);
    }

    public void Revive()
    {
        if (m_coroutine != null)
            StopCoroutine(m_coroutine);

        m_isAttackPos = false;
        GameManager.Inst.m_player.SetWeaponPosition(m_isAttackPos);
    }

    public void SetSuperArmor()
    {
        if (m_superArmor == false)
            m_superArmor = true;
        else
            m_superArmor = false;
    }

    #region Attack
    public void Attack_FirstAttack()
    {
        DoAttack(m_attackTarget[0].TargetList, m_stat.Attack, 0.8f);
        Managers.Sound.Play("Player/Attack");
    }

    public void Attack_SecondAttack()
    {
        DoAttack(m_attackTarget[0].TargetList, m_stat.Attack);
        Managers.Sound.Play("Player/Attack");
    }

    public void Attack_ThirdAttack()
    {
        DoAttack(m_attackTarget[0].TargetList, m_stat.Attack, 1.2f);
        Managers.Sound.Play("Player/Attack");
    }

    public void Attack_Upward()
    {
        DoAttack(m_attackTarget[1].TargetList, m_stat.Attack, 1.8f);
    }

    public void Attack_Jump()
    {
        DoAttack(m_attackTarget[2].TargetList, m_stat.Attack, 2.0f);
    }

    public void Attack_Whirl()
    {
        DoAttack(m_attackTarget[3].TargetList, m_stat.Attack, 2.3f);
    }

    public void Attack_XSlash()
    {
        DoAttack(m_attackTarget[4].TargetList, m_stat.Attack, 1.8f);
        Managers.Sound.Play("Player/XSlash");
    }

    public void Attack_Final()
    {
        DoAttack(m_attackTarget[5].TargetList, m_stat.Attack, 5f);
        Managers.Sound.Play("Player/FinalAttack");
    }
    #endregion

    #region Move when using skills
    public void Start_Upward()
    {
        StartCoroutine(DashCoroutine(7f, 0.1f));
    }

    public void Start_JumpAttack()
    {
        Managers.Sound.Play("Player/JumpAttack");
        StartCoroutine(JumpCoroutine());
    }

    public void Start_WhirlAttack()
    {
        StartCoroutine(DashCoroutine(7f, 0.5f));
    }

    public void Start_X_Slash()
    {
        StartCoroutine(DashCoroutine(10f, 0.3f));
    }
    #endregion

    #region Effect

    public void Set_Step_Sound()
    {
        Managers.Sound.Play("Player/Step", Define.Sound.Effect, 0.3f);
    }

    public void Set_Upward_FX()
    {
        m_skillEffect = GetPoolSkillFX("Upward_FX", 2);
        m_skillEffect.SetActive(true);

        Managers.Sound.Play("Player/Upward", Define.Sound.Effect, 1.0f, 3.0f);
    }

    public void Set_Jump_FX()
    {
        m_skillEffect = GetPoolSkillFX("Jump_FX", 0.5f, 2f);
        m_skillEffect.SetActive(true);
    }

    public void Set_Whirl_FX()
    {
        m_skillEffect = GetPoolSkillFX("Whirl_FX", 2f, 1f);
        m_skillEffect.SetActive(true);

        Managers.Sound.Play("Player/WhirlAttack");
    }

    public void Set_XSlash_FX()
    {
        m_skillEffect = GetPoolSkillFX("XSlash_FX", 2.5f);
        m_skillEffect.SetActive(true);
    }

    public void Set_FinalReady_FX()
    {
        m_skillEffect = GetPoolSkillFX("FinalReady_FX");
        m_skillEffect.SetActive(true);

        Managers.Sound.Play("Player/FinalCharge");
    }

    public void Set_Final_FX()
    {
        GameObject fx = GetPoolSkillFX("Final_FX", 0, 5f);
        fx.SetActive(true);
    }

    private GameObject GetPoolSkillFX(string name, float height = 0, float foward = 0)
    {
        GameObject obj = Managers.Resource.Instantiate($"Effect/Player/{name}");
        obj.transform.position = GameManager.Inst.m_player.transform.position + new Vector3(0, height, 0) + (GameManager.Inst.m_player.transform.forward * foward);
        obj.transform.rotation = GameManager.Inst.m_player.transform.rotation;

        return obj;
    }
    #endregion

    private void DoAttack(List<Collider> collList, int damage, float coefficient = 1)
    {
        foreach (Collider coll in collList)
        {
            if (coll == null)
                continue;

            if (IsCritical())
            {
                coll.GetComponent<Enemy>().TakeDamage(damage * 2, true, coefficient);
                Debug.Log("크리티컬!!");
            }
            else
                coll.GetComponent<Enemy>().TakeDamage(damage, false, coefficient);
        }

        StartCoroutine(AttackReact());

        IEnumerator AttackReact()
        {
            Time.timeScale = 0f;

            float timer = 0f;
            while (timer < 0.05f)
            {
                timer += Time.unscaledDeltaTime;
                yield return null;
            }

            Time.timeScale = 1f;
        }

        bool IsCritical()
        {
            bool isCritical;
            int num = Random.Range(0, 100);

            if (num < GameManager.Inst.m_player.m_stat.Critical)
                isCritical = true;
            else
                isCritical = false;

            return isCritical;
        }
    }

    private void LookAtForward()
    {
        GameManager.Inst.m_player.m_navAgent.ResetPath();

        m_forwardDir = Managers.Input.MousePointByRay() - GameManager.Inst.m_player.transform.position;
        m_forwardDir.y = 0;
        m_forwardDir.Normalize();

        GameManager.Inst.m_player.transform.forward = m_forwardDir; // 이렇게 정면을 관리하면 순간이동 함... 근데 해도 될 듯?
    }    

    private IEnumerator DashCoroutine(float dashDist, float duration)
    {
        // 대쉬 시작 시간
        float startTime = Time.time;

        // 대쉬가 끝나는 시간
        float endTime = startTime + duration;

        // 대쉬를 실행하는 동안 반복
        while (Time.time < endTime)
        {
            // 대상 지점 방향으로 일정 속도로 이동
            GameManager.Inst.m_player.transform.position += GameManager.Inst.m_player.transform.forward * dashDist * Time.deltaTime;

            yield return null;
        }
    }

    private IEnumerator JumpCoroutine()
    {
        float startTime = Time.time;
        float endTime = startTime + 0.7f;

        Vector3 taget = Managers.Input.MousePointByRay();

        while (Time.time < endTime)
        {
            GameManager.Inst.m_player.transform.position = Vector3.MoveTowards(GameManager.Inst.m_player.transform.position, taget, 20 * Time.deltaTime);

            yield return null;
        }
    }
}
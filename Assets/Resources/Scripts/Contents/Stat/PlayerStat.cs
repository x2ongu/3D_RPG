using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStat : Stat
{
    [SerializeField]
    protected int m_exp;
    [SerializeField]
    protected int m_totalExp;
    [SerializeField]
    protected int m_mp;
    [SerializeField]
    protected int m_maxMp;
    [SerializeField]
    protected int m_gold;
    [SerializeField]
    protected int m_critical;

    public int Exp
    {
        get { return m_exp; }
        set
        {
            m_exp = value;
            // totalExp를 가득 채워서 레벨 업을 했는지 검사
            // 다른 외부에서의 경험치를 올리는 경우(ex) 마인크래프트 인챈트 포션을 먹었을 때) 해당 작업 마다 레벨업을 했는지 검사해야 하지만
            // 해당 프로퍼티 내부에 만든다면 PlayerStat.Exp에 경험치를 추가했을 때 자동으로 검사해 줄 수 있다.
            int level = m_level;

            while (true)
            {
                Data.Stat stat;
                if (Managers.Data.StatDict.TryGetValue(level + 1, out stat) == false)   // 현재 레벨에서 +1 한 Data가 없다면 다음 레벨이 없으므로 break;
                    break;

                int requiredExpForNextLevel = stat.totalExp - Managers.Data.StatDict[level].totalExp;

                if (m_exp < requiredExpForNextLevel) // 남은 경험치가 다음 레벨업에 필요한 경험치보다 적으면 중단
                    break;

                m_exp -= requiredExpForNextLevel;
                level++;    // 위 조건에 해당되지 않았다면 레벨업!
            }

            if (m_level != level)    // _level과 level이 다르면? : 레벨에 변화가 일어났다면
            {
                Level = level;
                Managers.Sound.Play("Player/LevelUp");
                GameObject obj = Managers.Resource.Instantiate("Effect/Player/LevelUp");
                obj.SetActive(true);
                SetStat(Level);
                StartCoroutine(Managers.Data.SaveDataCoroutine());
            }
        }
    }
    public int TotalExp { get { return m_totalExp; } set { m_totalExp = value; } }
    public int Mp { get { return m_mp; } set { m_mp = value; } }
    public int MaxMp { get { return m_maxMp; } set { m_maxMp = value; } }
    public int Gold { get { return m_gold; } set { m_gold = value; } }
    public int Critical
    { 
        get
        {
            Mathf.Clamp(m_critical, 0, 100);
            return m_critical;
        } 
        set { m_critical = value; } 
    }

    public override void OnStart()
    {
        base.OnStart();

        m_attackRate = 0.5f;
        m_moveSpeed = 10f;
        m_critical = 20;

        InvokeRepeating("ManaRegen", 1.5f, 1.5f);
    }

    public void SetStat(int level)
    {
        Dictionary<int, Data.Stat> dict = Managers.Data.StatDict;

        Data.Stat stat = dict[level];
        int totalExp = level + 1;

        m_hp = stat.maxHp;
        m_maxHp = stat.maxHp;
        m_mp = stat.maxMp;
        m_maxMp = stat.maxMp;
        m_attack = stat.attack;

        if (level == 10)
            return;

        m_totalExp = dict[totalExp].totalExp;
        m_exp = 0;
    }

    public void TakeDamage(Transform boss, float damage)
    {
        if (GameManager.Inst.m_player.m_isLive == false)
            return;

        if (GameManager.Inst.m_player.m_animEvent.m_superArmor == false)
        {
            GameManager.Inst.m_player.m_animEvent.m_anim.Play("GetHit");
            GameManager.Inst.m_player.m_animEvent.m_isMove = false;
            GameManager.Inst.m_player.m_navAgent.ResetPath();
            transform.forward = boss.position - transform.position;
            TakeDamage(damage);
        }
        else
            TakeDamage(damage * 0.5f);
    }

    public override void TakeDamage(float damage)
    {
        if (GameManager.Inst.m_player.m_isLive == false)
            return;

        GameManager.Inst.m_player.m_isAttackMode = true;

        float tmp = Random.Range(damage - (damage * 0.2f), damage + (damage * 0.2f));
        m_damage = Mathf.RoundToInt(tmp);
        Hp -= Mathf.RoundToInt(m_damage);

        GameObject obj = Managers.Resource.Instantiate("UI/WorldSpace/UI_PlayerDamage");
        obj.GetComponent<UI_PlayerDamage>().SetTransform(gameObject);
        obj.SetActive(true);

        if (Hp <= 0)
        {
            Hp = 0;
            OnDead();
        }
    }

    private void ManaRegen()
    {
        Mp += Mathf.RoundToInt(MaxMp * 0.1f);

        if (Mp >= MaxMp)
            Mp = MaxMp;
    }

    protected void OnDead()
    {
        GameManager.Inst.m_player.m_animEvent.m_anim.SetBool("IsLive", false);
        GameManager.Inst.m_player.m_animEvent.m_anim.SetTrigger("GetDead");
        GameManager.Inst.m_player.m_isLive = false;
        GameManager.Inst.m_player.m_isAttackMode = false;
        GameManager.Inst.m_player.m_navAgent.isStopped = true;
        GameManager.Inst.m_player.m_navAgent.enabled = false;

        Managers.Sound.Play("Player/Dead");
        GameObject obj = Managers.Resource.Instantiate("Effect/Player/Dead");
        obj.SetActive(true);

        StartCoroutine(EndDeadAnim());

        IEnumerator EndDeadAnim()
        {
            yield return new WaitForSeconds(3.0f);

            GameManager.Inst.m_popup.OpenPopUp(GameManager.Inst.m_popup.m_reviver, false);
        }

    }
}
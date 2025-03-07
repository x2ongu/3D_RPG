using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStat : Stat
{
    [SerializeField]
    protected float m_attackRange;
    [SerializeField]
    protected float m_respawnTime;

    public float AttackRange { get { return m_attackRange; } set { m_attackRange = value; } }
    public float RespawnTime { get { return m_respawnTime; } set { m_respawnTime = value; } }


    public override void OnStart()
    {
        base.OnStart();
    }

    public override void TakeDamage(float damage, bool isCritical)
    {
        GameManager.Inst.m_player.m_isAttackMode = true;

        float tmp = Random.Range(damage - (damage * 0.2f), damage + (damage * 0.2f));
        m_damage = Mathf.RoundToInt(tmp);
        Hp -= Mathf.RoundToInt(m_damage);

        Bloodthirster(m_damage);

        GameObject obj = Managers.Resource.Instantiate("UI/WorldSpace/UI_Damage");
        obj.GetComponent<UI_Damage>().SetTransform(gameObject);
        obj.GetComponent<UI_Damage>().SetColorByCritical(isCritical);
        obj.SetActive(true);
    }

    public void SetEnemyStat(int level)
    {
        Dictionary<int, Data.Enemy> dict = Managers.Data.EnemyDict;

        Data.Enemy stat = dict[level];

        m_hp = stat.maxHp;
        m_maxHp = stat.maxHp;
        m_attack = stat.attack;
        m_attackRate = stat.attackRate;
        m_moveSpeed = stat.moveSpeed;
        m_attackRange = stat.attackRange;
        m_respawnTime = stat.respawnTime;
    }

    private void Bloodthirster(float damage)
    {
        if (!string.Equals(GameManager.Inst.m_player.m_weapon.name, "Weapon_Bloodthirster"))
            return;

        int value = Mathf.RoundToInt(damage / 10);

        GameObject obj = Managers.Resource.Instantiate("UI/WorldSpace/UI_Damage");
        obj.GetComponent<UI_Damage>().Bloodthirster(value);
        obj.SetActive(true);

        GameManager.Inst.m_player.m_stat.Hp += value;
    }
}

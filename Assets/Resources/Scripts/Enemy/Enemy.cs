using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEnemy
{   
    void Attack();
    void TakeDamage(int damage, bool isCritical, float coefficient = 1);
    void Die();
}

public class Enemy : MonoBehaviour, IEnemy
{
    public GameObject m_enemyPrefab;
    public GameObject[] m_dropItems;
    public GameObject m_hpBar;
    public GameObject m_minimapIcon;

    [HideInInspector]
    public SkinnedMeshRenderer m_meshRend;
    [HideInInspector]
    public StateManager m_stateManager;
    [HideInInspector]
    public EnemyStat m_stat;

    protected Collider m_coll;

    [HideInInspector]
    public bool m_isAttackPlaying = false;

    private void Awake()
    {
        m_stateManager = GetComponent<StateManager>();
        m_meshRend = GetComponentInChildren<SkinnedMeshRenderer>();

        m_stat = GetComponent<EnemyStat>();
        m_coll = GetComponent<Collider>();
    }

    public virtual void Attack()
    {
        m_stateManager.m_anim.SetBool("isAttack", true);
        transform.position = transform.position;
    }

    public virtual void TakeDamage(int damage, bool isCritical, float coefficient = 1)
    {
        if (m_stateManager.m_isDead)
            return;

        m_stateManager.m_anim.SetTrigger("getHit");
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

            m_meshRend.material.color = Color.gray;

            yield return new WaitForSeconds(0.1f);

            m_meshRend.material.color = color;

            if (m_stat.Hp <= 0)
            {
                m_stateManager.ChangeState(StateDie.Instance);
            }
        }
    }

    public virtual void Die()
    {
        ItemDrop();
        GameManager.Inst.m_quest.KillQuestProcess(gameObject);
        GetComponent<Collider>().enabled = false;
        m_minimapIcon.SetActive(false);
    }

    public virtual void SetInfo() { }

    public void RemoveTarget(Collider coll)
    {
        for (int i = 0; i < GameManager.Inst.m_player.m_targeters.Length; i++)
        {
            GameManager.Inst.m_player.m_targeters[i].RemoveTarget(coll);
        }
    }

    private void ItemDrop()
    {
        float dropRate = 0f;

        for (int i = 0; i < m_dropItems.Length; i++)
        {
            dropRate = Random.Range(0f, 100f);

            if (dropRate < m_dropItems[i].GetComponent<Item>().m_itemData.m_dropRate)
            {
                LoadDropItem(m_dropItems[i]);
            }
        }
    }

    private void LoadDropItem(GameObject obj)
    {
        Item item = obj.GetComponent<Item>();

        switch (item.m_itemData.m_itemType)
        {
            case ItemData.ItemType.Gold:
                {
                    GameObject dropItem = Managers.Resource.Instantiate($"Item/{item.name}");
                    dropItem.SetActive(true);
                    dropItem.transform.position = transform.position;
                }
                break;
            case ItemData.ItemType.Consumable:
                {
                    GameObject dropItem = Managers.Resource.Instantiate($"Item/Consumable/{item.name}");
                    dropItem.SetActive(true);
                    dropItem.transform.position = transform.position;
                }
                break;
            case ItemData.ItemType.Equipment:
                {
                    GameObject dropItem = Managers.Resource.Instantiate($"Item/Equipment/{item.name}");
                    dropItem.SetActive(true);
                    dropItem.transform.position = transform.position;
                }
                break;
            case ItemData.ItemType.ETC:
                {
                    GameObject dropItem = Managers.Resource.Instantiate($"Item/ETC/{item.name}");
                    dropItem.SetActive(true);
                    dropItem.transform.position = transform.position;
                }
                break;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Top", menuName = "Items/Equipment/Top")]
public class TopData : EquipmentData
{
    public int m_maxHealth;
    public override void WhenTheItemDrops()
    {
        Debug.Log(m_itemName + "¿ª »πµÊ«œºÃΩ¿¥œ¥Ÿ!");
    }

    public override void SetItem()
    {
        GameManager.Inst.m_player.m_stat.MaxHp += m_maxHealth;
    }

    public override void ClearItem()
    {
        GameManager.Inst.m_player.m_stat.MaxHp -= m_maxHealth;
    }
}
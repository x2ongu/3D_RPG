using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Helmet", menuName = "Items/Equipment/Helmet")]
public class HelmetData : EquipmentData
{
    public int m_critical;

    public override void WhenTheItemDrops()
    {
        Debug.Log(m_itemName + "¿ª »πµÊ«œºÃΩ¿¥œ¥Ÿ!");
    }

    public override void SetItem()
    {
        GameManager.Inst.m_player.m_stat.Critical += m_critical;
    }

    public override void ClearItem()
    {
        GameManager.Inst.m_player.m_stat.Critical -= m_critical;
    }
}
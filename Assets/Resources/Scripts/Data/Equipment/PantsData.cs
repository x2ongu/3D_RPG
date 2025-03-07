using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Pants", menuName = "Items/Equipment/Pants")]
public class PantsData : EquipmentData
{
    public float m_moveSpeed;

    public override void WhenTheItemDrops()
    {
        Debug.Log(m_itemName + "¿ª »πµÊ«œºÃΩ¿¥œ¥Ÿ!");
    }

    public override void SetItem()
    {
        GameManager.Inst.m_player.m_stat.MoveSpeed += m_moveSpeed;
    }

    public override void ClearItem()
    {
        GameManager.Inst.m_player.m_stat.MoveSpeed -= m_moveSpeed;
    }
}
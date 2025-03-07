using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Weapon", menuName = "Items/Equipment/Weapon")]
public class WeaponData : EquipmentData
{
    public int m_damage;
    public override void WhenTheItemDrops()
    {
        Debug.Log(m_itemName + "¿ª »πµÊ«œºÃΩ¿¥œ¥Ÿ!");
    }

    public override void SetItem()
    {
        GameManager.Inst.m_player.m_stat.Attack += m_damage;

        if (string.Equals(m_itemName, "««πŸ∂Û±‚"))
            GameManager.Inst.m_player.m_stat.Critical += 30;
    }

    public override void ClearItem()
    {
        GameManager.Inst.m_player.m_weapon.transform.localScale /= 2f;
        Managers.Resource.Destroy(GameManager.Inst.m_player.m_weapon);
        GameManager.Inst.m_player.m_weapon = null;
        GameManager.Inst.m_player.m_stat.Attack -= m_damage;

        if (string.Equals(m_itemName, "««πŸ∂Û±‚"))
            GameManager.Inst.m_player.m_stat.Critical -= 30;

        GameManager.Inst.m_copyPlayer.m_weapon.transform.localScale /= 2f;
        Managers.Resource.Destroy(GameManager.Inst.m_copyPlayer.m_weapon);
    }
}

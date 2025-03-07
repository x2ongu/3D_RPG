using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stats : UI_PopUp
{
    enum Objects
    {
        Armor_Slot_Helmet,
        Armor_Slot_Top,
        Armor_Slot_Pants,
        Weapon_Slot
    }

    enum Texts
    {
        Text_Attack_Value,
        Text_MaxHP,
        Text_Critical_Value,
        Text_MaxMP,
        Text_MoveSpeed,
        Text_Level
    }

    [Header("#Slots")]
    public UI_Slot_Stat[] m_slots;

    private void Update()
    {
        SetText();
    }

    public override void Init()
    {
        BindObject(typeof(Objects));
        BindText(typeof(Texts));

        SetText();

        m_slots = GetComponentsInChildren<UI_Slot_Stat>();
        for (int i = 0; i < m_slots.Length; i++)
        {
            if (m_slots[i] == null)
                m_slots[i] = GetComponent<UI_Slot_Stat>();
        }
    }

    public void SetText()
    {
        GetText((int)Texts.Text_Attack_Value).text = $"{GameManager.Inst.m_player.m_stat.Attack}";
        GetText((int)Texts.Text_MaxHP).text = $"{GameManager.Inst.m_player.m_stat.MaxHp}";
        GetText((int)Texts.Text_Critical_Value).text = $"{GameManager.Inst.m_player.m_stat.Critical}" + "%";
        GetText((int)Texts.Text_MaxMP).text = $"{GameManager.Inst.m_player.m_stat.MaxMp}";
        GetText((int)Texts.Text_MoveSpeed).text = $"{GameManager.Inst.m_player.m_stat.MoveSpeed}";
        GetText((int)Texts.Text_Level).text = $"{GameManager.Inst.m_player.m_stat.Level}";
    }

    public bool SetItem(EquipmentData data, GameObject changeSlot)
    {
        bool isItSet = false;

        switch (data.m_equipmentType)
        {
            case EquipmentData.EquipmentType.Helmet:        // Helmet Slot In Stats UI
                if (m_slots[0].m_itemData == null)
                {
                    m_slots[0].GetComponent<UI_Slot_Stat>().SetItem(data);
                    (data as HelmetData).SetItem();
                    isItSet = true;
                }  
                else
                {
                    ItemData temp = m_slots[0].m_itemData;
                    changeSlot.GetComponent<UI_Slot_Inven>().AddItem(temp);
                    (temp as HelmetData).ClearItem();

                    m_slots[0].GetComponent<UI_Slot_Stat>().SetItem(data);
                    (data as HelmetData).SetItem();
                }                
                break;



            case EquipmentData.EquipmentType.Top:        // Top Slot In Stats UI
                if (m_slots[1].m_itemData == null)
                {
                    m_slots[1].GetComponent<UI_Slot_Stat>().SetItem(data);
                    (data as TopData).SetItem();
                    isItSet = true;
                }
                else
                {
                    ItemData temp = m_slots[1].m_itemData;
                    changeSlot.GetComponent<UI_Slot_Inven>().AddItem(temp);
                    (temp as TopData).ClearItem();

                    m_slots[1].GetComponent<UI_Slot_Stat>().SetItem(data);
                    (data as TopData).SetItem();
                }
                break;



            case EquipmentData.EquipmentType.Pants:        // Pants Slot In Stats UI
                if (m_slots[2].m_itemData == null)
                {
                    m_slots[2].GetComponent<UI_Slot_Stat>().SetItem(data);
                    (data as PantsData).SetItem();
                    isItSet = true;
                }
                else
                {
                    ItemData temp = m_slots[2].m_itemData;
                    changeSlot.GetComponent<UI_Slot_Inven>().AddItem(temp);
                    (temp as PantsData).ClearItem();

                    m_slots[2].GetComponent<UI_Slot_Stat>().SetItem(data);
                    (data as PantsData).SetItem();
                }
                break;



            case EquipmentData.EquipmentType.Weapon:        // Weapon Slot In Stats UI
                if (m_slots[3].m_itemData == null)
                {
                    m_slots[3].GetComponent<UI_Slot_Stat>().SetItem(data);
                    (data as WeaponData).SetItem();
                    isItSet = true;

                    GameManager.Inst.m_player.m_weapon = LoadWeaponPrefab(data);
                    GameManager.Inst.m_player.SetWeaponPosition(GameManager.Inst.m_player.m_animEvent.m_isAttackPos);
                    GameManager.Inst.m_player.m_weapon.GetComponent<Weapon>().m_trailRenderer.SetActive(false);
                }
                else
                {
                    ItemData temp = m_slots[3].m_itemData;
                    changeSlot.GetComponent<UI_Slot_Inven>().AddItem(temp);
                    (temp as WeaponData).ClearItem();

                    m_slots[3].GetComponent<UI_Slot_Stat>().SetItem(data);
                    (data as WeaponData).SetItem();

                    bool isAttackPlaying = GameManager.Inst.m_player.m_animEvent.m_isAttackPlaying;
                    Debug.Log(isAttackPlaying);

                    GameManager.Inst.m_player.m_weapon = LoadWeaponPrefab(data);
                    GameManager.Inst.m_player.SetWeaponPosition(GameManager.Inst.m_player.m_animEvent.m_isAttackPos);
                    GameManager.Inst.m_player.m_weapon.GetComponent<Weapon>().m_trailRenderer.SetActive(false);
                }
                break;
        }

        return isItSet;
    }

    private GameObject LoadWeaponPrefab(EquipmentData data)
    {
        GameObject obj = Managers.Resource.Instantiate($"Item/Equipment/{data.m_itemPrefab.name}");
        obj.transform.localScale *= 2;

        GameManager.Inst.m_copyPlayer.SetWeapon(obj);

        return obj;
    }
}

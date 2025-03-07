using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_Slot_Stat : UI_Drag_Item
{
    enum Images
    {
        Item_Image,
    }

    private void Awake()
    {
        BindImage(typeof(Images));
    }

    protected override void OnClickSlot(PointerEventData eventData)
    {
        if (GameManager.Inst.m_player.m_isAttackMode == true)
            return;

        if (m_itemData == null)
            return;

        if (eventData.button == PointerEventData.InputButton.Right)
        {
            ClearSlot();
            Managers.Sound.Play("UI/Unequip");
        }
    }

    public override void SetColor(float alpha)
    {
        Color color = GetImage((int)Images.Item_Image).color;
        color.a = alpha;
        GetImage((int)Images.Item_Image).color = color;
    }

    public void SetItem(EquipmentData data)
    {
        BindImage(typeof(Images));
        m_itemData = data;
        GetImage((int)Images.Item_Image).sprite = m_itemData.m_icon;
        SetColor(1);
    }

    public override void ClearSlot()
    {
        GameManager.Inst.m_popup.m_invenPopUp.GetComponent<Inventory>().AcquireItem(m_itemData);

        switch((m_itemData as EquipmentData).m_equipmentType)
        {
            case EquipmentData.EquipmentType.Helmet:
                (m_itemData as HelmetData).ClearItem();
                break;
            case EquipmentData.EquipmentType.Top:
                (m_itemData as TopData).ClearItem();
                break;
            case EquipmentData.EquipmentType.Pants:
                (m_itemData as PantsData).ClearItem();
                break;
            case EquipmentData.EquipmentType.Weapon:
                (m_itemData as WeaponData).ClearItem();
                break;
        }

        m_itemData = null;

        SetColor(0);

        GetImage((int)Images.Item_Image).sprite = null;
    }
}
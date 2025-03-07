using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UI_Slot_Shop : UI_Drag_Item
{
    enum Images
    {
        Item_Icon,
    }

    enum Texts
    {
        Item_Name_Text,
        Item_Description_Text,
        Gold_Text
    }

    public override void SetInfo()
    {
        BindImage(typeof(Images));
        BindText(typeof(Texts));

        GetImage((int)Images.Item_Icon).sprite = m_itemData.m_icon;
        GetText((int)Texts.Item_Name_Text).text = m_itemData.m_itemName;
        GetText((int)Texts.Item_Description_Text).text = m_itemData.m_itemDesc;
        GetText((int)Texts.Gold_Text).text = string.Format("{0:#,###}", m_itemData.m_buyPrice);
    }

    protected override void OnClickSlot(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right) // 아이템 우클릭으로 사용 시 여긴 일단 나중에 건들이기
        {
            if (m_itemData != null)
            {
                BuyItem(m_itemData);
            }
        }
    }

    protected override void OnBeginDragSlot(PointerEventData eventData)
    {
        return;
    }

    public void BuyItem(ItemData data)
    {
        if (data.m_itemType == ItemData.ItemType.Equipment)
        {
            GameManager.Inst.m_popup.OpenPopUp(GameManager.Inst.m_popup.m_deal_equipment, false);
            GameManager.Inst.m_popup.m_deal_equipment.GetComponent<UI_Deal_Equipment>().Set_Buy_Item(data);
        }
        else if (data.m_itemType == ItemData.ItemType.Consumable)
        {
            GameManager.Inst.m_popup.OpenPopUp(GameManager.Inst.m_popup.m_deal_consumable, false);
            GameManager.Inst.m_popup.m_deal_consumable.GetComponent<UI_Deal_Consumable>().Set_Buy_Item(data);
        }
    }
}

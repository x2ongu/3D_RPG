using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class UI_Slot_Inven : UI_Drag_Item
{
    [SerializeField]
    private Image m_itemIcon;
    [SerializeField]
    private TextMeshProUGUI m_itemCountText;

    public int m_itemCount;


    public override void SetInfo()
    {
        if (m_itemData != null)
            AddItem(m_itemData, 0);
    }

    protected override void OnClickSlot(PointerEventData eventData)
    {
        if (GameManager.Inst.m_player.m_isAttackMode == true)
            return;

        if (eventData.button == PointerEventData.InputButton.Right) // 아이템 우클릭으로 사용 시
        {
            if (m_itemData != null)
            {
                if (!GameManager.Inst.m_popup.m_isShopOpen)
                {
                    if (m_itemData.m_itemType == ItemData.ItemType.Equipment)
                    {
                        // 장비템 착용
                        SetEquipment();
                    }
                }
                else
                {
                    SellItem(m_itemData , m_itemCount);
                }
            }
        }
    }

    protected override void OnEndDragSlot(PointerEventData eventData)
    {
        if (eventData.button != PointerEventData.InputButton.Left)
            return;

        if (m_itemData.IsNull() == false && !EventSystem.current.IsPointerOverGameObject())
        {
            ClearSlot();
        }

        base.OnEndDragSlot(eventData);
    }

    protected override void OnDropSlot(PointerEventData eventData)
    {
        UI_Slot dragSlot = UI_Drag_Slot.instance.m_slot;

        if (dragSlot.IsNull() == false)
        {
            // 해당 Script를 Drag해 본인 Script에 Drop할 경우 return
            if (dragSlot == this)
                return;

            if ((dragSlot is UI_Slot_Inven) == true)            // dragSlot이 같은 Inven창 에서 온 경우
            {
                ExchangeItem(dragSlot as UI_Slot_Inven);
            }
            else
                return;

            GameManager.Inst.m_quest.CollectQuestProcess();
        }
    }

    private void ExchangeItem(UI_Slot_Inven invenSlot)
    {
        ItemData temp = this.m_itemData;
        int tempCount = this.m_itemCount;

        ClearSlot();
        AddItem(invenSlot.m_itemData, invenSlot.m_itemCount);
        GameManager.Inst.m_popup.m_invenPopUp.GetComponent<Inventory>().SetQuickSlot(this);

        if (temp != null)
        {
            UI_Drag_Slot.instance.m_slot.GetComponent<UI_Slot_Inven>().ClearSlot();
            UI_Drag_Slot.instance.m_slot.GetComponent<UI_Slot_Inven>().AddItem(temp, tempCount);
            GameManager.Inst.m_popup.m_invenPopUp.GetComponent<Inventory>().SetQuickSlot(UI_Drag_Slot.instance.m_slot.GetComponent<UI_Slot_Inven>());
        }
        else
        {
            UI_Drag_Slot.instance.m_slot.GetComponent<UI_Slot_Inven>().ClearSlot();
        }

        GameManager.Inst.m_quest.CollectQuestProcess();
    }

    public void AddItem(ItemData item, int count = 1)
    {
        m_itemData = item;
        m_itemIcon.sprite = m_itemData.m_icon;

        if (m_itemData.m_itemType != ItemData.ItemType.Equipment)
        {
            m_itemCount += count;

            m_itemCountText.gameObject.SetActive(true);
            m_itemCountText.text = m_itemCount.ToString();            
        }
        else
        {
            m_itemCountText.text = "0";
            m_itemCountText.gameObject.SetActive(false);
        }

        SetColor(1);
    }

    public void SetItemCount(int count)
    {
        m_itemCount += count;
        m_itemCountText.text = m_itemCount.ToString();

        if (m_itemCount <= 0)
            ClearSlot();
    }

    public override void SetColor(float alpha)
    {
        Color color = m_itemIcon.color;
        color.a = alpha;
        m_itemIcon.color = color;
    }

    public override void ClearSlot()
    {
        if (m_itemData != null)
        {
            AddItem(m_itemData, -m_itemCount);
            GameManager.Inst.m_quest.CollectQuestProcess(m_itemData, -m_itemCount);
        }

        m_itemData = null;
        m_itemIcon.sprite = null;
        SetColor(0);

        m_itemCountText.text = "0";
        m_itemCountText.gameObject.SetActive(false);
    }

    public void SellItem(ItemData data, int count = 1)
    {
        if (data.m_itemType == ItemData.ItemType.Equipment)
        {
            GameManager.Inst.m_popup.OpenPopUp(GameManager.Inst.m_popup.m_deal_equipment, false);
            GameManager.Inst.m_popup.m_deal_equipment.GetComponent<UI_Deal_Equipment>().Set_Sell_Item(data, this);
        }
        else if (data.m_itemType == ItemData.ItemType.Consumable || data.m_itemType == ItemData.ItemType.ETC)
        {
            GameManager.Inst.m_popup.OpenPopUp(GameManager.Inst.m_popup.m_deal_consumable, false);
            GameManager.Inst.m_popup.m_deal_consumable.GetComponent<UI_Deal_Consumable>().Set_Sell_Item(data, this);
        }
    }

    private void SetEquipment()
    {
        EquipmentData data = m_itemData as EquipmentData;

        if (GameManager.Inst.m_popup.m_statsPopUp.GetComponent<Stats>().SetItem(data, gameObject) == true)
            ClearSlot();

        Managers.Sound.Play("UI/Equip");
    }
}

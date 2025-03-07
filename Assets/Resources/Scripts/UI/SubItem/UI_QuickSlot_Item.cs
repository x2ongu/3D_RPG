using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class UI_QuickSlot_Item : UI_Drag_Item
{
    enum Images
    {
        Item_Icon,
        Item_Cooldown_Image
    }

    enum Texts
    {
        Item_Count_Text,
        Item_Cooldown_Text,
        Item_Key_Text
    }

    [SerializeField]
    private UI_QuickSlot_Item m_anotherSlot;
    private ConsumableData m_data;
    // [HideInInspector]
    public UI_Slot_Inven m_invenSlot;

    [HideInInspector]
    public KeyCode m_inputKey;

    private float m_time;
    [HideInInspector]
    public bool m_isCoolDown = false;

    public void SetInfomation()
    {
        BindImage(typeof(Images));
        BindText(typeof(Texts));

        GetText((int)Texts.Item_Key_Text).text = SetString(m_inputKey.ToString());

        GetText((int)Texts.Item_Count_Text).text = "";
        GetImage((int)Images.Item_Cooldown_Image).fillAmount = 0;
        GetImage((int)Images.Item_Cooldown_Image).gameObject.SetActive(false);

        SetColor(0);

        SetEventHandler();
    }

    public void InputKey()
    {
        m_data = m_itemData as ConsumableData;

        if (m_data == null || m_isCoolDown == true)
            return;

        if (m_data.m_consumType == ConsumableData.ConsumableType.Health)
        {
            TakePosition(m_data.m_consumType);

            m_invenSlot.SetItemCount(-1);
            GetText((int)Texts.Item_Count_Text).text = string.Format("{0}", m_invenSlot.m_itemCount);
        }
        else if (m_data.m_consumType == ConsumableData.ConsumableType.Mana)
        {
            TakePosition(m_data.m_consumType);

            m_invenSlot.SetItemCount(-1);
            GetText((int)Texts.Item_Count_Text).text = string.Format("{0}", m_invenSlot.m_itemCount);
        }

        if (m_invenSlot.m_itemCount >= 1)
            StartCoroutine(CoolDown());
        else if (m_invenSlot.m_itemCount <= 0)
            ClearSlot();
    }

    protected override void OnEndDragSlot(PointerEventData eventData)
    {
        if (GameManager.Inst.m_player.m_isAttackMode == true)
            return;

        // 마지막 Drag위치가 UI가 아니라면
        if (m_itemData.IsNull() == false && !EventSystem.current.IsPointerOverGameObject())
        {
            if (m_isCoolDown == false)
                ClearSlot();
        }

        base.OnEndDragSlot(eventData);
    }

    protected override void OnDropSlot(PointerEventData eventData)
    {
        if (GameManager.Inst.m_player.m_isAttackMode == true)
            return;

        UI_Slot dragSlot = UI_Drag_Slot.instance.m_slot;

        if (dragSlot.IsNull() == false)
        {
            // 해당 Script를 Drag해 본인 Script에 Drop할 경우 return
            if (dragSlot == this)
                return;

            if ((dragSlot is UI_Slot_Inven) == true)            // dragSlot이 Inventory에서 온 경우
            {
                ChangeItem(dragSlot as UI_Slot_Inven);
            }
            else if ((dragSlot is UI_QuickSlot_Item) == true)   // dragSlot이 같은 QuickSlot에서 온 경우
            {
                ExchangeItem(dragSlot as UI_QuickSlot_Item);
            }
            else
                return;
        }
    }

    public void ChangeItem(UI_Slot_Inven itemSlot)
    {
        if (m_isCoolDown == true || m_anotherSlot.m_isCoolDown == true)
            return;

        if (m_anotherSlot.m_invenSlot == itemSlot)
            m_anotherSlot.ClearSlot();

        m_invenSlot = itemSlot;

        if (m_invenSlot == null)
            Debug.Log(m_invenSlot + " is null");

        SetItem(m_invenSlot.m_itemData, m_invenSlot.m_itemCount);
    }

    private void ExchangeItem(UI_QuickSlot_Item itemQuickSlot)
    {
        if (m_isCoolDown || itemQuickSlot.m_isCoolDown)
            return;

        UI_Slot_Inven temp = this.m_invenSlot;

        SetItem(itemQuickSlot.m_invenSlot.m_itemData, itemQuickSlot.m_invenSlot.m_itemCount);
        m_invenSlot = itemQuickSlot.m_invenSlot;
                
        if (temp != null)
        {
            UI_Drag_Slot.instance.m_slot.GetComponent<UI_QuickSlot_Item>().SetItem(temp.m_itemData, temp.m_itemCount);
            UI_Drag_Slot.instance.m_slot.GetComponent<UI_QuickSlot_Item>().m_invenSlot = temp;
        }
        else
        {
            UI_Drag_Slot.instance.m_slot.GetComponent<UI_QuickSlot_Item>().ClearSlot();
        }
    }

    protected void SetItem(ItemData item, int count)
    {
        if (item.m_itemType != ItemData.ItemType.Consumable)    // 오로지 소모품(Consumable)만
            return;

        BindImage(typeof(Images));
        BindText(typeof(Texts));

        m_itemData = item;
        GetImage((int)Images.Item_Icon).sprite = item.m_icon;
        GetText((int)Texts.Item_Count_Text).text = count.ToString();

        SetColor(1);

        if (count <= 0)
            ClearSlot();
    }

    public override void SetColor(float alpha)
    {
        Color color = GetImage((int)Images.Item_Icon).color;
        color.a = alpha;
        GetImage((int)Images.Item_Icon).color = color;
    }

    public override void ClearSlot()
    {
        GetImage((int)Images.Item_Cooldown_Image).fillAmount = 1;
        GetImage((int)Images.Item_Cooldown_Image).gameObject.SetActive(false);
        GetText((int)Texts.Item_Count_Text).text = "";
        SetColor(0);
        m_invenSlot = null;
        m_itemData = null;
    }    

    private void TakePosition(ConsumableData.ConsumableType type)
    {
        if (type == ConsumableData.ConsumableType.Health)
        {
            GameManager.Inst.m_player.m_stat.Hp += m_data.m_amount;
            if (GameManager.Inst.m_player.m_stat.Hp >= GameManager.Inst.m_player.m_stat.MaxHp)
                GameManager.Inst.m_player.m_stat.Hp = GameManager.Inst.m_player.m_stat.MaxHp;

            Managers.Sound.Play("Player/Heal");
            GameObject obj = Managers.Resource.Instantiate("Effect/Player/Heal");
            obj.transform.position = GameManager.Inst.m_player.transform.position;
            obj.SetActive(true);
        }
        else if(type == ConsumableData.ConsumableType.Mana)
        {
            GameManager.Inst.m_player.m_stat.Mp += m_data.m_amount;
            if (GameManager.Inst.m_player.m_stat.Mp >= GameManager.Inst.m_player.m_stat.MaxMp)
                GameManager.Inst.m_player.m_stat.Mp = GameManager.Inst.m_player.m_stat.MaxMp;

            Managers.Sound.Play("Player/Heal");
            GameObject obj = Managers.Resource.Instantiate("Effect/Player/Mana");
            obj.transform.position = GameManager.Inst.m_player.transform.position;
            obj.SetActive(true);
        }
    }

    private IEnumerator CoolDown()
    {
        m_data = m_itemData as ConsumableData;

        float tick = 1f / m_data.m_coolTime;
        float t = 0;

        m_isCoolDown = true;
        GetImage((int)Images.Item_Cooldown_Image).gameObject.SetActive(true);
        GetImage((int)Images.Item_Cooldown_Image).fillAmount = 1f;
        GetText((int)Texts.Item_Cooldown_Text).text = Mathf.CeilToInt(m_data.m_coolTime).ToString();

        while (GetImage((int)Images.Item_Cooldown_Image).fillAmount > 0f)
        {
            m_time += Time.deltaTime;
            float displayedTime = m_data.m_coolTime - m_time;

            GetImage((int)Images.Item_Cooldown_Image).fillAmount = Mathf.Lerp(1, 0, t);
            t += Time.deltaTime * tick;

            if (displayedTime <= 1f)
            {
                GetText((int)Texts.Item_Cooldown_Text).text = displayedTime.ToString("F1");
            }
            else
            {
                GetText((int)Texts.Item_Cooldown_Text).text = Mathf.CeilToInt(displayedTime).ToString();
            }

            yield return null;
        }

        m_time = 0f;
        m_isCoolDown = false;
        GetText((int)Texts.Item_Cooldown_Text).text = "";
        GetImage((int)Images.Item_Cooldown_Image).gameObject.SetActive(false);
    }
}
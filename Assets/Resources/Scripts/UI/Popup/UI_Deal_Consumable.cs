using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Deal_Consumable : UI_PopUp
{
    enum Images
    {
        Item_Icon,
    }

    enum Texts
    {
        Text_Gold,
        Text_Deal,
        Text_ItemCount,
    }

    enum Buttons
    {
        Button_Confirm,
        Button_Cancel,
        Button_Plus_1,
        Button_Plus_10,
        Button_Minus_1,
        Button_Minus_10,

    }
    private ItemData m_data = null;
    private UI_Slot_Inven m_inven = null;
    [Range(1, 100)]
    private int m_count = 1;
    private bool m_isSell = true;


    public bool m_isActive;

    private void OnEnable()
    {
        m_isActive = true;
    }

    private void OnDisable()
    {
        m_isActive = false;
    }

    public override void Init()
    {
        BindButton(typeof(Buttons));

        GetText((int)Texts.Text_ItemCount).text = "1";
        GetButton((int)Buttons.Button_Plus_1).onClick.AddListener(() => SetItemCount(1));
        GetButton((int)Buttons.Button_Plus_10).onClick.AddListener(() => SetItemCount(10));
        GetButton((int)Buttons.Button_Minus_1).onClick.AddListener(() => SetItemCount(-1));
        GetButton((int)Buttons.Button_Minus_10).onClick.AddListener(() => SetItemCount(-10));
    }

    public void Set_Sell_Item(ItemData data, UI_Slot_Inven invenSlot)
    {
        m_data = data;
        m_inven = invenSlot;
        m_count = 1;
        m_isSell = true;

        BindImage(typeof(Images));
        BindText(typeof(Texts));
        BindButton(typeof(Buttons));

        GetImage((int)Images.Item_Icon).sprite = data.m_icon;
        GetText((int)Texts.Text_Deal).text = "판매 하시겠습니까?";
        GetText((int)Texts.Text_ItemCount).text = string.Format("{0}", m_count);
        GetText((int)Texts.Text_Gold).text = "가격 : " + string.Format("{0:#,###}", data.m_sellPrice * m_count) + "G";

        GetButton((int)Buttons.Button_Confirm).onClick.RemoveAllListeners();
        GetButton((int)Buttons.Button_Confirm).onClick.AddListener(() => Sell_Equipment(data, invenSlot, m_count));
        GetButton((int)Buttons.Button_Cancel).onClick.AddListener(() => GameManager.Inst.m_popup.ClosePopUp(this, false));
    }

    public void Set_Buy_Item(ItemData data)
    {
        m_data = data;
        m_count = 1;
        m_isSell = false;

        BindImage(typeof(Images));
        BindText(typeof(Texts));
        BindButton(typeof(Buttons));

        GetImage((int)Images.Item_Icon).sprite = data.m_icon;
        GetText((int)Texts.Text_Deal).text = "구매 하시겠습니까?";
        GetText((int)Texts.Text_ItemCount).text = string.Format("{0}", m_count);
        GetText((int)Texts.Text_Gold).text = "가격 : " + string.Format("{0:#,###}", data.m_buyPrice * m_count) + "G";

        GetButton((int)Buttons.Button_Confirm).onClick.RemoveAllListeners();
        GetButton((int)Buttons.Button_Confirm).onClick.AddListener(() => Buy_Equipment(data, m_count));
        GetButton((int)Buttons.Button_Cancel).onClick.AddListener(() => GameManager.Inst.m_popup.ClosePopUp(this, false));
    }

    public void Sell_Equipment(ItemData data, UI_Slot_Inven invenSlot, int count = 1)
    {
        GameManager.Inst.m_player.m_stat.Gold += data.m_sellPrice * count;
        if (count == invenSlot.m_itemCount)
            invenSlot.ClearSlot();
        else
        {
            GameManager.Inst.m_popup.m_invenPopUp.GetComponent<Inventory>().AcquireItem(data, -count);
        }

        m_inven = null;

        Managers.Sound.Play("UI/Deal");
        GameManager.Inst.m_popup.ClosePopUp(this, false);
    }

    public void Buy_Equipment(ItemData data, int count = 1)
    {
        if (GameManager.Inst.m_player.m_stat.Gold >= data.m_buyPrice * count)
        {
            GameManager.Inst.m_player.m_stat.Gold -= data.m_buyPrice * count;
            GameManager.Inst.m_popup.m_invenPopUp.GetComponent<Inventory>().AcquireItem(data, count);

            Managers.Sound.Play("UI/Deal");
            GameManager.Inst.m_popup.ClosePopUp(this, false);
        }
        else
            Debug.Log("소지금이 부족합니다!!");
    }

    private void SetItemCount(int count)
    {
        BindText(typeof(Texts));

        if (m_inven != null)
        {
            if (m_count + count >= m_inven.m_itemCount)
            {
                m_count = m_inven.m_itemCount;
                GetText((int)Texts.Text_ItemCount).text = string.Format("{0}", m_count);
                SetItemPrice(m_data, m_count);
                return;
            }
        }
        else
        {
            if (m_count + count <= 0)
            {
                m_count = 1;
                GetText((int)Texts.Text_ItemCount).text = string.Format("{0}", m_count);
                SetItemPrice(m_data, m_count);
                return;
            }
        }

        if (m_count + count >= 100)
        {
            m_count = 99;
            GetText((int)Texts.Text_ItemCount).text = string.Format("{0}", m_count);
            SetItemPrice(m_data, m_count);
            return;
        }

        if (m_count + count <= 0)
        {
            m_count = 1;
            GetText((int)Texts.Text_ItemCount).text = string.Format("{0}", m_count);
            SetItemPrice(m_data, m_count);
            return;
        }

        GetText((int)Texts.Text_ItemCount).text = string.Format("{0}", m_count += count);
        SetItemPrice(m_data, m_count);
    }

    private void SetItemPrice(ItemData data, int count)
    {
        if (m_isSell == true)
        {
            GetText((int)Texts.Text_Gold).text = "가격 : " + string.Format("{0:#,###}", data.m_sellPrice * m_count) + "G";
        }
        else
        {
            GetText((int)Texts.Text_Gold).text = "가격 : " + string.Format("{0:#,###}", data.m_buyPrice * m_count) + "G";
        }
    }
}

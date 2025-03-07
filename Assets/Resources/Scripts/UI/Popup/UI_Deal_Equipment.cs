using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Deal_Equipment : UI_PopUp
{
    enum Images
    {
        Item_Icon,
    }

    enum Texts
    {
        Text_Gold,
        Text_Deal
    }

    enum Buttons
    {
        Button_Confirm,
        Button_Cancel
    }

    public bool m_isActive;

    private void OnEnable()
    {
        m_isActive = true;
    }

    private void OnDisable()
    {
        m_isActive = false;
    }

    public void Set_Sell_Item(ItemData data, UI_Slot_Inven invenSlot)
    {
        BindImage(typeof(Images));
        BindText(typeof(Texts));
        BindButton(typeof(Buttons));

        GetImage((int)Images.Item_Icon).sprite = data.m_icon;
        GetText((int)Texts.Text_Deal).text = "판매 하시겠습니까?";
        GetText((int)Texts.Text_Gold).text = "가격 : " + string.Format("{0:#,###}", data.m_sellPrice) + "G";

        GetButton((int)Buttons.Button_Confirm).onClick.RemoveAllListeners();
        GetButton((int)Buttons.Button_Confirm).onClick.AddListener(() => Sell_Equipment(data, invenSlot));
        GetButton((int)Buttons.Button_Cancel).onClick.AddListener(() => GameManager.Inst.m_popup.ClosePopUp(this, false));
    }

    public void Set_Buy_Item(ItemData data)
    {
        BindImage(typeof(Images));
        BindText(typeof(Texts));
        BindButton(typeof(Buttons));

        GetImage((int)Images.Item_Icon).sprite = data.m_icon;
        GetText((int)Texts.Text_Deal).text = "구매 하시겠습니까?";
        GetText((int)Texts.Text_Gold).text = "가격 : " + string.Format("{0:#,###}", data.m_buyPrice) + "G";

        GetButton((int)Buttons.Button_Confirm).onClick.RemoveAllListeners();
        GetButton((int)Buttons.Button_Confirm).onClick.AddListener(() => Buy_Equipment(data));
        GetButton((int)Buttons.Button_Cancel).onClick.AddListener(() => GameManager.Inst.m_popup.ClosePopUp(this, false));
    }

    public void Sell_Equipment(ItemData data, UI_Slot_Inven invenSlot)
    {
        GameManager.Inst.m_player.m_stat.Gold += data.m_sellPrice;
        invenSlot.ClearSlot();

        Managers.Sound.Play("UI/Deal");
        GameManager.Inst.m_popup.ClosePopUp(this, false);
    }

    public void Buy_Equipment(ItemData data)
    {
        if (GameManager.Inst.m_player.m_stat.Gold >= data.m_buyPrice)
        {
            GameManager.Inst.m_player.m_stat.Gold -= data.m_buyPrice;
            GameManager.Inst.m_popup.m_invenPopUp.GetComponent<Inventory>().AcquireItem(data);

            Managers.Sound.Play("UI/Deal");
            GameManager.Inst.m_popup.ClosePopUp(this, false);
        }
        else
            Debug.Log("소지금이 부족합니다!!");
    }
}

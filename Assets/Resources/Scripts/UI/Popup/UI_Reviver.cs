using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Reviver : UI_PopUp
{
    enum Texts
    {
        Text_CautionMessage
    }

    enum Buttons
    {
        Button_ReviveCurrentPos,
        Button_ReviveVillage
    }

    private void OnEnable()
    {
        BindButton(typeof(Buttons));
        BindText(typeof(Texts));

        if (GameManager.Inst.m_player.m_stat.Gold < 1000)
        {
            GetButton((int)Buttons.Button_ReviveCurrentPos).interactable = false;
            GetText((int)Texts.Text_CautionMessage).text = "소지 금액 부족";
        }
        else
        {
            GetButton((int)Buttons.Button_ReviveCurrentPos).interactable = true;
            GetText((int)Texts.Text_CautionMessage).text = "부활 시 1000G 소모";
        }

        GetButton((int)Buttons.Button_ReviveCurrentPos).onClick.RemoveAllListeners();
        GetButton((int)Buttons.Button_ReviveCurrentPos).onClick.AddListener(() => GameManager.Inst.ReviveCurrentPos());
        GetButton((int)Buttons.Button_ReviveVillage).onClick.RemoveAllListeners();
        GetButton((int)Buttons.Button_ReviveVillage).onClick.AddListener(() => GameManager.Inst.ReviveVillage());
    }
}
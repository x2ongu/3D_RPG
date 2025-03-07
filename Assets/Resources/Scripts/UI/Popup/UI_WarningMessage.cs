using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_WarningMessage : UI_PopUp
{
    enum Buttons
    {
        Button_Yes,
        Button_No
    }

    enum Texts
    {
        Text_Massage,
    }

    public event Action m_buttonYes;
    public string m_message;

    private void OnEnable()
    {
        BindText(typeof(Texts));
        GetText((int)Texts.Text_Massage).text = m_message;

        BindButton(typeof(Buttons));
        GetButton((int)Buttons.Button_Yes).onClick.RemoveAllListeners();
        GetButton((int)Buttons.Button_Yes).onClick.AddListener(Button_Yes);
        GetButton((int)Buttons.Button_No).onClick.RemoveAllListeners();
        GetButton((int)Buttons.Button_No).onClick.AddListener(Button_No);
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Return))
        {
            Button_Yes();
        }
    }

    public void Button_Yes()
    {
        m_buttonYes?.Invoke();
    }

    public void Button_No()
    {
        GameManager.Inst.m_popup.ClosePopUp(this, false);
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_EndGame : UI_PopUp
{
    enum Buttons
    {
        Button_Save,
        Button_Quit
    }

    UI_WarningMessage m_warningMessage;

    private void OnEnable()
    {
        m_warningMessage = GameManager.Inst.m_popup.m_warningMessage.GetComponent<UI_WarningMessage>();

        BindButton(typeof(Buttons));

        GetButton((int)Buttons.Button_Save).onClick.RemoveAllListeners();
        GetButton((int)Buttons.Button_Quit).onClick.RemoveAllListeners();

        GetButton((int)Buttons.Button_Save).onClick.AddListener(Button_Save);
        GetButton((int)Buttons.Button_Quit).onClick.AddListener(Button_Quit);
    }

    public void Button_Save()
    {
        StartCoroutine(Managers.Data.SaveDataCoroutine());
    }

    public void Button_Quit()
    {
        m_warningMessage.m_message = "게임을 종료하시겠습니까?";
        m_warningMessage.m_buttonYes -= QuitGame;
        m_warningMessage.m_buttonYes += QuitGame;
        GameManager.Inst.m_popup.OpenPopUp(GameManager.Inst.m_popup.m_warningMessage, false);
    }

    private void QuitGame()
    {
        Application.Quit();
        //UnityEditor.EditorApplication.isPlaying = false;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopUpManager : MonoBehaviour
{
    public UI_PopUp m_invenPopUp;
    public UI_PopUp m_skillPopUp;
    public UI_PopUp m_statsPopUp;
    public UI_PopUp m_questPopup;
    public UI_PopUp m_equipmentShop;
    public UI_PopUp m_consumableShop;
    public UI_PopUp m_deal_equipment;
    public UI_PopUp m_deal_consumable;
    public UI_PopUp m_reviver;
    public UI_PopUp m_dialogue;
    public UI_PopUp m_endGame;
    public UI_PopUp m_warningMessage;
    

    [HideInInspector]
    public UI_PopUp m_objWithDepth;

    public GameObject m_blocker;

    [Space]
    public KeyCode m_escapeKey = KeyCode.Escape;
    public KeyCode m_inventoryKey = KeyCode.I;
    public KeyCode m_skillKey = KeyCode.K;
    public KeyCode m_statsKey = KeyCode.P;
    public KeyCode m_questKey = KeyCode.F1;
    public KeyCode m_endGameKey = KeyCode.Escape;

    // LinkedList는 List와 달리
    private LinkedList<UI_PopUp> m_activePopupLList;    // 실시간 PopUp 관리 LinkedList
    private List<UI_PopUp> m_allPopupList;              // 전체 PopUp 목록

    [HideInInspector]
    public bool m_isShopOpen = false;
    [HideInInspector]
    public bool m_isOptionOpen = false;
    [HideInInspector]
    public bool m_isObjectOpenThatDontHaveDepth = false;

    private void Awake()
    {
        m_invenPopUp.GetComponent<Inventory>().SetSlot();

        m_activePopupLList = new LinkedList<UI_PopUp>();
        Init();
        InitCloseAll();
    }

    private void Init()
    {
        m_allPopupList = new List<UI_PopUp>()
        {
            m_invenPopUp, m_skillPopUp, m_statsPopUp, m_questPopup, m_equipmentShop, m_consumableShop
        };

        m_equipmentShop.m_closeButton = m_invenPopUp.m_closeButton;
        m_consumableShop.m_closeButton = m_invenPopUp.m_closeButton;

        foreach (var popUp in m_allPopupList)
        {
            popUp.OnFocus += () =>
            {
                m_activePopupLList.Remove(popUp);
                m_activePopupLList.AddFirst(popUp);
                RefreshAllPopupDepth();
            };

            popUp.m_closeButton.onClick.AddListener(() => ClosePopUp(popUp));
        }

        m_blocker.SetActive(false);
    }

    public void UI_Controller()
    {
        if (m_isShopOpen == true && m_isObjectOpenThatDontHaveDepth == false)
        {
            if (Input.GetKeyDown(m_escapeKey))
            {
                InitCloseAll();
                m_isShopOpen = false;
            }

            return;
        }

        if (Input.GetKeyDown(m_escapeKey))
        {
            if (m_isObjectOpenThatDontHaveDepth == true)
            {
                ClosePopUp(m_objWithDepth, false);
            }
            else
            {
                if (m_activePopupLList.Count > 0)
                {
                    ClosePopUp(m_activePopupLList.First.Value);
                }
                else
                {
                    ToggleKeyDownAction(m_endGameKey, m_endGame, false);
                }
            }
        }

        // 단축키 조작
        if (m_isShopOpen == false && m_isOptionOpen == false && m_isObjectOpenThatDontHaveDepth == false)
        {
            ToggleKeyDownAction(m_inventoryKey, m_invenPopUp);
            ToggleKeyDownAction(m_skillKey, m_skillPopUp);
            ToggleKeyDownAction(m_statsKey, m_statsPopUp);
            ToggleKeyDownAction(m_questKey, m_questPopup);
        }
    }
    
    public void OpenCloseShop(UI_PopUp shop)
    {
        if (m_isShopOpen == true)
        {
            if (shop == m_equipmentShop && m_isShopOpen == true)
            {
                ToggleOpenClosePopup(m_equipmentShop);
                ToggleOpenClosePopup(m_invenPopUp);
            }
            else if (shop == m_consumableShop && m_isShopOpen == true)
            {
                ToggleOpenClosePopup(m_consumableShop);
                ToggleOpenClosePopup(m_invenPopUp);
            }

            m_isShopOpen = false;
        }
        else
        {
            if (shop == m_equipmentShop && m_isShopOpen == false)
            {
                ToggleOpenClosePopup(m_equipmentShop);
                ToggleOpenClosePopup(m_invenPopUp);
            }
            else if (shop == m_consumableShop && m_isShopOpen == false)
            {
                ToggleOpenClosePopup(m_consumableShop);
                ToggleOpenClosePopup(m_invenPopUp);
            }

            m_isShopOpen = true;
        }
    }

    public void OpenPopUp(UI_PopUp popUp, bool doseThisHaveDepth = true)
    {
        if (doseThisHaveDepth == true)
        {
            m_activePopupLList.AddFirst(popUp);
            popUp.gameObject.SetActive(true);
            if (popUp == m_equipmentShop || popUp == m_consumableShop)
                m_blocker.SetActive(true);
            popUp.m_base.transform.position = popUp.m_origin;
            RefreshAllPopupDepth();
        }
        else
        {
            popUp.OnFocus += () => { };
            m_isObjectOpenThatDontHaveDepth = true;
            m_objWithDepth = popUp;

            popUp.gameObject.SetActive(true);
            popUp.m_base.transform.position = popUp.m_origin;
            if (popUp.m_closeButton != null)
                popUp.m_closeButton.onClick.AddListener(() => ClosePopUp(popUp, false));
        }
    }

    public void ClosePopUp(UI_PopUp popUp, bool isThisInPopupList = true)
    {
        if (isThisInPopupList == true)
        {
            if (m_isShopOpen == true)
                m_isShopOpen = false;

            m_activePopupLList.Remove(popUp);
            popUp.gameObject.SetActive(false);
            if (popUp == m_equipmentShop || popUp == m_consumableShop)
                m_blocker.SetActive(false);
            RefreshAllPopupDepth();
        }
        else
        {
            m_isObjectOpenThatDontHaveDepth = false;
            m_objWithDepth = null;

            popUp.gameObject.SetActive(false);
        }
        
    }

    #region 메뉴 UI에 있는 버튼을 누를 때 사용
    public void TryOpenInven()
    {
        ToggleOpenClosePopup(m_invenPopUp);
    }

    public void TryOpenSkill()
    {
        ToggleOpenClosePopup(m_skillPopUp);
    }

    public void TryOpenStats()
    {
        ToggleOpenClosePopup(m_statsPopUp);
    }

    public void TryOpenQuest()
    {
        ToggleOpenClosePopup(m_questPopup);
    }

    public void TryOpenEndGame()
    {
        ToggleOpenClosePopup(m_endGame);
    }
    #endregion

    private void InitCloseAll()
    {
        foreach (var popUp in m_allPopupList)
        {
            ClosePopUp(popUp);
        }
    }

    private void ToggleKeyDownAction(in KeyCode key, UI_PopUp popUp, bool isThisInPopupList = true)
    {
        if (Input.GetKeyDown(key))
            ToggleOpenClosePopup(popUp, isThisInPopupList);
    }

    /// <summary> 팝업의 상태(opened/closed)에 따라 열거나 닫기 </summary>
    private void ToggleOpenClosePopup(UI_PopUp popUp, bool isThisInPopupList = true)
    {
        if (!popUp.gameObject.activeSelf)
            OpenPopUp(popUp, isThisInPopupList);
        else
            ClosePopUp(popUp, isThisInPopupList);
    }    

    private void RefreshAllPopupDepth()
    {
        foreach (var popUp in m_activePopupLList)
        {
            popUp.transform.SetAsFirstSibling();
        }
    }
}

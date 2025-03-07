using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipmentMerchant : NPC
{
    public GameObject m_popupManager;
    PopUpManager m_popup;

    public override void Init()
    {
        base.Init();

        m_popup = m_popupManager.GetComponent<PopUpManager>();
    }

    public override void DoInteraction(bool questNPC)
    {
        if (m_popup.m_isShopOpen == false && m_popup.m_isObjectOpenThatDontHaveDepth == false)
            m_popup.OpenCloseShop(m_popup.m_equipmentShop);
    }
}
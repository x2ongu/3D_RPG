using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainQuestNPC : NPC
{
    public override void DoInteraction(bool questNPC)
    {
        if (questNPC == true)
        {
            GameManager.Inst.m_popup.OpenPopUp(GameManager.Inst.m_popup.m_dialogue, false);
        }
        else
        {

        }
    }
}
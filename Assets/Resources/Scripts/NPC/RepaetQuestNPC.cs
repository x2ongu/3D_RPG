using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RepaetQuestNPC : NPC
{
    public override void DoInteraction(bool questNPC)
    {
        Debug.Log("반복퀘스트 입니다.");
    }
}
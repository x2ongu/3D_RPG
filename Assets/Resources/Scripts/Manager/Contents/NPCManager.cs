using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCManager
{
    private static Dictionary<string, GameObject> m_npcDict = new Dictionary<string, GameObject>();

    public void RegisterNPC(GameObject npc)
    {
        if (!m_npcDict.ContainsKey(npc.name))
            m_npcDict.Add(npc.name, npc);
    }

    public GameObject GetNPC(string npcName)
    {
        m_npcDict.TryGetValue(npcName, out var npc);
        return npc;
    }
}

using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InteractionManager : MonoBehaviour
{
    private GameObject m_nearestNPC;
    private GameObject m_nearestItem;

    public GameObject NearestNPC { get { return m_nearestNPC; } set { m_nearestNPC = value; } }
    public GameObject NearestItem { get { return m_nearestItem; } set { m_nearestItem = value; } }

    [SerializeField]
    private TextMeshProUGUI m_interactText;
    [SerializeField]
    private TextMeshProUGUI m_itemText;
    [SerializeField]
    private float m_npcInterRad = 3.5f;    // NPC Interaction Radius
    [SerializeField]
    private float m_itemInterRad = 2f;   // Item Interaction Radius
    [SerializeField]
    private Inventory m_inven;

    private string m_npcInterTag = "NPC";    // NPC Interaction Tag
    private string m_itemInterTag = "Item";  // Item Interaction Tag

    private void Update()
    {
        CheckNPC();
        CheckItem();
    }

    public void ReturnSelectedNPC()
    {
        if (m_nearestNPC == null)
            return;

        GameManager.Inst.m_quest.SetQuestNPC();

        m_nearestNPC.GetComponent<INPC>().DoInteraction();
    }

    public void GetItem()
    {
        if (m_nearestItem == null)
            return;

        m_inven.AcquireItem(m_nearestItem.GetComponent<Item>().m_itemData);
        m_nearestItem.gameObject.SetActive(false);
        m_nearestItem = null;

        GameManager.Inst.m_quest.CollectQuestProcess();
    }

    private void CheckNPC()
    {
        if (FindNearestObj(m_npcInterRad, m_npcInterTag) != null)
        {
            m_nearestNPC = FindNearestObj(m_npcInterRad, m_npcInterTag);
            m_interactText.gameObject.SetActive(true);
            m_interactText.text = "<color=yellow>" + "(G)" + "</color>" + "≈∞∑Œ ªÛ»£¿€øÎ";            
        }
        else
        {
            m_nearestNPC = null;
            m_interactText.gameObject.SetActive(false);
        }
    }

    private void CheckItem()
    {
        if (FindNearestObj(m_itemInterRad, m_itemInterTag) != null)
        {
            m_nearestItem = FindNearestObj(m_itemInterRad, m_itemInterTag);
            m_itemText.gameObject.SetActive(true);
            m_itemText.text = m_nearestItem.GetComponent<Item>().m_itemData.m_itemName + " »πµÊ " + "<color=yellow>" + "(Space Bar)" + "</color>";
        }
        else
        {
            m_itemText.gameObject.SetActive(false);
        }
    }

    private GameObject FindNearestObj(float range, string tag)
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, range);

        if (colliders.Length > 0)
        {
            GameObject nearestObj = null;
            float nearestDist = 100f;

            foreach (Collider coll in colliders)
            {
                if (coll.CompareTag(tag))
                {
                    float dist = Vector3.Distance(transform.position, coll.transform.position);

                    if (dist < nearestDist)
                    {
                        nearestDist = dist;
                        nearestObj = coll.gameObject;

                        if (Vector3.Distance(transform.position, nearestObj.transform.position) >= range)
                            nearestObj = null;

                        return nearestObj;
                    }
                }
            }
        }

        return null;
    }
}

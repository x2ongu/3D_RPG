using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Gold", menuName = "Items/Gold")]
public class GoldData : ItemData
{
    public int m_minGold;
    public int m_maxGold;
    private int m_gold;

    public override void WhenTheItemDrops()
    {
        m_gold = Random.Range(m_minGold, m_maxGold);
        Debug.Log(m_itemName + " " + m_gold + "∏¶ »πµÊ«œºÃΩ¿¥œ¥Ÿ!");
    }
}

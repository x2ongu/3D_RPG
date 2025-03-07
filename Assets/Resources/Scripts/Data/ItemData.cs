using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = "Item")]
public class ItemData : ScriptableObject
{
    public string m_itemName;

    public ItemType m_itemType;

    public Sprite m_icon;
    public GameObject m_itemPrefab;

    [TextArea]
    public string m_itemDesc;

    public enum ItemType { Equipment, Consumable, ETC, Gold }

    [Range(0, 100)]
    public float m_dropRate;

    public int m_sellPrice;

    public int m_buyPrice;

    public virtual void WhenTheItemDrops()
    {

    }
}

public class ConsumableData : ItemData
{
    // 포션 : 체력, 마나 / 버프 : 공격력, 방어력, 이동속도
    public enum ConsumableType { Health, Mana }
    public ConsumableType m_consumType;

    public float m_coolTime;

    public int m_amount;
}
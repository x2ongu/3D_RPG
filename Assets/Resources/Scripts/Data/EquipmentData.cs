using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "Equipment", menuName = "Items/Equipment")]
public class EquipmentData : ItemData
{
    public enum EquipmentType { Helmet, Pants, Top, Weapon }
    public EquipmentType m_equipmentType;

    [HideInInspector]
    public bool m_doseItemSet = false;

    public override void WhenTheItemDrops()
    {
        Debug.Log(m_itemName + "¿ª »πµÊ«œºÃΩ¿¥œ¥Ÿ!");
    }

    public virtual void SetItem() { }

    public virtual void ClearItem() { }
}

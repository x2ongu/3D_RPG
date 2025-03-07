using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CopyPlayer : MonoBehaviour
{
    public Transform m_weaponBackPos;

    public GameObject m_weapon;

    public void SetWeapon(GameObject weapon)
    {
        if (weapon == null)
            return;

        m_weapon = Instantiate(weapon);

        m_weapon.transform.SetParent(m_weaponBackPos);
        m_weapon.transform.localPosition = Vector3.zero;
        m_weapon.transform.localRotation = Quaternion.Euler(0, -90, 0);
    }
}
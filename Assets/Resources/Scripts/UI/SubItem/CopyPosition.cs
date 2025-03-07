using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CopyPosition : MonoBehaviour
{
    [SerializeField]
    private bool x, y, z;
    [SerializeField]
    private Transform m_target;

    void Update()
    {
        if (!m_target)
            return;

        transform.position = new Vector3(
            (x ? m_target.position.x : transform.position.x),
            (y ? m_target.position.y : transform.position.y),
            (z ? m_target.position.z : transform.position.z));
    }
}

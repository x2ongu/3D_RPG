using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TargeterType { Player, Enemy }

public class TargetManager : MonoBehaviour
{
    [Header("[ 공격 대상 리스트 ]"), SerializeField]
    List<Collider> m_targetList = new List<Collider>();
    public List<Collider> TargetList { get { return m_targetList; } }

    public TargeterType m_targeter;

    private void OnTriggerEnter(Collider other)
    {
        if (m_targeter == TargeterType.Player)
        {
            if (other.gameObject.CompareTag("Enemy"))
                m_targetList.Add(other);
        }
        else if (m_targeter == TargeterType.Enemy)
        {
            if (other.gameObject.CompareTag("Player"))
                m_targetList.Add(other);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (m_targeter == TargeterType.Player)
        {
            if (other.gameObject.CompareTag("Enemy"))
            {
                m_targetList.Remove(other);
            }
        }
        else if (m_targeter == TargeterType.Enemy)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                m_targetList.Remove(other);
            }
        }
    }

    public void RemoveTarget(Collider coll)
    {
        if (m_targeter == TargeterType.Player)
        {
            if (coll.gameObject.CompareTag("Enemy"))
            {
                if (m_targetList.Contains(coll))
                    m_targetList.Remove(coll);
            }
        }
        else if (m_targeter == TargeterType.Enemy)
        {
            if (m_targetList.Contains(coll))
                m_targetList.Remove(coll);
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Cinemachine;

public class Player : MonoBehaviour
{
    #region Class Variable    
    [HideInInspector]
    public AnimationEvent m_animEvent;
    [HideInInspector]
    public InteractionManager m_interManager;
    [HideInInspector]
    public NavMeshAgent m_navAgent;
    [HideInInspector]
    public Camera m_mainCam;
    [HideInInspector]
    public PlayerStat m_stat;
    #endregion

    public TargetManager[] m_targeters;

    public GameObject m_weapon;

    public Transform m_weaponHandPos;
    public Transform m_weaponBackPos;

    [HideInInspector]
    public bool m_inItWeapon = false;
    [HideInInspector]
    public bool m_isAttackMode = false;
    [HideInInspector]
    public bool m_isLive = true;

    private void Awake()
    {
        #region GetComPonent
        m_animEvent = GetComponentInChildren<AnimationEvent>();
        m_interManager = GetComponent<InteractionManager>();
        m_navAgent = GetComponent<NavMeshAgent>();
        m_mainCam = Camera.main;
        m_stat = GetComponent<PlayerStat>();
        #endregion

        m_stat.OnStart();
        m_navAgent.speed = m_stat.MoveSpeed;
    }

    private void Update()
    {
        m_navAgent.speed = m_stat.MoveSpeed;

        if (m_stat.Hp >= m_stat.MaxHp)
            m_stat.Hp = m_stat.MaxHp;

        if (m_animEvent.m_isAttackPlaying == true)
        {
            NavMeshHit hit;
            if (!NavMesh.SamplePosition(transform.position, out hit, 1.0f, NavMesh.AllAreas))
            {
                // NavMesh 영역 밖에 있는 경우 가장 가까운 유효한 NavMesh 위치로 조정
                if (NavMesh.FindClosestEdge(transform.position, out hit, NavMesh.AllAreas))
                {
                    transform.position = hit.position;
                }
            }
        }
    }

    private void LateUpdate()
    {
        if (m_animEvent.m_isMove && m_isLive)
        {
            if (m_navAgent.remainingDistance <= m_navAgent.stoppingDistance)
                m_animEvent.m_anim.SetFloat("Speed", 0f);
        }
    }

    public void SetWeaponPosition(bool isReadyToAttack)
    {
        if (m_weapon == null)
            return;

        if (isReadyToAttack == true)
        {
            m_weapon.transform.SetParent(m_weaponHandPos);
            m_weapon.transform.localPosition = Vector3.zero;
            m_weapon.transform.localRotation = Quaternion.Euler(0, -90, 0);
        }
        else
        {
            m_weapon.transform.SetParent(m_weaponBackPos);
            m_weapon.transform.localPosition = Vector3.zero;
            m_weapon.transform.localRotation = Quaternion.Euler(0, -90, 0);
            m_isAttackMode = false;
        }
    }
}
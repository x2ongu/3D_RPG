using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class StateManager : FSM<StateManager>
{
    public Animator m_anim;
    public Transform m_target;
    public Transform m_spawnPoint;
    public NavMeshAgent m_navEnemy;
    public EnemyStat m_stat;
    NavMeshPath m_path;

    public IEnemy m_iEnemy;

    public Quaternion m_initialRot;

    [HideInInspector]
    public Enemy m_enemy;
    [HideInInspector]
    public float m_returnSpeed = 18f;   // Return Speed to Spawn Point
    [HideInInspector]
    public float m_searchRange = 25f;
    [HideInInspector]
    public float m_traceRange = 40f;

    [HideInInspector]
    public bool m_isDead = false;

    private void Awake()
    {
        m_enemy = GetComponent<Enemy>();
        m_anim = GetComponentInChildren<Animator>();
        m_navEnemy = GetComponent<NavMeshAgent>();
        m_iEnemy = GetComponent<IEnemy>();
        m_stat = GetComponent<EnemyStat>();
        m_path = new NavMeshPath();
        m_initialRot = transform.rotation;
    }

    private void Start()    { InitState(this, StateStay.Instance); }

    private void Update()    { FSMUpdate(); }

    public bool IsCloseToTarget(Vector3 targetPos, float range)
    {
        float dist = Vector3.SqrMagnitude(transform.position - targetPos);

        if (dist < range * range)
            return true;

        return false;
    }

    public bool IsFacingTarget(Vector3 targetPos, float maxAngle)
    {
        Vector3 dirToPlayer = targetPos - m_navEnemy.transform.position;
        Vector3 forwardDir = m_navEnemy.transform.forward;

        float angle = Vector3.Angle(dirToPlayer, forwardDir);

        if (angle <= maxAngle)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void Move(Vector3 targetPos)
    {
        if (m_isDead)
            return;

        m_navEnemy.SetDestination(targetPos);
        m_navEnemy.speed = m_stat.MoveSpeed;
        m_navEnemy.stoppingDistance = m_stat.AttackRange;
    }

    public void MoveReturn(Vector3 targetPos)
    {
        if (m_isDead)
            return;

        m_navEnemy.SetDestination(targetPos);
        m_navEnemy.speed = m_returnSpeed;
        m_navEnemy.stoppingDistance = 0f;
    }

    public void WatchToTarget(Vector3 target)
    {
        if (m_isDead)
            return;

        Quaternion lookRotation = Quaternion.LookRotation(target - m_navEnemy.transform.position);
        m_navEnemy.transform.rotation = Quaternion.Lerp(m_navEnemy.transform.rotation, lookRotation, Time.deltaTime * 4f);
    }

    public float GetRemainingDistance(Transform trans)
    {
        m_navEnemy.CalculatePath(trans.position, m_path);

        float remaningDist = 0f;
        for (int i = 0; i < m_path.corners.Length - 1; i++)
        {
            remaningDist += Vector3.Distance(m_path.corners[i], m_path.corners[i + 1]);
        }

        return remaningDist;
    }
    
    public IEnumerator SetRotation()
    {
        yield return new WaitForSeconds(0.2f);
        transform.rotation = m_initialRot;
    }
}
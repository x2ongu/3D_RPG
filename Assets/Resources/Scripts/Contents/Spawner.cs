using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Spawner : MonoBehaviour
{
    [SerializeField]
    public GameObject m_enemy;

    [SerializeField]
    private Transform[] m_spawnPoints;
    public List<Transform> m_spawnPointList = new List<Transform>();

    private void Awake()
    {
        m_spawnPoints = GetComponentsInChildren<Transform>();
        m_spawnPointList.AddRange(m_spawnPoints);
    }

    private void Start()
    {
        Spawn(m_spawnPoints.Length - 1);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            if (m_spawnPointList.Count == 0)
            {
                Debug.Log("No available spawn points.");
                return;
            }
        }
    }

    void Spawn()
    {
        GameObject enemy = Managers.Resource.Instantiate($"Enemy/{m_enemy.name}");

        enemy.SetActive(true);

        enemy.GetComponent<NavMeshAgent>().enabled = false;

        int num = Random.Range(1, m_spawnPointList.Count);

        Transform spawnPoint = m_spawnPointList[num];

        enemy.transform.position = spawnPoint.position;

        enemy.GetComponent<StateManager>().m_spawnPoint = spawnPoint;

        enemy.GetComponent<StateManager>().m_target = GameManager.Inst.m_player.transform;

        enemy.GetComponent<StateManager>().m_initialRot = spawnPoint.rotation;

        enemy.GetComponent<NavMeshAgent>().enabled = true;

        m_spawnPointList.RemoveAt(num);
    }

    void Spawn(int count)
    {
        for (int i = 0; i < count; i++)
        {
            Spawn();
        }
    }
}
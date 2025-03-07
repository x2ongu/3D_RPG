using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private Rigidbody m_rigid;
    public int m_attack;
    public float m_speed = 15f;

    private void Awake()
    {
        m_rigid = GetComponent<Rigidbody>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            gameObject.SetActive(false);
            GameManager.Inst.m_player.m_stat.TakeDamage(m_attack);
        }
    }

    public void Shoot()
    {
        m_rigid.velocity = transform.up * m_speed;
        StartCoroutine(PoolProjectile());
    }

    IEnumerator PoolProjectile()
    {
        yield return new WaitForSeconds(2f);
        Managers.Resource.Destroy(gameObject);
    }
}
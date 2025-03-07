using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public ItemData m_itemData;
    private Rigidbody m_rigid;

    public Vector2 m_dropRange = new Vector2(5f, 5f);
    public float m_gravity = -30f;
    public float m_parabolaHeight = 10f;

    private Vector3 m_dropPos;

    private void OnEnable()
    {
        m_rigid = GetComponent<Rigidbody>();
        if (m_rigid == null)
        {
            m_rigid = gameObject.AddComponent<Rigidbody>();
        }

        m_dropPos = transform.position + new Vector3(Random.Range(-m_dropRange.x, m_dropRange.x), 0f, Random.Range(-m_dropRange.y, m_dropRange.y));

        m_rigid.velocity = Parabola();
        StartCoroutine(PoolItem());
    }

    private void FixedUpdate()
    {
        m_rigid.AddForce(Vector3.up * m_gravity, ForceMode.Acceleration);
    }

    private Vector3 Parabola()
    {
        Vector3 dir = m_dropPos - transform.position;
        dir.y = 0f;

        float initialVelocityY = Mathf.Sqrt(-2 * m_gravity * m_parabolaHeight);
        float initialVelocityXZ = dir.magnitude / (Mathf.Sqrt(-2 * m_parabolaHeight / m_gravity) + Mathf.Sqrt(2 * (dir.y - m_parabolaHeight) / m_gravity));

        Vector3 initialVelocity = dir.normalized * initialVelocityXZ;
        initialVelocity.y = initialVelocityY;

        return initialVelocity;
    }

    IEnumerator PoolItem()
    {
        yield return new WaitForSeconds(15f);
        Managers.Resource.Destroy(gameObject);
    }
}
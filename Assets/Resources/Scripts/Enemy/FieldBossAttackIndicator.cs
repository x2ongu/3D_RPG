using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldBossAttackIndicator : MonoBehaviour
{
    [Header("# Jump Attack Area")]
    public Projector m_jumpAttack;
    float m_jumpAttackRate = 1.6f;

    [Header("# Downward Area")]
    public Projector m_downwardLeft;
    public Projector m_downwardRight;
    float m_downwardRate = 1f;

    [Header("# Roaring Area")]
    public Projector[] m_roarings;
    float m_roaringRate = 2.7f;

    public void StartJumpAttack()
    {
        StartCoroutine(JumpAttack());

        IEnumerator JumpAttack()
        {
            float runningTime = m_jumpAttackRate;

            m_jumpAttack.gameObject.SetActive(true);
            m_jumpAttack.orthographicSize = 0.01f;

            while (runningTime > 0f)
            {
                runningTime -= Time.deltaTime;
                m_jumpAttack.orthographicSize += 8.2f / m_jumpAttackRate * Time.deltaTime;
                yield return null;
            }

            m_jumpAttack.gameObject.SetActive(false);
        }
    }

    public void StartDownwardLeftAttack()
    {
        StartCoroutine(DownwardLeft());

        IEnumerator DownwardLeft()
        {
            float runningTime = m_downwardRate;

            m_downwardLeft.gameObject.SetActive(true);
            m_downwardLeft.aspectRatio = 0.01f;

            while (runningTime > 0f)
            {
                runningTime -= Time.deltaTime;
                m_downwardLeft.aspectRatio += 3f / m_downwardRate * Time.deltaTime;
                yield return null;
            }

            m_downwardLeft.gameObject.SetActive(false);
        }
    }

    public void StartDownwardRightAttack()
    {
        StartCoroutine(DownwardRight());

        IEnumerator DownwardRight()
        {
            float runningTime = m_downwardRate;

            m_downwardRight.gameObject.SetActive(true);
            m_downwardRight.aspectRatio = 0.01f;

            while (runningTime > 0f)
            {
                runningTime -= Time.deltaTime;
                m_downwardRight.aspectRatio += 3.5f / m_downwardRate * Time.deltaTime;
                yield return null;
            }

            m_downwardRight.gameObject.SetActive(false);
        }
    }

    public void StartRoaring()
    {
        StartCoroutine(Roaring());
        
        IEnumerator Roaring()
        {
            float runningTime = m_roaringRate;

            for (int i = 0; i < m_roarings.Length; i++)
            {
                m_roarings[i].gameObject.SetActive(true);
                m_roarings[i].orthographicSize = 0.01f;
            }

            while (runningTime > 0f)
            {
                runningTime -= Time.deltaTime;

                for (int i = 0; i < m_roarings.Length; i++)
                {
                    m_roarings[i].orthographicSize += 10f / m_roaringRate * Time.deltaTime;
                }

                yield return null;
            }

            for (int i = 0; i < m_roarings.Length; i++)
            {
                m_roarings[i].gameObject.SetActive(false);
            }
        }        
    }    
}

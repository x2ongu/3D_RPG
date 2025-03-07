using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UI_PlayerDamage : MonoBehaviour
{
    Stat m_stat;
    TextMeshProUGUI m_text;
    Color m_alpha;

    private void OnEnable()
    {
        Init();
        StartCoroutine(SetActiveFalse());
    }

    private void Update()
    {
        transform.Translate(Vector3.up * 0.02f);
        m_alpha.a = Mathf.Lerp(m_alpha.a, 0f, Time.deltaTime * 0.7f);
        m_text.color = m_alpha;
        transform.rotation = Camera.main.transform.rotation;
    }

    public void Init()
    {
        m_text = GetComponentInChildren<TextMeshProUGUI>();
        m_alpha = m_text.color;
        m_alpha.a = 1;

        m_alpha = Color.red;
    }

    public void SetTransform(GameObject obj)
    {
        m_stat = obj.GetComponent<Stat>();

        transform.position = obj.transform.position + (Vector3.up * 5f);

        m_text.text = m_stat.m_damage.ToString();
    }

    IEnumerator SetActiveFalse()
    {
        yield return new WaitForSeconds(0.7f);
        gameObject.SetActive(false);
    }
}
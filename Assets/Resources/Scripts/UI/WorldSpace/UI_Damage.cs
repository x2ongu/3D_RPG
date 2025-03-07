using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UI_Damage : MonoBehaviour
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
        m_alpha.a = 1;
    }

    public void SetTransform(GameObject obj)
    {
        m_stat = obj.GetComponent<Stat>();

        transform.position = obj.transform.position + Vector3.up * (obj.GetComponent<Collider>().bounds.size.y + 0.5f);

        m_text.text = m_stat.m_damage.ToString();
    }

    public void SetColorByCritical(bool isCritical)
    {
        if (isCritical == true)
            m_alpha = Color.yellow;
        else
            m_alpha = Color.white;
    }

    public void Bloodthirster(int damage)
    {
        transform.position = GameManager.Inst.m_player.transform.position + Vector3.up * 4f;
        m_text.text = damage.ToString();
        m_alpha = Color.green;
    }

    IEnumerator SetActiveFalse()
    {
        yield return new WaitForSeconds(0.7f);
        gameObject.SetActive(false);
    }
}
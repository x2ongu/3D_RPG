using System.Collections;
using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UI_PopUp : UI_Base, IPointerDownHandler
{
    [Header("# UI")]
    public GameObject m_base;
    public Button m_closeButton;
    public event Action OnFocus;

    [HideInInspector]
    public Vector3 m_origin;

    private void Awake()
    {
        m_origin = m_base.transform.position;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        OnFocus();
    }
}
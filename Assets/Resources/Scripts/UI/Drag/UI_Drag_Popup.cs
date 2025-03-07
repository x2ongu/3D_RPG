using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UI_DragBar : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [SerializeField]
    private GameObject m_popupBase;

    private Vector3 m_offset;

    public void OnBeginDrag(PointerEventData eventData)
    {
        m_offset = m_popupBase.transform.position - (Vector3)eventData.position;
    }

    public void OnDrag(PointerEventData eventData)
    {
        m_popupBase.transform.position = (Vector3)eventData.position + m_offset;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        m_popupBase.transform.position = (Vector3)eventData.position + m_offset;
    }
}

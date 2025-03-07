using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Drag_Slot : MonoBehaviour
{
    public static UI_Drag_Slot instance;

    public UI_Slot m_slot;   // ���� ��� ����
    public Image m_icon;           // ������ �̹���

    void Start()
    {
        instance = this;
    }

    // �巡�� �� ��� Ȱ��ȭ
    public void DragSetImage(Sprite icon)
    {
        //Managers.UI.SetOrder(GetComponent<Canvas>());
        m_icon.sprite = icon;
        SetColor(1);
    }

    public void SetColor(float alpha)
    {
        Color color = m_icon.color;
        color.a = alpha;
        m_icon.color = color;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Drag_Slot : MonoBehaviour
{
    public static UI_Drag_Slot instance;

    public UI_Slot m_slot;   // 슬롯 담는 변수
    public Image m_icon;           // 아이템 이미지

    void Start()
    {
        instance = this;
    }

    // 드래그 할 경우 활성화
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

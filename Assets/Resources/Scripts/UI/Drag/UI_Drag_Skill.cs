using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_Drag_Skill : UI_Slot
{
    public SkillData m_skillData;

    protected override void OnBeginDragSlot(PointerEventData eventData)
    {
        if (eventData.button != PointerEventData.InputButton.Left)
            return;

        if (m_skillData.IsNull() == true)
            return;

        UI_Drag_Slot.instance.m_slot = this;
        UI_Drag_Slot.instance.DragSetImage(m_skillData.m_icon);

        UI_Drag_Slot.instance.m_icon.transform.position = eventData.position;
    }

    // 마우스 드래그 방향으로 이동
    protected override void OnDragSlot(PointerEventData eventData)
    {
        if (m_skillData.IsNull() == false)
            UI_Drag_Slot.instance.m_icon.transform.position = eventData.position;
    }

    protected override void OnEndDragSlot(PointerEventData eventData)
    {
        UI_Drag_Slot.instance.SetColor(0);
        UI_Drag_Slot.instance.m_slot = null;
    }
}

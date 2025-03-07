using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UI_Slot : UI_Base
{
    enum Images { ItemImage, }

    public Image m_icon;

    public override void Init()
    {
        SetInfo();
        SetEventHandler();
    }

    public virtual void SetInfo()
    {
        BindImage(typeof(Images));
        m_icon = GetImage((int)Images.ItemImage);
    }

    protected virtual void SetEventHandler()
    {
        gameObject.BindEvent((PointerEventData eventData) => { OnEnterSlot(eventData); }, Define.UIEvent.Enter);
        gameObject.BindEvent((PointerEventData eventData) => { OnExitSlot(eventData); }, Define.UIEvent.Exit);
        gameObject.BindEvent((PointerEventData eventData) => { OnClickSlot(eventData); }, Define.UIEvent.Click);
        gameObject.BindEvent((PointerEventData eventData) => { OnBeginDragSlot(eventData); }, Define.UIEvent.BeginDrag);
        gameObject.BindEvent((PointerEventData eventData) => { OnDragSlot(eventData); }, Define.UIEvent.Drag);
        gameObject.BindEvent((PointerEventData eventData) => { OnEndDragSlot(eventData); }, Define.UIEvent.EndDrag);
        gameObject.BindEvent((PointerEventData eventData) => { OnDropSlot(eventData); }, Define.UIEvent.Drop);
    }

    protected virtual void OnEnterSlot(PointerEventData eventData) { }        //  ���콺 �����Ͱ� ���� ���� ���
    protected virtual void OnExitSlot(PointerEventData eventData) { }         //  ���콺 �����Ͱ� �����Լ� ��� ���
    protected virtual void OnClickSlot(PointerEventData eventData) { }        //  ���콺 ���� Ŭ���� ���
    protected virtual void OnBeginDragSlot(PointerEventData eventData) { }    //  ���콺 �巡�� ����
    protected virtual void OnDragSlot(PointerEventData eventData) { }         //  ���콺 �巡�� ����
    protected virtual void OnEndDragSlot(PointerEventData eventData) { }      //  ���콺 �巡�� ����
    protected virtual void OnDropSlot(PointerEventData eventData) { }         //  ���콺 �巡�װ� �� ������ ������ ��

    public virtual void SetColor(float alpha)
    {
        Color color = m_icon.color;
        color.a = alpha;
        m_icon.color = color;
    }

    public virtual void ClearSlot()
    {
        m_icon.sprite = null;

        SetColor(0);
    }

    public string SetString(string input)
    {
        if (input.StartsWith("Alpha"))
        {
            string result = input.Substring("Alpha".Length);
            return result;
        }

        return input;
    }
}

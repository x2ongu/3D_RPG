using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public abstract class UI_Base : MonoBehaviour
{
    // Key�� Type�̸� value�� ��� ��ü�� ��ӹ޴� Object���� ��� �迭
    // Enum Type�� Key�� �Ͽ� Enum�ȿ� ��� �̸��鿡 �ش��ϴ� ���� ������Ʈ���� �迭�� ��� Value�� �� (C#������ ����(Reflection)�ϸ� C++����� �𸮾󿡼��� ����ϴ� �������
    // ��ũ�� �Լ��� ����� �Լ� �� ���̰� �� ������ ������ �Ľ��ϰ� ���� ����Ͽ� Refleection��
    protected Dictionary<Type, UnityEngine.Object[]> m_objects = new Dictionary<Type, UnityEngine.Object[]>();

    protected bool m_init = false;

    public virtual void Init() { }

    private void Start()
    {
        Init();
    }

    // �Ű� ���� Type type���� Object���� �̸��� ��� �������� ���� �� Enum Ŭ�������� Reflection ������ ��
    // ex) Bind<ExClass>(typeof(ExClass));  Exclass Ŭ������ �ִ�enum ������ �ѱ� : ExClass �ȿ� enum���� ���ǵǾ� �ִ� Object���� �̸��� ������ ã�� �ϳ��ϳ��� �ε� 
    protected void Bind<T>(Type type) where T : UnityEngine.Object
    {
        // Enum.GetNames(type) : type���� �Ѱܹ��� ��ü�� Enum�� ���� �޾� string �迭�� ���� -> Slot��� Object�ȿ� ���� Text�� Image���� ���� �� �ֱ� ����
        string[] names = Enum.GetNames(type);

        if (m_objects.ContainsKey(typeof(T)) == true)
            return;

        // type(Dictionary�� Key)���� �޾ƿ� names�� �迭�� ũ�� ��ŭ Object�迭�� ũ�⸦ �����ְ� Dictonary�� Value�� ����
        UnityEngine.Object[] objects = new UnityEngine.Object[names.Length];
        m_objects.Add(typeof(T), objects);

        // for���� ����� �ڽ� ��ü���� ã�� ���� ����
        for (int i = 0; i < names.Length; i++)
        {
            if (typeof(T) == typeof(GameObject))    // ������Ʈ�� ���� �� GameObject ����
                objects[i] = Util.FindChild(gameObject, names[i], true);
            else                                    // ������Ʈ T�� ����
                objects[i] = Util.FindChild<T>(gameObject, names[i], true);

            if (objects[i].IsNull() == true)
                Debug.Log($"Failed to bind({names[i]})");
        }
    }

    // Type�� ����(��, ������Ʈ�� ����) �޼��� �� ����
    // �� TextMeshProUGUI �ܿ� (Legacy)Text Component���� ��������Ƿ� (Legacy)Text���� �޼��带 �߰��ؾ� �� ����...
    protected void BindObject(Type type) { Bind<GameObject>(type); }
    protected void BindImage(Type type) { Bind<Image>(type); }
    protected void BindText(Type type) { Bind<TextMeshProUGUI>(type); }
    protected void BindButton(Type type) { Bind<Button>(type); }
    protected void BindSlider(Type type) { Bind<Slider>(type); }

    // T Component(or Object)�� ������ ������ �Ƕ���ͷ� �ѱ� int idx�� �ش��ϴ� Object�� T type���� return    
    protected T Get<T>(int idx) where T : UnityEngine.Object
    {
        UnityEngine.Object[] objects = null;

        if (m_objects.TryGetValue(typeof(T), out objects) == false)
            return null;

        return objects[idx] as T;
    }
    // ex) GameObject go = GetImage((int)Images.ItemIcon).gameObject;
                        // -> m_object Dictionary�� typeof(T) Key�� �����ϸ� true return + objects �迭�� �� Key�� Valye�� ����
                        // (int)Images.ItemIcon�� int�� ����ȯ �� �����ε� �̴� enum���� ������ return

    // ���� ����ϴ� ������Ʈ�� ����ϱ� ���� �޼ҵ� ����... �̶�� ��
    protected GameObject GetObject(int idx) { return Get<GameObject>(idx); }
    protected Image GetImage(int idx) { return Get<Image>(idx); }
    protected TextMeshProUGUI GetText(int idx) { return Get<TextMeshProUGUI>(idx); }
    protected Button GetButton(int idx) { return Get<Button>(idx); }
    protected Slider GetSlider(int idx) { return Get<Slider>(idx); }

    // Event Handler�� ���� �޼ҵ� (Comand ����)
    public static void BindEvent(GameObject obj, Action<PointerEventData> action, Define.UIEvent type = Define.UIEvent.Click)
    {        
        UI_EventHandler evt = Util.GetOrAddComponent<UI_EventHandler>(obj);

        switch (type)
        {
            case Define.UIEvent.Enter:
                evt.OnEnterHandler -= action;   // Ȥ�� �̹� �ִٸ� ����
                evt.OnEnterHandler += action;
                break;
            case Define.UIEvent.Exit:
                evt.OnExitHandler -= action;
                evt.OnExitHandler += action;
                break;
            case Define.UIEvent.Click:
                evt.OnClickHandler -= action;
                evt.OnClickHandler += action;
                break;
            case Define.UIEvent.Drag:
                evt.OnDragHandler -= action;
                evt.OnDragHandler += action;
                break;
            case Define.UIEvent.BeginDrag:
                evt.OnBeginDragHandler -= action;
                evt.OnBeginDragHandler += action;
                break;
            case Define.UIEvent.EndDrag:
                evt.OnEndDragHandler -= action;
                evt.OnEndDragHandler += action;
                break;
            case Define.UIEvent.Drop:
                evt.OnDropHandler -= action;
                evt.OnDropHandler += action;
                break;
        }
    }
}
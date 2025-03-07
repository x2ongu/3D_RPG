using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public abstract class UI_Base : MonoBehaviour
{
    // Key는 Type이며 value는 모든 객체가 상속받는 Object들이 담긴 배열
    // Enum Type을 Key로 하여 Enum안에 담긴 이름들에 해당하는 실제 오브젝트들을 배열로 담아 Value로 함 (C#에서만 가능(Reflection)하며 C++기반인 언리얼에서는 모방하는 방식으로
    // 매크로 함수를 멤버나 함수 등등에 붙이고 이 정보를 가지고 파싱하고 따로 기록하여 Refleection함
    protected Dictionary<Type, UnityEngine.Object[]> m_objects = new Dictionary<Type, UnityEngine.Object[]>();

    protected bool m_init = false;

    public virtual void Init() { }

    private void Start()
    {
        Init();
    }

    // 매개 변수 Type type에는 Object들의 이름을 담아 종류별로 구분 한 Enum 클래스들의 Reflection 정보가 들어감
    // ex) Bind<ExClass>(typeof(ExClass));  Exclass 클래스에 있는enum 정보를 넘김 : ExClass 안에 enum으로 정의되어 있는 Object들의 이름을 씬에서 찾아 하나하나씩 로드 
    protected void Bind<T>(Type type) where T : UnityEngine.Object
    {
        // Enum.GetNames(type) : type으로 넘겨받은 객체의 Enum을 전부 받아 string 배열에 저장 -> Slot라는 Object안에 여러 Text와 Image등이 있을 수 있기 떄문
        string[] names = Enum.GetNames(type);

        if (m_objects.ContainsKey(typeof(T)) == true)
            return;

        // type(Dictionary의 Key)으로 받아온 names의 배열의 크기 만큼 Object배열의 크기를 정해주고 Dictonary의 Value에 저장
        UnityEngine.Object[] objects = new UnityEngine.Object[names.Length];
        m_objects.Add(typeof(T), objects);

        // for문을 사용해 자식 객체에서 찾아 정보 저장
        for (int i = 0; i < names.Length; i++)
        {
            if (typeof(T) == typeof(GameObject))    // 컴포넌트가 없는 빈 GameObject 전용
                objects[i] = Util.FindChild(gameObject, names[i], true);
            else                                    // 컴포넌트 T에 따라
                objects[i] = Util.FindChild<T>(gameObject, names[i], true);

            if (objects[i].IsNull() == true)
                Debug.Log($"Failed to bind({names[i]})");
        }
    }

    // Type에 따라(즉, 컴포넌트에 따라) 메서드 재 정의
    // 난 TextMeshProUGUI 외에 (Legacy)Text Component또한 사용했으므로 (Legacy)Text전용 메서드를 추가해야 할 수도...
    protected void BindObject(Type type) { Bind<GameObject>(type); }
    protected void BindImage(Type type) { Bind<Image>(type); }
    protected void BindText(Type type) { Bind<TextMeshProUGUI>(type); }
    protected void BindButton(Type type) { Bind<Button>(type); }
    protected void BindSlider(Type type) { Bind<Slider>(type); }

    // T Component(or Object)를 가지고 있으며 피라미터로 넘긴 int idx에 해당하는 Object를 T type으로 return    
    protected T Get<T>(int idx) where T : UnityEngine.Object
    {
        UnityEngine.Object[] objects = null;

        if (m_objects.TryGetValue(typeof(T), out objects) == false)
            return null;

        return objects[idx] as T;
    }
    // ex) GameObject go = GetImage((int)Images.ItemIcon).gameObject;
                        // -> m_object Dictionary에 typeof(T) Key가 존재하면 true return + objects 배열에 그 Key의 Valye를 저장
                        // (int)Images.ItemIcon을 int로 형변환 한 형태인데 이는 enum에서 정수가 return

    // 자주 사용하는 컴포넌트는 사용하기 좋게 메소드 생성... 이라고 함
    protected GameObject GetObject(int idx) { return Get<GameObject>(idx); }
    protected Image GetImage(int idx) { return Get<Image>(idx); }
    protected TextMeshProUGUI GetText(int idx) { return Get<TextMeshProUGUI>(idx); }
    protected Button GetButton(int idx) { return Get<Button>(idx); }
    protected Slider GetSlider(int idx) { return Get<Slider>(idx); }

    // Event Handler에 관한 메소드 (Comand 패턴)
    public static void BindEvent(GameObject obj, Action<PointerEventData> action, Define.UIEvent type = Define.UIEvent.Click)
    {        
        UI_EventHandler evt = Util.GetOrAddComponent<UI_EventHandler>(obj);

        switch (type)
        {
            case Define.UIEvent.Enter:
                evt.OnEnterHandler -= action;   // 혹시 이미 있다면 빼줌
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
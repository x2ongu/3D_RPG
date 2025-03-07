using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

// 수한 종류의 static 메서드인데, 마치 다른 클래스의 메서드인 것 처럼 호출해 사용할 수 있다.
// 확장 메서드 : 클래스와 함수들이 static이며 매개 변수가 없는 함수
public static class Extension
{
    // 매개변수 자리에 this가 들어가면서 확장 메서드 기능을 함 -> GameObject obj;
    //                                                     obj.GetOrAddComponent<Transform>();      이런식으로 사용 가능
    public static T GetOrAddComponent<T>(this GameObject go) where T : UnityEngine.Component
    {
        return Util.GetOrAddComponent<T>(go);
    }
    
    // go는 this를 사용해 호출 시 매개 변수가 없고 action과 Define만을 사용해 
    public static void BindEvent(this GameObject go, Action<PointerEventData> action, Define.UIEvent type = Define.UIEvent.Click)
    {
        UI_Base.BindEvent(go, action, type);
    }

    // ReferenceEquals 두개의 매개변수가 둘 다 null이거나 참조하는 주소값(힙 메모리의 위치)가 같을때만 true, null과 비교하는거니까 깊게 생각 ㄴㄴ
    // UnityEngine.Object       - Unity 엔진에서 모든 리소스와 게임 오브젝트를 나타내는 기본 클래스, C#의 System.Object에서 파생 X
    // System.Object            - C#에서의 모든 클래스의 최상위 클래스
    // GameObject               -  Unity에서 게임 오브젝트를 나타내는 클래스, 게임 오브젝트는 Unity 시스템에서 씬에서 사용되는 기본 단위이며, 다양한 컴포넌트를 가질 수 있음
    public static bool IsNull(this UnityEngine.Object go) { return ReferenceEquals(go, null); }
    public static bool IsNull(this System.Object go) { return ReferenceEquals(go, null); }

    // Fake Null 체크 : Object의 주소값이 있고 Object가 있을 때, 이 이외의 경우는 FakeNull
    public static bool IsFakeNull(this UnityEngine.Object go) { return (go.IsNull() == false && go == true) == false; }

    // 객체 유효성 확인
    public static bool isValid(this GameObject go) { return go.IsNull() == false && go.activeSelf == true; }
}

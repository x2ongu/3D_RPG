using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Util
{
    // 컴포넌트 찾은 후 추가하기 : 매개변수 go에 T라는 Component가 없으면 추가도 해주고 있다면 GetComponent로 인스턴스를 해주는 메소드
    public static T GetOrAddComponent<T>(GameObject go) where T : UnityEngine.Component
    {
        T component = go.GetComponent<T>();

        // T라는 Component가 null이면
        if (component.IsNull() == true)
            component = go.AddComponent<T>();

        return component;
    }

    // 자기 자신 GameObject(Canvas로 이뤄진)의 모든 자식 Object들에 접근해 names[i]와 일치하는 오브젝트를 로드해 옴
    // 이름으로 검사해 names[i] 이름과 일치하는 Object를 씬에서 찾아와 Dictionary의 Value인 배열의 원소 하나하나(objects[i])에 런타임 로드

    // GameObjec는 컴포넌트가 아니므로 Transform을 통해 찾으며 아래 는 Component 없는 빈 Object만 넘겨받음
    public static GameObject FindChild(GameObject go, string name = null, bool recursive = false)
    {
        Transform transform = FindChild<Transform>(go, name, recursive);
        if (transform.IsNull() == true)
            return null;
        
        return transform.gameObject;
    }

    // 자식 객체 컴포넌트 찾기
    public static T FindChild<T>(GameObject go, string name = null, bool recursive = false) where T : UnityEngine.Object
    {
        if (go.IsNull() == true)
            return null;

        // recursive : 자기 자신의 자식 객체(자식, 손자, 증손자 포함 모~~~든 자식 객체)를 가져올지 판단
        // 만약 false일 경우 직속 자식만 가져옴
        if (recursive == false)
        {
            // go의 자식객체 수 만큼
            for(int i=0; i<go.transform.childCount; i++)
            {
                // 지정된 자식객체를 transform에 반환
                Transform transform = go.transform.GetChild(i);

                // string.IsNullOrEmpty = 빈문자열이면 true (null 또는 "") : 만약 이름이 없거나 null이면(즉, 호출 시 안넘겨줬다면)
                if (string.IsNullOrEmpty(name) || transform.name == name)
                {
                    // 바로 그에 해당하는 T(Button, Text, ...) 컴포넌트 반환
                    T component = transform.GetComponent<T>();
                    if (component.IsNull() == false)
                        return component;
                }
            }
        }
        else
        {
            foreach(T component in go.GetComponentsInChildren<T>())
            {
                if (string.IsNullOrEmpty(name) || component.name == name)
                    return component;
            }
        }

        return null;
    }
}
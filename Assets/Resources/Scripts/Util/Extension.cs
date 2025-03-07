using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

// ���� ������ static �޼����ε�, ��ġ �ٸ� Ŭ������ �޼����� �� ó�� ȣ���� ����� �� �ִ�.
// Ȯ�� �޼��� : Ŭ������ �Լ����� static�̸� �Ű� ������ ���� �Լ�
public static class Extension
{
    // �Ű����� �ڸ��� this�� ���鼭 Ȯ�� �޼��� ����� �� -> GameObject obj;
    //                                                     obj.GetOrAddComponent<Transform>();      �̷������� ��� ����
    public static T GetOrAddComponent<T>(this GameObject go) where T : UnityEngine.Component
    {
        return Util.GetOrAddComponent<T>(go);
    }
    
    // go�� this�� ����� ȣ�� �� �Ű� ������ ���� action�� Define���� ����� 
    public static void BindEvent(this GameObject go, Action<PointerEventData> action, Define.UIEvent type = Define.UIEvent.Click)
    {
        UI_Base.BindEvent(go, action, type);
    }

    // ReferenceEquals �ΰ��� �Ű������� �� �� null�̰ų� �����ϴ� �ּҰ�(�� �޸��� ��ġ)�� �������� true, null�� ���ϴ°Ŵϱ� ��� ���� ����
    // UnityEngine.Object       - Unity �������� ��� ���ҽ��� ���� ������Ʈ�� ��Ÿ���� �⺻ Ŭ����, C#�� System.Object���� �Ļ� X
    // System.Object            - C#������ ��� Ŭ������ �ֻ��� Ŭ����
    // GameObject               -  Unity���� ���� ������Ʈ�� ��Ÿ���� Ŭ����, ���� ������Ʈ�� Unity �ý��ۿ��� ������ ���Ǵ� �⺻ �����̸�, �پ��� ������Ʈ�� ���� �� ����
    public static bool IsNull(this UnityEngine.Object go) { return ReferenceEquals(go, null); }
    public static bool IsNull(this System.Object go) { return ReferenceEquals(go, null); }

    // Fake Null üũ : Object�� �ּҰ��� �ְ� Object�� ���� ��, �� �̿��� ���� FakeNull
    public static bool IsFakeNull(this UnityEngine.Object go) { return (go.IsNull() == false && go == true) == false; }

    // ��ü ��ȿ�� Ȯ��
    public static bool isValid(this GameObject go) { return go.IsNull() == false && go.activeSelf == true; }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Util
{
    // ������Ʈ ã�� �� �߰��ϱ� : �Ű����� go�� T��� Component�� ������ �߰��� ���ְ� �ִٸ� GetComponent�� �ν��Ͻ��� ���ִ� �޼ҵ�
    public static T GetOrAddComponent<T>(GameObject go) where T : UnityEngine.Component
    {
        T component = go.GetComponent<T>();

        // T��� Component�� null�̸�
        if (component.IsNull() == true)
            component = go.AddComponent<T>();

        return component;
    }

    // �ڱ� �ڽ� GameObject(Canvas�� �̷���)�� ��� �ڽ� Object�鿡 ������ names[i]�� ��ġ�ϴ� ������Ʈ�� �ε��� ��
    // �̸����� �˻��� names[i] �̸��� ��ġ�ϴ� Object�� ������ ã�ƿ� Dictionary�� Value�� �迭�� ���� �ϳ��ϳ�(objects[i])�� ��Ÿ�� �ε�

    // GameObjec�� ������Ʈ�� �ƴϹǷ� Transform�� ���� ã���� �Ʒ� �� Component ���� �� Object�� �Ѱܹ���
    public static GameObject FindChild(GameObject go, string name = null, bool recursive = false)
    {
        Transform transform = FindChild<Transform>(go, name, recursive);
        if (transform.IsNull() == true)
            return null;
        
        return transform.gameObject;
    }

    // �ڽ� ��ü ������Ʈ ã��
    public static T FindChild<T>(GameObject go, string name = null, bool recursive = false) where T : UnityEngine.Object
    {
        if (go.IsNull() == true)
            return null;

        // recursive : �ڱ� �ڽ��� �ڽ� ��ü(�ڽ�, ����, ������ ���� ��~~~�� �ڽ� ��ü)�� �������� �Ǵ�
        // ���� false�� ��� ���� �ڽĸ� ������
        if (recursive == false)
        {
            // go�� �ڽİ�ü �� ��ŭ
            for(int i=0; i<go.transform.childCount; i++)
            {
                // ������ �ڽİ�ü�� transform�� ��ȯ
                Transform transform = go.transform.GetChild(i);

                // string.IsNullOrEmpty = ���ڿ��̸� true (null �Ǵ� "") : ���� �̸��� ���ų� null�̸�(��, ȣ�� �� �ȳѰ���ٸ�)
                if (string.IsNullOrEmpty(name) || transform.name == name)
                {
                    // �ٷ� �׿� �ش��ϴ� T(Button, Text, ...) ������Ʈ ��ȯ
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
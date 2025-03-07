using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager
{
    #region Pool
    class Pool
    {
        public GameObject Original { get; private set; }
        public Transform Root { get; set; }

        Stack<Poolable> m_poolStack = new Stack<Poolable>();

        public void Init(GameObject original, int count = 5)
        {
            Original = original;
            Root = new GameObject().transform;
            Root.name = $"{original.name}_Root";

            for (int i = 0; i < count; i++)
            {
                Push(Create());
            }
        }

        Poolable Create()
        {
            GameObject go = Object.Instantiate<GameObject>(Original);
            go.name = Original.name;
            return go.GetOrAddComponent<Poolable>();
        }

        public void Push(Poolable poolable)
        {
            if (poolable == null)
                return;

            poolable.transform.parent = Root;
            poolable.gameObject.SetActive(false);
            poolable.isUsing = false;

            m_poolStack.Push(poolable);
        }

        public Poolable Pop(Transform parent)
        {
            Poolable poolable;

            if (m_poolStack.Count > 0)
                poolable = m_poolStack.Pop();
            else
                poolable = Create();

            poolable.gameObject.SetActive(true);

            if (parent == null)
                poolable.transform.parent = Managers.Scene.CurrentScene.transform;

            poolable.transform.parent = parent;
            poolable.isUsing = true;

            return poolable;
        }
    }
    #endregion

    Dictionary<string, Pool> m_pool = new Dictionary<string, Pool>();

    Transform m_root;

    public void Init()
    {
        if (m_root == null)
        {
            m_root = new GameObject { name = "@Pool_Root" }.transform;
            Object.DontDestroyOnLoad(m_root);
        }
    }

    public void CreatePool(GameObject original, int count = 5)
    {
        Pool pool = new Pool();
        pool.Init(original, count);
        pool.Root.parent = m_root;

        m_pool.Add(original.name, pool);
    }

    public void Push(Poolable poolable)
    {
        string name = poolable.gameObject.name;

        if (m_pool.ContainsKey(name) == false)
        {
            GameObject.Destroy(poolable.gameObject);
            return;
        }

        m_pool[name].Push(poolable);
    }

    public Poolable Pop(GameObject original, Transform parent = null)
    {
        if (m_pool.ContainsKey(original.name) == false)
            CreatePool(original);

        return m_pool[original.name].Pop(parent);
    }

    public GameObject GetOriginal(string name)
    {
        if (m_pool.ContainsKey(name) == false)
            return null;

        return m_pool[name].Original;
    }

    public void Clear()
    {
        foreach (Transform child in m_root)
        {
            GameObject.Destroy(child.gameObject);
        }

        m_pool.Clear();
    }
}
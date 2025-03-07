using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Managers : MonoBehaviour
{
    static Managers s_instance;
    static Managers Instance { get { Init(); return s_instance; } }

    NPCManager m_npc = new NPCManager();

    DataManager m_data = new DataManager();
    InputManager m_input = new InputManager();
    PoolManager m_pool = new PoolManager();
    ResourceManager m_resource = new ResourceManager();
    SceneManagerEx m_scene = new SceneManagerEx();
    SoundManager m_sound = new SoundManager();

    public static NPCManager NPC { get { return Instance.m_npc; } }

    public static DataManager Data { get { return Instance.m_data; } }
    public static InputManager Input { get { return Instance.m_input; } }
    public static PoolManager Pool { get { return Instance.m_pool; } }
    public static ResourceManager Resource { get { return Instance.m_resource; } }
    public static SceneManagerEx Scene { get { return Instance.m_scene; } }
    public static SoundManager Sound { get { return Instance.m_sound; } }

    void Update()
    {
        if (m_scene.CurrentScene.SceneType == Define.Scene.Game)
            s_instance.m_input.OnUpdate();
    }

    static void Init()
    {
        if (s_instance == null)
        {
            GameObject go = GameObject.Find("@Managers");
            if (go == null)
            {
                go = new GameObject { name = "@Managers" };
                go.AddComponent<Managers>();
            }

            DontDestroyOnLoad(go);
            s_instance = go.GetComponent<Managers>();

            s_instance.m_data.Init();
            s_instance.m_pool.Init();
            s_instance.m_sound.Init();
        }
    }

    public static void Clear()
    {
        Sound.Clear();
        Scene.Clear();

        Pool.Clear();
    }
}

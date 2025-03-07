using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager
{
    public AudioSource[] m_audioSources = new AudioSource[(int)Define.Sound.MaxCount];

    Dictionary<string, AudioClip> m_audioClips = new Dictionary<string, AudioClip>();

    float m_maxEffectVolume = 1.0f;

    public void Init()
    {
        GameObject root = GameObject.Find("@Sound");
        if (root == null)
        {
            root = new GameObject { name = "@Sound" };
            Object.DontDestroyOnLoad(root);

            string[] soundNames = System.Enum.GetNames(typeof(Define.Sound));
            for (int i = 0; i < soundNames.Length - 1; i++)
            {
                GameObject go = new GameObject() { name = soundNames[i] };
                m_audioSources[i] = go.AddComponent<AudioSource>();
                go.transform.parent = root.transform;
            }

            m_audioSources[(int)Define.Sound.Bgm].loop = true;
        }
    }

    public void Clear()
    {
        foreach (AudioSource audioSource in m_audioSources)
        {
            audioSource.clip = null;
            audioSource.Stop();
        }
        m_audioClips.Clear();
    }

    public void Play(string name, Define.Sound type = Define.Sound.Effect, float volume = 1.0f ,float pitch = 1.0f)  // Ŭ���� �̸��� Load�� �޾ƿ��� ����
    {
        AudioClip audioClip = GetOrAddAudioClip(name, type);
        Play(audioClip, type, volume, pitch);
    }

    public void Play(AudioClip audioClip, Define.Sound type = Define.Sound.Effect, float volume = 1.0f, float pitch = 1.0f)  // ����� Ŭ���� ���� �޴� ����
    {

        if (audioClip == null)
            return;

        if (type == Define.Sound.Bgm)   // BGM�� ���
        {
            AudioSource audioSource = m_audioSources[(int)Define.Sound.Bgm];

            if (audioSource.clip == audioClip)
                return;

            if (audioSource.isPlaying)
                audioSource.Stop();

            audioSource.pitch = pitch;
            audioSource.clip = audioClip;
            audioSource.Play();
        }
        else                            // Effect�� ���
        {
            AudioSource audioSource = m_audioSources[(int)Define.Sound.Effect];
            audioSource.pitch = pitch;
            audioSource.volume = m_maxEffectVolume * volume;
            audioSource.PlayOneShot(audioClip);
        }
    }

    AudioClip GetOrAddAudioClip(string name, Define.Sound type = Define.Sound.Effect)
    {
        AudioClip audioClip = null;

        if (type == Define.Sound.Bgm)   // BGM�� ���
        {
            name = $"Sounds/Bgm/{name}";
            audioClip = Managers.Resource.Load<AudioClip>(name);
        }
        else                            // Effect�� ���
        {
            name = $"Sounds/Effect/{name}";

            if (m_audioClips.TryGetValue(name, out audioClip) == false)
            {
                audioClip = Managers.Resource.Load<AudioClip>(name);
                m_audioClips.Add(name, audioClip);
            }
        }

        if (audioClip == null) // ���� name�� ���� �̸��� ���� Clip�� ���� �� -> ���߿� �����
        {
            Debug.Log($"Audio Clip Missing !! {name}");
        }

        return audioClip;
    }

    public void SetVolume()
    {
        // todo : m_audioSources�� ��� �� ���� �����ϴ� �ɼ� ���
    }
}
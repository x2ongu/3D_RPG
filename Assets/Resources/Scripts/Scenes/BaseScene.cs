using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public abstract class BaseScene : MonoBehaviour
{
    public Define.Scene SceneType { get; protected set; } = Define.Scene.Unknown;

    void Awake()
    {
        Init();
    }

    protected virtual void Init()
    {
        Object obj = GameObject.FindObjectOfType(typeof(EventSystem));
        if (obj == null)
            Managers.Resource.Instantiate("UI/EventSystem").name = "@EventSystem";
    }

    public abstract void Clear();

    protected IEnumerator FadeInAndPlayBGM(AudioClip newClip, float fadeDuration)
    {
        float startVolume = Managers.Sound.m_audioSources[(int)Define.Sound.Bgm].volume;

        // BGM 변경
        Managers.Sound.m_audioSources[(int)Define.Sound.Bgm].volume = 0.0f;
        Managers.Sound.Play(newClip, Define.Sound.Bgm);
        yield return new WaitForSeconds(0.1f);

        // 새로운 BGM 페이드 인
        while (Managers.Sound.m_audioSources[(int)Define.Sound.Bgm].volume < startVolume)
        {
            Managers.Sound.m_audioSources[(int)Define.Sound.Bgm].volume += Time.deltaTime / fadeDuration;
            yield return null;
        }
    }
}
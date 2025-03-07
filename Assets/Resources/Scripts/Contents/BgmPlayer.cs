using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BgmPlayer : MonoBehaviour
{
    [SerializeField]
    AudioClip m_clip;

    Coroutine m_coroutine;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            ChangeBgm(m_clip);
        }
    }

    void ChangeBgm(AudioClip clip)
    {
        if (Managers.Sound.m_audioSources[(int)Define.Sound.Bgm].clip == clip)
            return;

        m_coroutine = StartCoroutine(FadeOutAndChangeBGM(clip, 3f));
    }

    IEnumerator FadeOutAndChangeBGM(AudioClip newClip, float fadeDuration)
    {
        // 현재 BGM 페이드 아웃
        float startVolume = Managers.Sound.m_audioSources[(int)Define.Sound.Bgm].volume;

        while (Managers.Sound.m_audioSources[(int)Define.Sound.Bgm].volume > 0)
        {
            Managers.Sound.m_audioSources[(int)Define.Sound.Bgm].volume -= Time.deltaTime / fadeDuration;
            yield return null;
        }

        // BGM 변경
        Managers.Sound.Play(newClip, Define.Sound.Bgm);
        yield return new WaitForSeconds(0.1f);  // 잠깐 연유를 둬야 정상적으로 작동함

        // 새로운 BGM 페이드 인
        while (Managers.Sound.m_audioSources[(int)Define.Sound.Bgm].volume < startVolume)
        {
            Managers.Sound.m_audioSources[(int)Define.Sound.Bgm].volume += Time.deltaTime / fadeDuration;
            yield return null;
        }
    }
}

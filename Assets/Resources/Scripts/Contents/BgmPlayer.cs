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
        // ���� BGM ���̵� �ƿ�
        float startVolume = Managers.Sound.m_audioSources[(int)Define.Sound.Bgm].volume;

        while (Managers.Sound.m_audioSources[(int)Define.Sound.Bgm].volume > 0)
        {
            Managers.Sound.m_audioSources[(int)Define.Sound.Bgm].volume -= Time.deltaTime / fadeDuration;
            yield return null;
        }

        // BGM ����
        Managers.Sound.Play(newClip, Define.Sound.Bgm);
        yield return new WaitForSeconds(0.1f);  // ��� ������ �־� ���������� �۵���

        // ���ο� BGM ���̵� ��
        while (Managers.Sound.m_audioSources[(int)Define.Sound.Bgm].volume < startVolume)
        {
            Managers.Sound.m_audioSources[(int)Define.Sound.Bgm].volume += Time.deltaTime / fadeDuration;
            yield return null;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyScene : BaseScene
{
    public AudioClip m_clip;

    private void Update()
    {
        if (Input.anyKeyDown)
        {
            Managers.Scene.LoadScene(Define.Scene.Game);
        }
    }

    protected override void Init()
    {
        base.Init();

        SceneType = Define.Scene.Lobby;

        StartCoroutine(FadeInAndPlayBGM(m_clip, 3f));
    }

    public override void Clear() { }
}

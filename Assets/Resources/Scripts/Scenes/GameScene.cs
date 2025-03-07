using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameScene : BaseScene
{
    public AudioClip m_clip;

    protected override void Init()
    {
        base.Init();

        SceneType = Define.Scene.Game;

        StartCoroutine(FadeInAndPlayBGM(m_clip, 3f));
    }

    public override void Clear()
    {

    }
}
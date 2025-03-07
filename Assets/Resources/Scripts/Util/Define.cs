using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Define : MonoBehaviour
{
    public enum Quest
    {
        NotStarted,
        WorkInProgress,
        CanBeCompleted,
        Finished
    }

    public enum Scene
    {
        Unknown,
        Login,
        Lobby,
        Game,
        Loading,
    }

    public enum Sound
    {
        Bgm,
        Effect,
        MaxCount,
    }

    public enum UIEvent
    {
        Enter,
        Exit,
        Click,
        BeginDrag,
        Drag,
        EndDrag,
        Drop
    }
}

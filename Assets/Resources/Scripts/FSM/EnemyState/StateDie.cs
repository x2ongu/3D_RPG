using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateDie : FSMSingleton<StateDie>, IFSMState<StateManager>
{
    public void Enter(StateManager e)
    {
        e.m_anim.SetBool("isLive", false);
        e.m_anim.SetTrigger("getDead");
        e.m_navEnemy.enabled = false;
        e.m_iEnemy.Die();
    }

    public void Execute(StateManager e)
    {

    }

    public void Exit(StateManager e)
    {
        e.m_navEnemy.enabled = true;
    }
}

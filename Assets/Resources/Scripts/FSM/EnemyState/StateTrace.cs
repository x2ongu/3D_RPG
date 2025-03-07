using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateTrace : FSMSingleton<StateTrace>, IFSMState<StateManager>
{
    public void Enter(StateManager e)
    {
        e.m_anim.SetBool("isMove", true);
    }

    public void Execute(StateManager e)
    {
        if (GameManager.Inst.m_player.m_isLive == false)
            e.ChangeState(StateReturn.Instance);

        if (e.IsCloseToTarget(e.m_spawnPoint.position, e.m_traceRange))
        {
            e.Move(e.m_target.position);

            if (e.IsCloseToTarget(e.m_target.position, e.m_stat.AttackRange))
            {
                e.ChangeState(StateWatch.Instance);
            }
        }
        else
            e.ChangeState(StateReturn.Instance);
    }

    public void Exit(StateManager e)
    {
        e.m_anim.SetBool("isMove", false);
    }
}

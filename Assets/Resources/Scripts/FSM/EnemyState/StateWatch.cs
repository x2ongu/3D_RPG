using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateWatch : FSMSingleton<StateWatch>, IFSMState<StateManager>
{
    Vector3 m_target;

    public void Enter(StateManager e)
    {
        m_target = e.m_target.position;
    }

    public void Execute(StateManager e)
    {
        if (GameManager.Inst.m_player.m_isLive == false)
            e.ChangeState(StateReturn.Instance);

        if (e.IsCloseToTarget(m_target, e.m_stat.AttackRange))
        {
            e.WatchToTarget(m_target);

            if (e.IsFacingTarget(m_target, 5f))
            {
                e.ChangeState(StateAttack.Instance);
            }
        }
        else
        {
            e.ChangeState(StateTrace.Instance);
        }
    }

    public void Exit(StateManager e)
    {
        
    }
}

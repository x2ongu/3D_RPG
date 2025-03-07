using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateStay : FSMSingleton<StateStay>, IFSMState<StateManager>
{
    public void Enter(StateManager e)
    {
        e.StartCoroutine(e.SetRotation());
        e.m_stat.Hp = e.m_stat.MaxHp;
    }

    public void Execute(StateManager e)
    {
        if (GameManager.Inst.m_player.m_isLive != false)
        {
            if (e.m_target != null)
            {
                if (e.IsCloseToTarget(e.m_target.position, e.m_searchRange) && e.GetRemainingDistance(e.m_target) <= e.m_searchRange)
                {
                    e.ChangeState(StateTrace.Instance);
                }
            }
        }
    }

    public void Exit(StateManager e)
    { 
        
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateAttack : FSMSingleton<StateAttack>, IFSMState<StateManager>
{
    public void Enter(StateManager e)
    {
        e.m_anim.SetBool("isMove", false);
        e.m_enemy.m_isAttackPlaying = true;
        e.m_iEnemy.Attack();
    }

    public void Execute(StateManager e)
    {
        if (!e.m_enemy.m_isAttackPlaying)
        {
            if (GameManager.Inst.m_player.m_isLive == false)
                e.ChangeState(StateReturn.Instance);
            else
                e.ChangeState(StateWatch.Instance);
        }
    }

    public void Exit(StateManager e)
    {
        
    }
}

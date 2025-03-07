using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateReturn : FSMSingleton<StateReturn>, IFSMState<StateManager>
{
    public void Enter(StateManager e)
    {
        //Debug.Log("복귀 중...");
        // + 체력 채우기
        e.m_anim.SetBool("isMove", true);
    }

    public void Execute(StateManager e)
    {
        if (Vector3.Distance(e.m_spawnPoint.position, e.transform.position) > 2f)
        {
            e.MoveReturn(e.m_spawnPoint.position);
        }
        else
            e.ChangeState(StateStay.Instance);
    }

    public void Exit(StateManager e)
    {
        e.m_anim.SetBool("isMove", false);
    }
}

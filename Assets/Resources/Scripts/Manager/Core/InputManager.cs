using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InputManager
{
    private Vector3 m_mousePoint;
    private Vector3 m_movePoint;

    private float m_attackTime = -float.MaxValue;

    public void OnUpdate()
    {
        if(Managers.Scene.CurrentScene.SceneType == Define.Scene.Game)
        {
            if (DoItBlockGetKey())    // Block Get Key By OnEabled Objects
                return;

            GetKeyDown_WhenPlayerIsAlive();
        }
    }

    private void GetKeyDown_WhenPlayerIsAlive()
    {
        if (GameManager.Inst.m_player.m_isLive == false)
            return;

        GetKeyDown_Weapon();

        GetKey_MouseRight();

        GetKeyDown_Space();

        GetKeyDown_G();

        GetKeyDown_S();

        GameManager.Inst.m_quickSlot.InputItemQuickSlot();

        GameManager.Inst.m_popup.UI_Controller();
    }


    public void GetKey_MouseLeftorA()
    {
        m_attackTime += Time.deltaTime;

        if (Input.GetKey(KeyCode.Mouse0))
        {
            if (EventSystem.current.IsPointerOverGameObject() == false)
            {
                if (UI_Drag_Slot.instance.m_slot == null)    // 인벤이 안켜져있다면 -> 드래그 해서 아이템을 UI 밖으로 뺄 경우 공격해서 이렇게 막음
                {
                    DoAttack();
                }
            }
        }
        else if(Input.GetKey(KeyCode.A))
        {
            DoAttack();
        }
    }

    public void GetKey_MouseRight()
    {
        if (Input.GetKey(KeyCode.Mouse1) && GameManager.Inst.m_player.m_animEvent.m_isMove)
        {
            if (EventSystem.current.IsPointerOverGameObject() == false)
            {
                m_movePoint = MousePointByRay();

                if (Vector3.Distance(GameManager.Inst.m_player.transform.position, m_movePoint) > 0.1f && GameManager.Inst.m_player.m_animEvent.m_isMove)
                {
                    GameManager.Inst.m_player.m_navAgent.SetDestination(m_movePoint);
                    GameManager.Inst.m_player.m_animEvent.m_anim.SetFloat("Speed", GameManager.Inst.m_player.m_stat.MoveSpeed);
                }
            }
        }
    }

    public void GetKeyDown_Space()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            GameManager.Inst.m_player.m_interManager.GetItem();
        }
    }

    public void GetKeyDown_G()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            GameManager.Inst.m_player.m_interManager.ReturnSelectedNPC();
        }
    }

    public void GetKeyDown_S()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            GameManager.Inst.m_player.m_navAgent.ResetPath();
        }
    }

    public Vector3 MousePointByRay()
    {
        Ray ray = GameManager.Inst.m_player.m_mainCam.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit groundHit, Mathf.Infinity, LayerMask.GetMask("Ground")))
        {
            m_mousePoint = groundHit.point;
            m_mousePoint.y = GameManager.Inst.m_player.transform.position.y;
        }
        else if (Physics.Raycast(ray, Mathf.Infinity, LayerMask.GetMask("BackGround")))
        {
            if (Physics.Raycast(ray, out RaycastHit pivotHit, Mathf.Infinity, LayerMask.GetMask("Pivot")))
            {
                m_mousePoint = pivotHit.point;
                m_mousePoint.y = GameManager.Inst.m_player.transform.position.y;
            }
        }

        return m_mousePoint;
    }

    private void GetKeyDown_Weapon()
    {
        if (GameManager.Inst.m_player.m_weapon != null)
        {
            GetKey_MouseLeftorA();

            if (GameManager.Inst.m_player.m_animEvent.m_isAttackPlaying == false)
                GameManager.Inst.m_quickSlot.InputSkillQuickSlot();
        }
        else
            return;
    }

    private bool DoItBlockGetKey()
    {
        // If Deal Equipment UI Is Open
        if (GameManager.Inst.m_popup.m_deal_equipment.GetComponent<UI_Deal_Equipment>().m_isActive)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                GameManager.Inst.m_popup.ClosePopUp(GameManager.Inst.m_popup.m_deal_equipment, false);
                return true;
            }
        }

        // If Deal Consumable UI Is Open
        if (GameManager.Inst.m_popup.m_deal_consumable.GetComponent<UI_Deal_Consumable>().m_isActive)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                GameManager.Inst.m_popup.ClosePopUp(GameManager.Inst.m_popup.m_deal_consumable, false);
                return true;
            }
        }

        return false;
    }

    private void DoAttack()
    {
        GameManager.Inst.m_player.m_weapon.GetComponent<Weapon>().m_trailRenderer.SetActive(true);

        if (m_attackTime > GameManager.Inst.m_player.m_stat.AttackRate || m_attackTime < 0f)
        {
            GameManager.Inst.m_player.m_animEvent.m_anim.SetTrigger("Attack");
            m_attackTime = 0f;
        }
    }
}

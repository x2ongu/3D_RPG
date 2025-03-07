using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager m_inst;
    public static GameManager Inst
    { 
        get
        {
            if (m_inst == null)
            {
                m_inst = FindObjectOfType<GameManager>();

                if (m_inst == null)
                {
                    GameObject singletonObj = new GameObject("Game Manager");
                    m_inst = singletonObj.AddComponent<GameManager>();
                }
            }
            return m_inst;
        }
    }

    public Player m_player;
    public CopyPlayer m_copyPlayer;
    public PopUpManager m_popup;
    public UI_QuickSlot m_quickSlot;
    public QuestManager m_quest;

    private void Awake()
    {
        OnInstance();
    }

    private void Start()
    {
        Managers.Data.LoadData();
    }

    private void OnInstance()
    {
        if (m_inst == null)
            m_inst = this;
    }

    public void Respawn(Enemy enemy)
    {
        StartCoroutine(Respawn(enemy.m_stat.RespawnTime));

        IEnumerator Respawn(float respawnTime)
        {
            yield return new WaitForSeconds(respawnTime);

            GameObject obj = Managers.Resource.Instantiate($"Enemy/{enemy.name}");
            Enemy enemyObj = obj.GetComponent<Enemy>();

            enemyObj.SetInfo();

            obj.transform.position = enemy.m_stateManager.m_spawnPoint.position;
            obj.transform.rotation = enemy.m_stateManager.m_initialRot;

            enemyObj.m_stateManager.m_anim.SetBool("isLive", true);

            enemyObj.m_stateManager.ChangeState(StateStay.Instance);
            enemyObj.m_stateManager.m_isDead = false;
        }
    }

    public void ReviveCurrentPos()
    {
        m_player.m_stat.Gold -= 1000;

        Revive();
    }

    public void ReviveVillage()
    {
        m_player.gameObject.transform.position = new Vector3(-10.0f, 0.45f, -12.0f);
        m_player.gameObject.transform.rotation = Quaternion.Euler(0, 75, 0);

        Revive();
    }

    private void Revive()
    {
        m_popup.ClosePopUp(m_popup.m_reviver, false);

        m_player.m_animEvent.m_anim.Play("Revive");
        m_player.m_stat.Hp = m_player.m_stat.MaxHp;

        Managers.Sound.Play("Player/Revive");
        GameObject obj = Managers.Resource.Instantiate("Effect/Player/Revive");
        obj.SetActive(true);

        StartCoroutine(EndReviverMotion());
    }

    IEnumerator EndReviverMotion()
    {
        yield return new WaitForSeconds(3.0f);

        m_player.m_isLive = true;
        m_player.m_animEvent.m_anim.SetBool("CanAttack", true);
        m_player.m_animEvent.m_anim.SetBool("IsLive", true);
        m_player.m_animEvent.m_isMove = true;
        m_player.m_navAgent.enabled = true;
    }
}
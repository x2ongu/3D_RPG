using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface INPC
{
    void DoInteraction(bool questNPC = false);
    void DoReaction();
}

public class NPC : MonoBehaviour, INPC
{
    [HideInInspector]
    public Animator m_anim;

    private void Awake()
    {
        Init();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            DoReaction();
        }
    }

    public virtual void Init()
    {
        m_anim = GetComponentInChildren<Animator>();
        Managers.NPC.RegisterNPC(gameObject);
    }

    public virtual void DoInteraction(bool questNPC) { }

    public void DoReaction()
    {
        m_anim.SetTrigger("reaction");
    }
}

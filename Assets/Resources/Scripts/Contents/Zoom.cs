using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.EventSystems;

public class Zoom : MonoBehaviour
{
    [SerializeField]
    private CinemachineVirtualCamera[] m_virtualCameras;

    [SerializeField]
    private float m_zoomSpeed = 1f;

    private int m_curCamIndex = 0;

    private void Start()
    {
        SwitchCamera(m_curCamIndex);
    }

    private void Update()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");

        if (scroll > 0f)
        {
            if (m_curCamIndex + 1 < m_virtualCameras.Length)
                if (EventSystem.current.IsPointerOverGameObject() == false)
                    SwitchCamera(m_curCamIndex + 1);
        }
        else if (scroll < 0f)
        {
            if (m_curCamIndex > 0)
                if (EventSystem.current.IsPointerOverGameObject() == false)
                    SwitchCamera(m_curCamIndex - 1);
        }
    }

    void SwitchCamera(int newIndex)
    {
        m_virtualCameras[m_curCamIndex].gameObject.SetActive(false);
        m_virtualCameras[newIndex].gameObject.SetActive(true);

        m_curCamIndex = newIndex;

        // Virtual Camera의 Active를 켜고 끄면서 전환되는 화면 속도를 조절
        CinemachineCore.Instance.GetActiveBrain(0).m_DefaultBlend.m_Time = m_zoomSpeed;
    }
}

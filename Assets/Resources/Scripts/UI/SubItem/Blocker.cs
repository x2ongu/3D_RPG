using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Blocker : MonoBehaviour
{
    private void Update()
    {
        if (Input.GetMouseButton(0) || Input.GetMouseButton(1))
        {
            if (EventSystem.current.IsPointerOverGameObject())
                return;
        }
    }
}

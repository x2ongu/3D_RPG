using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FixRotation : MonoBehaviour
{
    private float fixedRotX = 90;
    private float fixedRotY = 0;

    void Update()
    {
        transform.rotation = Quaternion.Euler(fixedRotX, fixedRotY, transform.rotation.y);
    }
}

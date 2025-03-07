using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectActiveFalse : MonoBehaviour
{
    private void OnEnable()
    {
        StartCoroutine(ActiveFalse());
    }

    IEnumerator ActiveFalse()
    {
        yield return new WaitForSeconds(3f);

        Managers.Resource.Destroy(gameObject);
    }
}
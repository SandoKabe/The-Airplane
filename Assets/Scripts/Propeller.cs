using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Propeller : MonoBehaviour
{
    Coroutine mCoroutine;

    public static Propeller Instance { get; private set;}

    private void Start()
    {
        Instance = this;
    }

    public void RotatePorpeller()
    {
        mCoroutine = StartCoroutine("Rot");
    }
    public void StopRotatePorpeller()
    {
        if (mCoroutine != null)
        {
            StopCoroutine(mCoroutine);
        }
        
    }
    IEnumerator Rot()
    {
        
        while (true)
        {
            transform.Rotate(0, 50, 0);
            yield return null;
        }
    }
}

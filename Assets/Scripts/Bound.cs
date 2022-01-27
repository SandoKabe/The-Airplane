using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bound : MonoBehaviour
{
    private Collider col;

    static Bound s_Instance = null;
    public static Bound Instance => s_Instance;

    void Start()
    {
        s_Instance = this;
        col = GetComponent<Collider>();
        
    }

    public bool OutOfBound(Vector3 pos)
    {
        if (pos.x > col.bounds.max.x || pos.x < col.bounds.min.x )
        {
            return true;
        }
        if (pos.y > col.bounds.max.y || pos.y < col.bounds.min.y)
        {
            return true;
        }
        if (pos.z > col.bounds.max.z || pos.y < col.bounds.min.z)
        {
            return true;
        }
        return false;
    }
}

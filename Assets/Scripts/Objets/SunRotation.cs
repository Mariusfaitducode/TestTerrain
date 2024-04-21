using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SunRotation : MonoBehaviour
{
    public Vector3 rotation;

    private float lastVal;
    private float val;
    void Update()
    {
        
        if (val < 0)
        {
            transform.Rotate(rotation * Time.deltaTime);
        }
        else
        {
            transform.Rotate(rotation * 3f *Time.deltaTime);
        }
        val = Math.Abs(lastVal) - Math.Abs(transform.rotation.x);

        lastVal = transform.rotation.x;
    }
}

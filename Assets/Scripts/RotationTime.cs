using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationTime : MonoBehaviour
{
    float count;
    void Update()
    {
        count += 0.5f;
        transform.localRotation = Quaternion.Euler(new Vector3(0, count, 0));
    }
}

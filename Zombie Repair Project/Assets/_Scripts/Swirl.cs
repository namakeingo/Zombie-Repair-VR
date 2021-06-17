using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Rotates stuff around
/// UNUSED
/// </summary>
public class Swirl : MonoBehaviour
{
    public Vector3 rotationAxis;
    public float speed;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    void FixedUpdate()
    {
        var rot = Quaternion.AngleAxis(speed * Time.deltaTime, rotationAxis);
        transform.localRotation *= rot;
    }
}

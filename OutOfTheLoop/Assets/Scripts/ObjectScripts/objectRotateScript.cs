using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class objectRotateScript : MonoBehaviour
{
public float rotationSpeed = 50f; // Speed of rotation in degrees per second

    private void FixedUpdate()
    {
        // Rotate the object around its own axis
        transform.Rotate(Vector3.forward, rotationSpeed * Time.deltaTime);
    }
}

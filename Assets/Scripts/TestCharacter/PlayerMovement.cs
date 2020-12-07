using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    public float Speed = 5.0f;

    private Rigidbody Rb => GetComponent<Rigidbody>();

    private void FixedUpdate()
    {
        UpdateMovement();
    }

    private void UpdateMovement()
    {
        var horizontal = Input.GetAxis("Horizontal");
        var vertical = Input.GetAxis("Vertical");

        Rb.AddForce(new Vector3(horizontal * Speed, 0, vertical * Speed));
    }
}

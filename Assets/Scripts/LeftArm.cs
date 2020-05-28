using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeftArm : MonoBehaviour
{
    public float speed = 2f;
    public float jumpHeight = 10f;
    public float groundDistance = 0.2f;
    public LayerMask groundLayer;

    Rigidbody _body;
    Vector3 _direction = Vector3.zero;

    void Start()
    {
        _body = GetComponent<Rigidbody>();
    }

    void Update()
    {
        // Basic directional controls.
        _direction = Vector3.zero;
        _direction.x -= Input.GetKey(KeyCode.A) ? 1 : 0;  // left
        _direction.x += Input.GetKey(KeyCode.D) ? 1 : 0;  // right
        _direction.z -= Input.GetKey(KeyCode.S) ? 1 : 0;  // forwards
        _direction.z += Input.GetKey(KeyCode.W) ? 1 : 0;  // backwards
        _direction.Normalize();

        // Jump control.
        bool isGrounded = Physics.CheckSphere(_body.position, groundDistance,
            groundLayer, QueryTriggerInteraction.Ignore);
        if (Input.GetKeyDown(KeyCode.LeftShift) && isGrounded)
        {
            // Use a projectile motion equation to calculate initial y-velocity
            // needed for the body to reach the specified jump height.
            float vyi = Mathf.Sqrt(jumpHeight * -2f * Physics.gravity.y);
            _body.AddForce(Vector3.up * vyi, ForceMode.VelocityChange);
        }
    }

    void FixedUpdate()
    {
        Vector3 displacement = _direction * speed * Time.fixedDeltaTime;
        _body.MovePosition(_body.position + displacement);
    }
}

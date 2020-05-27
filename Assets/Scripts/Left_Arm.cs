using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Left_Arm : MonoBehaviour
{

    public float Speed = 5f;
    public float JumpHeight = 5f;
    public float GroundDistance = 0.2f;
    public LayerMask Ground;

    private Rigidbody _body;
    private Vector3 _inputs = Vector3.zero;
    private bool _isGrounded = true;
    private Transform _groundChecker;

    void Start()
    {
        _body = GetComponent<Rigidbody>();
        _groundChecker = transform.GetChild(0);
        Debug.Log(transform.childCount);
    }

    void Update()
    {
        _isGrounded = Physics.CheckSphere(_groundChecker.position, GroundDistance, Ground, QueryTriggerInteraction.Ignore);


        _inputs = Vector3.zero;
        _inputs.x = Input.GetKey(KeyCode.A) ? 1 : (Input.GetKey(KeyCode.D) ? -1 : 0);
        _inputs.z = Input.GetKey(KeyCode.S) ? 1 : (Input.GetKey(KeyCode.W) ? -1 : 0);
        //if (_inputs != Vector3.zero)
            //transform.forward = _inputs;

        if (Input.GetKeyDown(KeyCode.LeftShift) && _isGrounded)
        {
            _body.AddForce(Vector3.up * Mathf.Sqrt(JumpHeight * -2f * Physics.gravity.y), ForceMode.VelocityChange);
        }
    }


    void FixedUpdate()
    {
        _body.MovePosition(_body.position + _inputs * Speed * Time.fixedDeltaTime);
    }
}

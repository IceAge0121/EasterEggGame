using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public CharacterController _controller;

    public float _speed = 12f;
    public float _gravity = -9.81f;
    public float _jumpHeight = 3f;

    public Transform _groundCheck;
    public float _groundDistance = 0.4f;
    public LayerMask _groundMask;

    Vector3 _velocity;
    bool _isGrounded;

    private void Start()
    {
        _controller = GetComponent<CharacterController>();
    }


    // Update is called once per frame
    void Update()
    {
        //_isGrounded = Physics.CheckSphere(_groundCheck.position, _groundDistance, _groundMask);

        //Uses built in method "isGrounded" to check if Character Controller is colliding with anything.
        if (_controller.isGrounded)
        {
            _isGrounded = true;
            Debug.Log("Player is grounded!");
        }
        else
        {
            _isGrounded = false;
            Debug.Log("Player is in the air!");
        }
            
        //Forces player to the ground when their Y velocity is 0 or less
        if (_isGrounded && _velocity.y < 0)
        {
            _velocity.y = -2f;
        }

        float _x = Input.GetAxis("Horizontal");
        float _z = Input.GetAxis("Vertical");

        Vector3 _move = transform.right * _x + transform.forward * _z;

        _controller.Move(_move * _speed * Time.deltaTime);

        /*if(_controller.isGrounded)
        {
            Debug.Log("Player is grounded!");
        }*/
        
        
        //if(Input.GetButtonDown("Jump"))
        if (Input.GetButtonDown("Jump") && _isGrounded)
        {
            _velocity.y = Mathf.Sqrt(_jumpHeight * -2f * _gravity);
        }

        _velocity.y += _gravity * Time.deltaTime;

        _controller.Move(_velocity * Time.deltaTime);

    }
}

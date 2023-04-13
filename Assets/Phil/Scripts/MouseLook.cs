using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseLook : MonoBehaviour
{
    public float _mouseSensitivity = 50f;

    public Transform _playerBody;

    float xRotation = 0f;
    
    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = true;
    }

    // Update is called once per frame
    void Update()
    {
        //Assigns floats for mouse x and y axes multiplied by mouse sensitivity * delta time.
        //float mouseX = Input.GetAxis("Mouse X") * _mouseSensitivity * Time.deltaTime;
        float mouseX = Input.GetAxis("Mouse X") * _mouseSensitivity;

        //float mouseY = Input.GetAxis("Mouse Y") * _mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * _mouseSensitivity;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        
        
        //Rotates the player body around the Y axis (up) by the amount specified by "mouseX"
        _playerBody.Rotate(Vector3.up * mouseX);
    }
}

using UnityEngine;

public class CameraControllerAK : MonoBehaviour
{
    [SerializeField] private float _mouseSensitivity = 800.0f;
    [SerializeField] private Transform _playerCameraAnchor;
    [SerializeField] private Transform _cameraTransform;
    private float xRotation = 0.0f;

    private void Awake()
    {
        if (_playerCameraAnchor == null)
        {
            _playerCameraAnchor = GameObject.FindGameObjectWithTag
                                  ("CameraAnchor").transform;
        }

        if (_cameraTransform == null)
        {
            _cameraTransform = GameObject.FindGameObjectWithTag
                               ("MainCamera").transform;

        }
        UpdatePosition();
    }

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        Vector3 currentRotation = transform.rotation.eulerAngles;
        transform.rotation = Quaternion.Euler(new Vector3(0.0f,
                                                          currentRotation.y,
                                                          0.0f));
    }

    private void Update()
    {
        UpdatePosition();
        //float inputScale = Time.fixedDeltaTime * _mouseSensitivity;
        float inputScale = _mouseSensitivity / 100.0f;
        float mouseX = Input.GetAxis("Mouse X") * inputScale;
        float mouseY = Input.GetAxis("Mouse Y") * inputScale;

        transform.Rotate(Vector3.up * mouseX);

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90.0f, 90.0f);
        _cameraTransform.localRotation = Quaternion.Euler(xRotation, 0.0f, 0.0f);
    }

    private void UpdatePosition()
    {
        transform.position = _playerCameraAnchor.position;
    }
}

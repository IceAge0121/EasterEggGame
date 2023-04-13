using UnityEngine;

public class MouseLookAK : MonoBehaviour
{
    [SerializeField] private float _mouseSensitivity = 800.0f;
    [SerializeField] private Transform _playerTransform;
    private float xRotation = 0.0f;

    private void Awake()
    {
        if (_playerTransform == null)
            _playerTransform = GameObject.FindGameObjectWithTag("Player").transform;

        transform.localRotation = Quaternion.Euler(Vector3.zero);
    }

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        float inputScale = Time.deltaTime * _mouseSensitivity;
        //float inputScale = _mouseSensitivity / 100.0f;
        float mouseX = Input.GetAxis("Mouse X") * inputScale;
        float mouseY = Input.GetAxis("Mouse Y") * inputScale;

        _playerTransform.Rotate(Vector3.up * mouseX);

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90.0f, 90.0f);
        transform.localRotation = Quaternion.Euler(xRotation, 0.0f, 0.0f);
    }
}

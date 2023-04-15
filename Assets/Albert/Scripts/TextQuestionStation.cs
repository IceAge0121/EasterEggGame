using TMPro;
using UnityEngine;

public class TextQuestionStation : MonoBehaviour
{
    [SerializeField] private TMP_InputField _inputField;
    [SerializeField] private Transform _inputObject;
    [SerializeField] private CameraControllerAK _cameraController;
    [SerializeField] private PlayerMovement2AK _playerMovement;

    private RaycastHit raycastHit;

    private void Awake()
    {
        if (_inputField == null)
            _inputField = GetComponentInChildren<TMP_InputField>();

        if (_cameraController == null)
            _cameraController = Camera.main.GetComponentInParent<CameraControllerAK>();

        if (_playerMovement == null)
            _playerMovement = GameObject.FindGameObjectWithTag("Player").
                              GetComponent<PlayerMovement2AK>();

        //if (_inputObject == null)
        //_inputObject = GetComponentInChildren<>
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {

        }
    }
}

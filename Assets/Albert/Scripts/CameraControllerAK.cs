using UnityEngine;

public class CameraControllerAK : MonoBehaviour
{
    [SerializeField] private float _mouseSensitivity = 800.0f;
    [SerializeField] private Transform _cameraTransform;
    private float xRotation = -10.0f;

    private bool _canCast = true;
    private RaycastHit _raycastHit;
    private GameObject _highlightedObject;
    [SerializeField] private float _maxInteractionDistance = 10.0f;

    public bool CanCast
    {
        get { return _canCast; }
        set { _canCast = value; }
    }

    private void Awake()
    {
        if (_cameraTransform == null)
            _cameraTransform = Camera.main.transform;
    }

    private void Start()
    {
        LockCursor();
    }

    private void Update()
    {
        UpdateRotation();
        ClearHighlighted();

        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0.0f));

        // If we pass this, it means our raycast hit someting
        if (!Physics.Raycast(ray, out _raycastHit))
            return;

        if (_raycastHit.distance > _maxInteractionDistance)
            return;

        if (!HighlightObject(_raycastHit.transform))
            return;

        if (!_canCast)
            return;

        if (Input.GetMouseButtonDown(0) == false)
            return;

        if (HandlePickup(_raycastHit.transform.gameObject))
            return;

        HandleTextInput(_raycastHit.transform.gameObject);
    }

    // Private methods.
    private void UpdateRotation()
    {
        float inputScale = _mouseSensitivity / 100.0f;
        float mouseX = Input.GetAxis("Mouse X") * inputScale;
        float mouseY = Input.GetAxis("Mouse Y") * inputScale;

        transform.Rotate(Vector3.up * mouseX);

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90.0f, 90.0f);
        _cameraTransform.localRotation = Quaternion.Euler(xRotation, 0.0f, 0.0f);
    }

    private void ClearHighlighted()
    {
        if (_highlightedObject != null)
        {
            _highlightedObject.gameObject.GetComponent<Outline>().enabled = false;
            _highlightedObject = null;
        }
    }

    private bool HighlightObject(Transform toHighlight)
    {
        if (_highlightedObject != null)
        {
            _highlightedObject.GetComponent<Outline>().enabled = false;
            _highlightedObject = null;
        }

        if (toHighlight.gameObject.GetComponent<Outline>() != null)
        {
            _highlightedObject = toHighlight.gameObject;
            _highlightedObject.GetComponent<Outline>().enabled = true;
            return true;
        }
        return false;
    }

    private bool HandlePickup(GameObject pickupObject)
    {
        if (pickupObject.GetComponentInParent<Pickup>() == null)
            return false;

        pickupObject.GetComponentInParent<Pickup>().PickedUp();
        return true;
    }

    private bool HandleTextInput(GameObject textInput)
    {
        if (textInput.GetComponentInParent<TextQuestionStation>() == null)
            return false;

        textInput.GetComponentInParent<TextQuestionStation>().HandleTextInput();
        return true;
    }

    // Public methods.
    public void LockCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void FreeCursor()
    {
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
    }
}

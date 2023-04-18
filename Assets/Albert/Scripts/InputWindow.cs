using TMPro;
using UnityEngine;

[RequireComponent(typeof(TMP_InputField))]
public class InputWindow : MonoBehaviour
{
    [SerializeField] private GameManager _gameManager;
    [SerializeField] private CameraControllerAK _cameraController;
    [SerializeField] private TMP_InputField _inputField;

    private string _answerText;
    private TextQuestionStation _activeriddle;

    private void Awake()
    {
        if (_inputField == null)
            _inputField = GetComponent<TMP_InputField>();

        if (_gameManager == null)
            _gameManager = FindObjectOfType<GameManager>();

        if (_cameraController == null)
            _cameraController = Camera.main.GetComponentInParent<CameraControllerAK>();

        Hide();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return) && gameObject.activeInHierarchy)
        {
            _answerText = _inputField.text;
            ClearInput();
            _activeriddle.CompareAnswer(_answerText);
            _cameraController.CanCast = true;
            Hide();
        }
    }

    private void ClearInput()
    {
        _inputField.text = "";
    }

    public void Show(TextQuestionStation riddleResponsible = null)
    {
        gameObject.SetActive(true);
        _activeriddle = riddleResponsible;
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}

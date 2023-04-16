using TMPro;
using UnityEngine;

public class InputWindow : MonoBehaviour
{
    [SerializeField] private TMP_InputField _inputField;
    [SerializeField] private GameManager _gameManager;
    [SerializeField] private string _answerText;
    private TextQuestionStation _activeriddle;
    [SerializeField] private CameraControllerAK _cameraController;
    private void Awake()
    {
        //Hide();
        if (_inputField == null)
            _inputField = GetComponent<TMP_InputField>();

        if (_gameManager == null)
            _gameManager = GameManager.FindObjectOfType<GameManager>();

        if (_cameraController == null)
            _cameraController = Camera.main.GetComponentInParent<CameraControllerAK>();
    }

    private void Update()
    {
        //Debug.Log(gameObject.activeInHierarchy);
        if (Input.GetKeyDown(KeyCode.Return) && gameObject.activeInHierarchy)
        {
            _answerText = _inputField.text;
            ClearInput();
            _activeriddle.CompareAnswer(_answerText);

            Hide();
        }
    }

    public void Show(TextQuestionStation riddle = null)
    {
        Debug.Log("Command to appear recieved");
        gameObject.SetActive(true);
        _activeriddle = riddle;
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    private void ClearInput()
    {
        _inputField.text = "";
    }
}

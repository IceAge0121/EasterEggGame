using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// This class should be made static in the future.
public class GameManager : MonoBehaviour
{
    [SerializeField] private int _puzzleCount;
    [SerializeField] private CameraControllerAK _cameraController;
    [SerializeField] private PlayerControllerAK _playerController;
    [SerializeField] private TextMeshProUGUI[] _titleTexts;
    [SerializeField] private TextMeshProUGUI[] _completeTexts;
    [SerializeField] private RawImage _blackScreen;
    [SerializeField] private float _textFadeSpeed = 0.1f;

    private bool _gameStarted = false;
    private bool _inputtingText = false;
    private int _pickedUpCount = 0;

    private void Awake()
    {
        if (_cameraController == null)
            _cameraController = Camera.main.GetComponentInParent<CameraControllerAK>();

        if (_playerController == null)
            _playerController = GameObject.FindGameObjectWithTag("Player").
                              GetComponent<PlayerControllerAK>();

        // // Consider changing this.
        DeactivatePlayerControls();
        StartCoroutine(FadeScreen());
        // //
    }

    // // Start of methods to consider changing.
    private void Update()
    {
        if (!_gameStarted && Input.GetMouseButtonDown(0))
            StartTheGame();
    }

    private void StartTheGame()
    {
        ActivatePlayerControls();
        StartCoroutine(FadeOpeningTexts());
        _gameStarted = true;

        Debug.Log($"'_gameStarted' set to {_gameStarted}.");
    }

    public void GameComplete()
    {
        StartCoroutine(ShowEndingText());
        StartCoroutine(ShowScreen());
        Invoke("ResetScene", 7.0f);
    }

    private void ResetScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private IEnumerator FadeOpeningTexts()
    {
        bool keepgoing = true;
        while (keepgoing)
        {
            foreach (TextMeshProUGUI tmp in _titleTexts)
            {
                tmp.alpha -= _textFadeSpeed * Time.deltaTime;
                keepgoing = tmp.alpha >= 0.0f;
            }
            yield return null;
        }
    }

    private IEnumerator ShowEndingText()
    {
        bool keepgoing = true;
        while (keepgoing)
        {
            foreach (TextMeshProUGUI tmp in _completeTexts)
            {
                tmp.alpha += _textFadeSpeed * Time.deltaTime;
                Debug.Log(tmp.alpha);
                keepgoing = tmp.alpha <= 1.0f;
            }
            yield return null;
        }
    }

    private IEnumerator FadeScreen()
    {
        yield return new WaitForSeconds(1.5f);
        DeactivatePlayerControls();
        _blackScreen.CrossFadeAlpha(0.0f, 3.0f, false);
    }

    private IEnumerator ShowScreen()
    {
        yield return new WaitForSeconds(1.0f);
        _blackScreen.CrossFadeAlpha(1.0f, 2.0f, false);
    }
    // // End of methods possbly to change.

    public void IncrementPickedUpCount()
    {
        _pickedUpCount++;

        if (_pickedUpCount >= _puzzleCount)
            GameComplete();
    }

    public void ActivatePlayerControls()
    {
        _cameraController.enabled = true;
        _playerController.enabled = true;
        _cameraController.LockCursor();
    }

    public void DeactivatePlayerControls()
    {
        _cameraController.FreeCursor();
        _cameraController.enabled = false;
        _playerController.enabled = false;
    }
}

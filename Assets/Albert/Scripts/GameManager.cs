using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField] public CameraControllerAK _cameraController;
    [SerializeField] public PlayerMovement2AK _playerController;
    [SerializeField] public TextMeshProUGUI[] _titleTexts;
    [SerializeField] public TextMeshProUGUI[] _completeTexts;
    [SerializeField] public RawImage _blackScreen;
    [SerializeField] public float _textFadeSpeed = 0.1f;

    private bool _gameStarted = false;
    [SerializeField] private int _puzzleCount;
    private int _pickedUpCount = 0;

    //
    //private string _currentRiddleSolution;
    //private string _currentInputAnswer;

    //public void ChangeRiddleSolution(string solution)
    //{
    //    _currentRiddleSolution = solution;
    //}

    //public void ChangeInputAnswer(string answer)
    //{
    //    _currentInputAnswer = answer;
    //}
    //

    private void Awake()
    {
        if (_cameraController == null)
            _cameraController = Camera.main.GetComponentInParent<CameraControllerAK>();

        if (_playerController == null)
            _playerController = GameObject.FindGameObjectWithTag("Player").
                              GetComponent<PlayerMovement2AK>();

        DeactivatePlayerControls();
        StartCoroutine(FadeScreen());
    }

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

    private IEnumerator FadeOpeningTexts()
    {
        bool keepgoing = true;
        while (keepgoing)
        {
            foreach (TextMeshProUGUI tmp in _titleTexts)
            {
                //Debug.Log(tmp.alpha);

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

    public void IncrementPickedUpCount()
    {
        _pickedUpCount++;

        if (_pickedUpCount >= _puzzleCount)
        {
            GameComplete();
        }
    }

    public void ActivatePlayerControls()
    {
        _cameraController.enabled = true;
        _playerController.enabled = true;
    }

    public void DeactivatePlayerControls()
    {
        _cameraController.enabled = false;
        _playerController.enabled = false;
    }
}

using System.Collections;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class TextQuestionStation : MonoBehaviour
{
    [SerializeField] private GameManager _gameManager;

    [SerializeField] private InputWindow _inputWindow;
    [SerializeField] private Transform _inputOptionObject;
    [SerializeField] private GameObject _questionMonument;
    [SerializeField] private string _solutionString;

    [SerializeField] private Vector3 _pickupSpawnOffset;
    [SerializeField] private GameObject _pickupPrefab;

    [SerializeField] private AudioClip _puffClip;
    [SerializeField] private GameObject _poofVFX;
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private AudioClip _selectSound;

    [SerializeField] private AudioClip _correctSound;
    [SerializeField] private AudioClip _incorrectSound;

    private RaycastHit _raycastHit;
    private bool _canCast = true;

    private void Awake()
    {
        if (_gameManager == null)
            _gameManager = FindObjectOfType<GameManager>();

        if (_audioSource == null)
            _audioSource = GetComponent<AudioSource>();

        if (_inputWindow == null)
            _inputWindow = GameObject.FindGameObjectWithTag("QuestionInputField").GetComponent<InputWindow>();

        if (_inputOptionObject == null || _questionMonument == null)
        {
            Transform[] allChildren = GetComponentsInChildren<Transform>();

            foreach (Transform transform in allChildren)
            {
                if (transform.tag == "Selectable")
                    _inputOptionObject = transform;

                if (transform.tag == "QuestionMonument")
                    _questionMonument = transform.gameObject;
            }
        }
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && _canCast)
        {
            Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0.0f));

            if (Physics.Raycast(ray, out _raycastHit))
            {
                if (_raycastHit.transform != _inputOptionObject)
                    return;

                PlaySelectSound();

                _gameManager.DeactivatePlayerControls();
                _canCast = false;
                _inputWindow.Show(this);

                //Debug.Log("Hit input object.");
            }
        }

        if (Input.GetKeyDown(KeyCode.Return) && !_canCast)
        {
            _canCast = true;
        }

        if (Input.GetKeyDown(KeyCode.F) && _questionMonument.activeInHierarchy == false)
        {
            PlayPuffSounds();
            ToolBox.SpawnVFXLocal(_poofVFX, transform);
            _questionMonument.SetActive(true);
        }
    }

    private void HandleTextInput()
    {
        PlaySelectSound();
        _gameManager.DeactivatePlayerControls();
        _inputWindow.Show(this);
    }

    public void CompareAnswer(string answer)
    {
        Debug.Log($"Comparing sol: {_solutionString} with ans: {answer}.");
        if (answer == _solutionString)
        {
            StartCoroutine(CorrectAnswer());
        }
        else
        {
            StartCoroutine(IncorrectAnswer());
        }
    }

    private void SpawnPickupPrefab()
    {
        if (_pickupPrefab != null)
            Instantiate(_pickupPrefab, _questionMonument.transform.position + _pickupSpawnOffset, _questionMonument.transform.rotation);
    }

    private void PlayPuffSounds()
    {
        if (_puffClip != null)
            _audioSource.PlayOneShot(_puffClip, 0.6f);
    }

    private void PlaySelectSound()
    {
        if (_selectSound != null)
            _audioSource.PlayOneShot(_selectSound, 0.6f);
    }

    private IEnumerator DelayedDestroy()
    {
        yield return new WaitForSeconds(5.0f);

        Destroy(gameObject);
    }

    private void PlayIncorrectSound()
    {
        _audioSource.PlayOneShot(_incorrectSound, 0.5f);
    }

    private void PlayCorrectSound()
    {
        _audioSource.PlayOneShot(_correctSound);
    }

    private IEnumerator IncorrectAnswer()
    {
        PlayIncorrectSound();
        yield return new WaitForSeconds(0.5f);
        _gameManager.ActivatePlayerControls();
    }

    private IEnumerator CorrectAnswer()
    {
        PlayCorrectSound();

        yield return new WaitForSeconds(1.0f);
        _gameManager.ActivatePlayerControls();
        PlayPuffSounds();
        //_vfx.SpawnVFX();
        _questionMonument.SetActive(false);

        SpawnPickupPrefab();
        StartCoroutine(DelayedDestroy());
    }
}

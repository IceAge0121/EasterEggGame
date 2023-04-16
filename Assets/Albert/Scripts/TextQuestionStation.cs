using System.Collections;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class TextQuestionStation : MonoBehaviour
{
    [SerializeField] private InputWindow _inputWindow;

    [SerializeField] private GameManager _gameManager;

    [SerializeField] private Transform _inputObject;
    [SerializeField] private CameraControllerAK _cameraController;
    [SerializeField] private PlayerMovement2AK _playerMovement;
    [SerializeField] private TestVFXController _vfx;
    [SerializeField] private GameObject _questionMonument;
    private AudioSource _audioSource;

    [SerializeField] private Vector3 _pickupSpawnOffset;
    [SerializeField] private GameObject _pickupPrefab;

    [SerializeField] private AudioClip _puffClip;
    [SerializeField] private AudioClip _selectSound;

    [SerializeField] private AudioClip _correctSound;
    [SerializeField] private AudioClip _incorrectSound;

    [SerializeField] private string _solutionString;

    private RaycastHit _raycastHit;
    private bool _canCast = true;

    private void Awake()
    {
        if (_gameManager == null)
            _gameManager = GameManager.FindObjectOfType<GameManager>();

        if (_audioSource == null)
            _audioSource = GetComponent<AudioSource>();

        if (_inputWindow == null)
            _inputWindow = GameObject.FindGameObjectWithTag("QuestionInputField").GetComponent<InputWindow>();

        if (_cameraController == null)
            _cameraController = Camera.main.GetComponentInParent<CameraControllerAK>();

        if (_playerMovement == null)
            _playerMovement = GameObject.FindGameObjectWithTag("Player").
                              GetComponent<PlayerMovement2AK>();

        if (_inputObject == null || _questionMonument == null)
        {
            Transform[] allChildren = GetComponentsInChildren<Transform>();

            foreach (Transform transform in allChildren)
            {
                if (transform.tag == "Selectable")
                    _inputObject = transform;

                if (transform.tag == "QuestionMonument")
                    _questionMonument = transform.gameObject;
            }
        }

        if (_vfx == null)
            _vfx = GetComponentInChildren<TestVFXController>();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && _canCast)
        {
            Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0.0f));

            if (Physics.Raycast(ray, out _raycastHit))
            {
                if (_raycastHit.transform != _inputObject)
                    return;

                PlaySelectSound();

                DeactivatePlayer();
                Debug.Log("Commanding input window to appear.");
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
            _vfx.SpawnVFX();
            _questionMonument.SetActive(true);
        }
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
        ActivatePlayer();
    }

    private IEnumerator CorrectAnswer()
    {
        PlayCorrectSound();

        yield return new WaitForSeconds(1.0f);
        ActivatePlayer();
        PlayPuffSounds();
        _vfx.SpawnVFX();
        _questionMonument.SetActive(false);
        _gameManager.IncrementPickedUpCount();

        SpawnPickupPrefab();
        StartCoroutine(DelayedDestroy());
    }

    private void DeactivatePlayer()
    {
        _cameraController.FreeCursor();
        _gameManager.DeactivatePlayerControls();
    }

    private void ActivatePlayer()
    {
        _gameManager.ActivatePlayerControls();
        _cameraController.LockCursor();
    }
}

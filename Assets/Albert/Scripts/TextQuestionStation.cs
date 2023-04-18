using System.Collections;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class TextQuestionStation : MonoBehaviour
{
    [SerializeField] private GameManager _gameManager;
    [SerializeField] private InputWindow _inputWindow;

    // These should be set in the inspector.
    [Header("Assign Question Station Objects")]
    [SerializeField] private GameObject _questionMonument;
    [SerializeField] private Transform _vfxAnchorTransform;
    [SerializeField] private Transform _inputOptionObject;
    [SerializeField] private string _solutionString;

    [Header("Pickup Object")]
    [SerializeField] private GameObject _pickupPrefab;
    [SerializeField] private Vector3 _pickupSpawnOffset;

    [Header("Played on Solution")]
    [SerializeField] private GameObject _poofVFX;

    [Header("Audio")]
    private AudioSource _audioSource;
    [SerializeField] private AudioClip _puffClip;
    private float _puffVolume = 0.6f;
    [SerializeField] private AudioClip _selectSound;
    private float _selectVolume = 1.0f;
    [SerializeField] private AudioClip _correctSound;
    private float _correctVolume = 1.0f;
    [SerializeField] private AudioClip _incorrectSound;
    private float _incorrectVolume = 1.0f;

    private RaycastHit _raycastHit;

    private void Awake()
    {
        if (_gameManager == null)
            _gameManager = FindObjectOfType<GameManager>();

        if (_inputWindow == null)
            _inputWindow = GameObject.FindGameObjectWithTag("QuestionInputField").GetComponent<InputWindow>();

        _audioSource = GetComponent<AudioSource>();
    }

    private IEnumerator IncorrectAnswer()
    {
        ToolBox.PlayAudio(_audioSource, _incorrectSound, _incorrectVolume);

        yield return new WaitForSeconds(0.5f);

        _gameManager.ActivatePlayerControls();
    }

    private IEnumerator CorrectAnswer()
    {
        ToolBox.PlayAudio(_audioSource, _correctSound, _correctVolume);

        yield return new WaitForSeconds(1.0f);

        _gameManager.ActivatePlayerControls();

        ToolBox.PlayAudio(_audioSource, _puffClip, _puffVolume);

        ToolBox.SpawnLocal(_poofVFX, _vfxAnchorTransform.transform);

        _questionMonument.SetActive(false);

        Vector3 pickupSpawnPos = _questionMonument.transform.position + _pickupSpawnOffset;
        Quaternion pickupSpawnRot = _questionMonument.transform.rotation;
        ToolBox.SpawnGlobal(_pickupPrefab, pickupSpawnPos, pickupSpawnRot);
        StartCoroutine(ToolBox.DelayedDestroy(gameObject, 5.0f));
    }

    public void HandleTextInput()
    {
        ToolBox.PlayAudio(_audioSource, _selectSound, _selectVolume);
        _inputWindow.Show(this);
        _gameManager.DeactivatePlayerControls();
    }

    public void CompareAnswer(string answer)
    {
        if (answer == _solutionString)
        {
            StartCoroutine(CorrectAnswer());
        }
        else
        {
            StartCoroutine(IncorrectAnswer());
        }
    }
}

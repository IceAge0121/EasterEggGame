using System.Linq;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]

public class Pickup : MonoBehaviour
{
    private Camera _mainCamera;
    [SerializeField] private GameManager _gameManager;

    [Header("Must assign in inspector")]
    [SerializeField] private GameObject _pickupObject;
    [SerializeField] private GameObject _pickupPersistentEffects;
    [SerializeField] private Vector3 _persistentEffectsPosOffset;

    [SerializeField] private GameObject _onPickupEffects;
    [SerializeField] private Vector3 _onPickupEffectsOffset;
    [SerializeField] private AudioClip _onPickupSound;
    [SerializeField] private float _onPickupSoundVolume;
    private AudioSource _audioSource;

    [SerializeField] private float _destroyDelay = 3.0f;

    private bool _hasBeenPickedUp = false;

    private void Awake()
    {
        if (_gameManager == null)
            _gameManager = FindObjectOfType<GameManager>();

        if (_audioSource == null)
            _audioSource = GetComponent<AudioSource>();

        _mainCamera = Camera.main;
    }

    private void Update()
    {
        ToolBox.MoveToTarget(_pickupPersistentEffects.transform,
                             _pickupObject.transform,
                             _persistentEffectsPosOffset);
    }

    public void PickedUp()
    {
        if (_hasBeenPickedUp == true)
            return;

        _hasBeenPickedUp = true;

        _gameManager.IncrementPickedUpCount();

        if (_onPickupSound != null)
            _audioSource.PlayOneShot(_onPickupSound, _onPickupSoundVolume);

        if (_onPickupEffects != null)
        {
            Vector3 effectPos = _pickupObject.transform.position +
                                _onPickupEffectsOffset;
            Quaternion effectRot = _pickupObject.transform.rotation;
            ToolBox.SpawnVFXGlobal(_onPickupEffects, effectPos, effectRot);
        }

        Transform[] childrenTransforms = transform.Cast<Transform>().ToArray();
        ToolBox.DisableObjects(childrenTransforms);

        StartCoroutine(ToolBox.DelayedDestroy(gameObject, _destroyDelay));
    }
}

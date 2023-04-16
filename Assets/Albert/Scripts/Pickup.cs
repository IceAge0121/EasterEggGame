using System.Collections;
using UnityEngine;

public class Pickup : MonoBehaviour
{
    [SerializeField] private GameManager _gameManager;
    private Camera _mainCamera;
    [SerializeField] private TestVFXController _vfxController;
    [SerializeField] private Vector3 _vfxSpawnOffset;
    [SerializeField] private AudioClip _pickupSound;
    [SerializeField] private GameObject[] _children;
    [SerializeField] private GameObject _pickupOrb;

    private bool _hasBeenPickedUp = false;
    private AudioSource _audioSource;

    private RaycastHit _raycastHit;

    private void Awake()
    {
        if (_gameManager == null)
            _gameManager = GameManager.FindObjectOfType<GameManager>();

        if (_audioSource == null)
            _audioSource = GetComponent<AudioSource>();

        if (_vfxController == null)
            _vfxController = GetComponent<TestVFXController>();

        _mainCamera = Camera.main;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && !_hasBeenPickedUp)
        {
            Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0.0f));

            if (Physics.Raycast(ray, out _raycastHit))
            {
                if (_raycastHit.transform != _pickupOrb.transform)
                    return;

                PickedUp();
            }
        }
    }

    private void PickedUp()
    {
        _hasBeenPickedUp = true;

        _gameManager.IncrementPickedUpCount();

        if (_pickupSound != null)
            _audioSource.PlayOneShot(_pickupSound, 0.2f);

        _vfxController.SpawnVFXGlobal(_pickupOrb.transform.position + _vfxSpawnOffset, _pickupOrb.transform.rotation);

        DisableChildren();

        StartCoroutine(DelayedDestroy());
    }

    private void DisableChildren()
    {
        foreach (GameObject gameObject in _children)
        {
            gameObject.SetActive(false);
        }
    }

    private IEnumerator DelayedDestroy()
    {
        yield return new WaitForSeconds(3.0f);

        Destroy(this.gameObject);
    }
}

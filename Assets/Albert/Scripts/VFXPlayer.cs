using UnityEngine;

public class VFXPlayer : MonoBehaviour
{
    [SerializeField] GameObject _vfxObjectPrefab;
    private ParticleSystem _particleSystem;

    private void Update()
    {
        SpawnParticles();
    }

    private void SpawnParticles()
    {
        if (_vfxObjectPrefab == null)
            return;

        if (Input.GetKeyDown(KeyCode.E))
        {
            GameObject vfxInstance = Instantiate(_vfxObjectPrefab, transform);

            _particleSystem = vfxInstance.GetComponentInChildren<ParticleSystem>();

            if (_particleSystem == null)
                return;

            _particleSystem.Play();
        }
    }
}

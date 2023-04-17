using UnityEngine;
using UnityEngine.VFX;

[RequireComponent(typeof(VisualEffect))]
public class TemporaryVFX : MonoBehaviour
{
    [SerializeField] private VisualEffect _thisVFX;
    [SerializeField] private float _animationDuration = 1.0f;
    private float _initializationTime;
    private float _elapsedTime = 0.0f;

    private void Awake()
    {
        if (_thisVFX == null)
            _thisVFX = GetComponent<VisualEffect>();
    }

    private void Start()
    {
        _initializationTime = Time.timeSinceLevelLoad;
    }

    private void Update()
    {
        _elapsedTime = Time.timeSinceLevelLoad - _initializationTime;

        if (_elapsedTime > _animationDuration && _thisVFX.aliveParticleCount <= 0)
            Destroy(transform.gameObject);
    }
}

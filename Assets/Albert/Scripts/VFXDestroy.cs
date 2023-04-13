using UnityEngine;
using UnityEngine.VFX;

[RequireComponent(typeof(VisualEffect))]
public class VFXDestroy : MonoBehaviour
{
    [SerializeField] private VisualEffect _thisVFX;
    [SerializeField] private float _animationDuration = 1.0f;
    private float _initializationTime;
    private float _elapsedTime = 0.0f;

    private void Awake()
    {
        _thisVFX = GetComponent<VisualEffect>();
    }

    private void Start()
    {
        _initializationTime = Time.timeSinceLevelLoad;
    }

    private void Update()
    {
        _elapsedTime = Time.timeSinceLevelLoad - _initializationTime;

        //Debug.Log(_thisVFX.aliveParticleCount);
        if (_elapsedTime > _animationDuration && _thisVFX.aliveParticleCount <= 0)
        {
            //Debug.Log("Want to destory.");
            Destroy(transform.gameObject);
        }
    }
}

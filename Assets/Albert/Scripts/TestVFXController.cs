using UnityEngine;

public class TestVFXController : MonoBehaviour
{
    [SerializeField] private GameObject _vfx;

    public void SpawnVFX()
    {
        if (_vfx != null)
            Instantiate(_vfx, transform);
    }

    public void SpawnVFXGlobal(Vector3 inPosition, Quaternion inRotation)
    {
        if (_vfx != null)
            Instantiate(_vfx, inPosition, inRotation);
    }

    public void SpawnVFXToTransform(Transform transform)
    {
        if (_vfx != null)
            Instantiate(_vfx, transform);
    }
}

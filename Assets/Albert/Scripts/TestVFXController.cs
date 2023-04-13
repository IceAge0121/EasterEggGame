using UnityEngine;

public class TestVFXController : MonoBehaviour
{
    [SerializeField] private GameObject _vfx;

    private void Update()
    {
        if (_vfx != null && Input.GetMouseButtonDown(0))
            Instantiate(_vfx, transform);
    }
}

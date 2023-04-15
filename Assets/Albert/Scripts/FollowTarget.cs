using UnityEngine;

public class FollowTarget : MonoBehaviour
{
    [SerializeField] private Transform _targetTransform;
    [SerializeField] private Vector3 _offset;

    private void Update()
    {
        if (_targetTransform != null)
            transform.localPosition = _targetTransform.localPosition + _offset;
    }
}

using System.Collections;
using UnityEngine;

public static class ToolBox
{
    public static void MoveToTarget(Transform toMove, Transform target, Vector3 offset)
    {
        toMove.position = target.position + offset;
    }

    public static void SpawnVFXLocal(GameObject vfxPrefab, Transform transform)
    {
        if (vfxPrefab != null && transform != null)
            Object.Instantiate(vfxPrefab, transform);
    }

    public static void SpawnVFXGlobal(GameObject vfxPrefab, Vector3 position, Quaternion rotation)
    {
        if (vfxPrefab != null)
            Object.Instantiate(vfxPrefab, position, rotation);
    }

    public static void DisableObjects(Transform[] toDisable)
    {
        for (int i = 0; i < toDisable.Length; i++)
        {
            toDisable[i].gameObject.SetActive(false);
        }
    }

    public static IEnumerator DelayedDestroy(GameObject gameObject, float destroyDelay)
    {
        yield return new WaitForSeconds(3.0f);
        Object.Destroy(gameObject);
    }

    private static float ReturnHorizontalMagnitude(Vector3 input)
    {
        Vector3 horizontalVector = new Vector3(input.x, 0.0f, input.z);
        return horizontalVector.magnitude;
    }
}

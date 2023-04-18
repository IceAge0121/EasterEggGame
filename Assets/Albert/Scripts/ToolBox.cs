using System.Collections;
using UnityEngine;

public static class ToolBox
{
    public static void MoveToTarget(Transform toMove, Transform target, Vector3 offset)
    {
        toMove.position = target.position + offset;
    }

    public static void SpawnLocal(GameObject prefab, Transform transform)
    {
        if (prefab != null && transform != null)
            Object.Instantiate(prefab, transform);
    }

    public static void SpawnGlobal(GameObject prefab, Vector3 position, Quaternion rotation)
    {
        if (prefab != null)
            Object.Instantiate(prefab, position, rotation);
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
        yield return new WaitForSeconds(destroyDelay);
        Object.Destroy(gameObject);
    }

    public static Vector3 ReturnHorizontal(Vector3 input, bool norm)
    {
        Vector3 horizontalInput = input;
        horizontalInput.y = 0;

        if (norm)
            return horizontalInput.normalized;

        return horizontalInput;
    }

    public static void PlayAudio(AudioSource source, AudioClip clip, float volume = 1.0f)
    {
        if (clip != null)
        {
            source.PlayOneShot(clip, volume);
        }
    }
}

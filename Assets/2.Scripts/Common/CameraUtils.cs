using UnityEngine;

public static class CameraUtils
{
    private static float GetCameraToWorldZDistance(Camera cam, float worldZ)
    {
        float distance = Vector3.Dot((new Vector3(0f, 0f, worldZ) - cam.transform.position), cam.transform.forward);
        return distance;
    }

    public static Vector3 GetOrthographicBottomLeftWorld(Camera cam, float worldZ)
    {
        if (cam == null || !cam.orthographic)
            return Vector3.zero;

        float distance = GetCameraToWorldZDistance(cam, worldZ);
        Vector3 p = cam.ViewportToWorldPoint(new Vector3(0f, 0f, distance));
        p.z = worldZ;
        return p;
    }

    public static Vector3 GetOrthographicBottomRightWorld(Camera cam, float worldZ)
    {
        if (cam == null || !cam.orthographic)
            return Vector3.zero;

        float distance = GetCameraToWorldZDistance(cam, worldZ);
        Vector3 p = cam.ViewportToWorldPoint(new Vector3(1f, 0f, distance));
        p.z = worldZ;
        return p;
    }

    public static Vector3 GetOrthographicTopLeftWorld(Camera cam, float worldZ)
    {
        if (cam == null || !cam.orthographic)
            return Vector3.zero;

        float distance = GetCameraToWorldZDistance(cam, worldZ);
        Vector3 p = cam.ViewportToWorldPoint(new Vector3(0f, 1f, distance));
        p.z = worldZ;
        return p;
    }

    public static Vector3 GetOrthographicTopRightWorld(Camera cam, float worldZ)
    {
        if (cam == null || !cam.orthographic)
            return Vector3.zero;

        float distance = GetCameraToWorldZDistance(cam, worldZ);
        Vector3 p = cam.ViewportToWorldPoint(new Vector3(1f, 1f, distance));
        p.z = worldZ;
        return p;
    }

    public static Vector2 GetWorldSizeOrthographic(Camera cam)
    {
        if (cam == null || !cam.orthographic)
            return Vector2.zero;

        float height = cam.orthographicSize * 2f;
        float width = height * cam.aspect;
        return new Vector2(width, height);
    }
}
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

namespace SharedUtils {
public static class CameraExtension {
    public static float WorldToScreenDistance(this Camera cam, float worldDistance) {
        var pixelsPerUnit = Screen.height / cam.orthographicSize / 2f;
        return worldDistance * pixelsPerUnit;
    }
}
}
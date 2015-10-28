using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class CameraEffectBase : MonoBehaviour
{
    public Material material;

    protected virtual void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        Graphics.Blit(source, destination, material);
    }
}

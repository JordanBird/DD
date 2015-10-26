using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class CameraReduceResolution : CameraEffectBase
{
    public int pixelsAcross = 320;
    public FilterMode filterMode = FilterMode.Point;
    private RenderTexture _renderTexture;

    protected virtual void OnEnable()
    {
        CreateRenderTexture();
    }

    protected virtual void OnDisable()
    {
        _DestroyRenderTexture();
    }

    protected override void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        // Blit source to the render texture with the default material.
        Graphics.Blit(source, _renderTexture);

        // Blit the render texture to the destination and apply the shader (resolution downscale might mean more efficient processing?).
        Graphics.Blit(_renderTexture, destination, material);
    }

    /// <summary>
    /// Creates the render texture to use. Call this whenever pixelsAcross or filterMode are changed.
    /// </summary>
    public void CreateRenderTexture()
    {
        _DestroyRenderTexture();

        if (pixelsAcross <= 0)
        {
            enabled = false;
            throw new UnityException("Disabling CameraReduceResolution component. Pixels Across value is invalid.");
        }

        int pixelsDown = (int)(pixelsAcross * Screen.height / Screen.width);
        _renderTexture = new RenderTexture(pixelsAcross, pixelsDown, 24);
        _renderTexture.filterMode = filterMode;
    }

    private void _DestroyRenderTexture()
    {
        if (_renderTexture)
        {
            DestroyImmediate(_renderTexture);
        }
    }
}

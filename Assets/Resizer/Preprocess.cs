using UnityEngine;

public static class Preprocess
{
    public static Texture2D ScaleAndCropImage(WebCamTexture webCamTexture, int desiredSize)
    {
        var scale = new Vector2(1, 1);
        var offset = Vector2.zero;
        var renderTexture = new RenderTexture(desiredSize, desiredSize, 0, RenderTextureFormat.ARGB32);
        scale.x = (float) webCamTexture.height / webCamTexture.width;
        offset.x = (1 - scale.x) / 2f;
        Graphics.Blit(webCamTexture, renderTexture, scale, offset);
        return Rotate270(ToTexture2D(renderTexture));
    }

    private static Texture2D ToTexture2D(RenderTexture textureToConvert)
    {
        var texture = new Texture2D(textureToConvert.width, textureToConvert.height, TextureFormat.RGB24, false);
        var renderTextureToSave = RenderTexture.active;
        RenderTexture.active = textureToConvert;

        var textureWidth = textureToConvert.width;
        var textureHeight = textureToConvert.height;
        
        texture.ReadPixels(new Rect(0, 0, textureWidth, textureHeight), 0, 0);
        texture.Apply();

        RenderTexture.active = renderTextureToSave;
        return texture;
    }

    private static Texture2D Rotate270(Texture2D textureToRotate)
    {
        var textureWidth = textureToRotate.width;
        var textureHeight = textureToRotate.height;
        
        var texturePixels = textureToRotate.GetPixels32(0);
        var newTexturePixels = new Color32[textureWidth * textureHeight];
        var i = 0;
        for (var y = 0; y < textureHeight; y++)
        {
            for (var x = 0; x < textureWidth; x++)
            {
                newTexturePixels[textureWidth * textureHeight - (textureHeight * x + textureHeight) + y] = texturePixels[i];
                i++;
            }
        }

        var newTexture = new Texture2D(textureHeight, textureWidth, textureToRotate.format, false);
        newTexture.SetPixels32(newTexturePixels, 0);
        newTexture.Apply();
        return newTexture;
    }
}
using UnityEngine;
using UnityEngine.UI;

public class ViewPrepare : MonoBehaviour
{
    public WebCamTexture WebcamTexture { get; private set; }

    [SerializeField] private RawImage rawImage;
    [SerializeField] private AspectRatioFitter fitter;

    private bool _ratioSet;

    private void Awake()
    {
        _ratioSet = false;

        var cameraName = WebCamTexture.devices[0].name;
        WebcamTexture = new WebCamTexture(cameraName, Screen.width, Screen.height / 2, 30);
        rawImage.texture = WebcamTexture;
        WebcamTexture.Play();
    }

    private void Update()
    {
        if (WebcamTexture.width <= 100 || _ratioSet) return;
        _ratioSet = true;
        SetAspectRatio();
    }

    private void OnDestroy()
    {
        WebcamTexture?.Stop();
    }

    private void SetAspectRatio()
    {
        fitter.aspectRatio = (float) WebcamTexture.width / WebcamTexture.height;
    }
}
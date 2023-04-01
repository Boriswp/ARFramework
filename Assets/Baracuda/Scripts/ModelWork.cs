using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.Barracuda;
using UnityEngine;
using UnityEngine.UI;


public class ModelWork : MonoBehaviour
{
    [SerializeField] private NNModel modelAsset;
    [SerializeField] private MenuSegmentation menu;
    [SerializeField] private UIController uIController;
    [SerializeField] private TextAsset labelMap;
    
    private Model _runtimeModel;
    private IWorker _worker;
    private string[] _labels;
    const int SIZE = 224;
    private ViewPrepare _viewPrepare;

    private void Awake()
    {
        _viewPrepare = GetComponent<ViewPrepare>();
        _runtimeModel = ModelLoader.Load(modelAsset);
        _labels = labelMap.text.Split('\n');
    }

    private void Start()
    {
        _worker = WorkerFactory.CreateWorker(WorkerFactory.Type.Auto, _runtimeModel);
    }

    private void Update()
    {
        if (Time.frameCount % 10 != 0) return;
        WebCamTexture webCamTexture = _viewPrepare.WebcamTexture;

        if (webCamTexture.didUpdateThisFrame && webCamTexture.width > 100)
        {
            StartCoroutine(Runner(Preprocess.ScaleAndCropImage(webCamTexture, SIZE)));
        }
    }

    private void OnDestroy()
    {
        StopAllCoroutines();
        _worker.Dispose();
    }

    private IEnumerator Runner(Texture2D texture2D)
    {

        var inputTensor = new Tensor(texture2D, 3);

        _worker.Execute(inputTensor);

        var outputTensor = _worker.PeekOutput("output");
        List<float> temp = outputTensor.ToReadOnlyArray().ToList();
        float max = temp.Max();
        int index = temp.IndexOf(max);

        var line = _labels[index];
        if (max > 8f)
        {
            uIController.SetOutputText( $"{line} \n score: {max}");
            menu.FindLabel(line);   
        }
        inputTensor.Dispose();
        outputTensor.Dispose();
        yield return null;
    }
}

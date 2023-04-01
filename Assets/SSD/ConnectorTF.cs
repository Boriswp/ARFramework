using System.IO;
using System.Linq;
using UnityEngine;
using TensorFlowLite;

public class ConnectorTF : MonoBehaviour
{
    [SerializeField, FilePopup("*.tflite")]
    private string fileName = "coco_ssd_mobilenet_quant.tflite";
    
    [SerializeField, Range(0f, 1f)] private float scoreThreshold = 0.5f;
    [SerializeField] private TextAsset labelMap = null;
    [SerializeField] private UIController uiController;
    [SerializeField] private MenuSegmentation menu;

    private ViewPrepare _viewPrepare;
    private PredictorTF.Result[] _results;
    private PredictorTF _predictorTf;
    private string[] _labels;

    private void Awake()
    {
        var path = Path.Combine(Application.streamingAssetsPath, fileName);
        _viewPrepare = GetComponent<ViewPrepare>();
        _predictorTf = new PredictorTF(path);
        _labels = labelMap.text.Split('\n');
    }

    private void Update()
    {
        if (Time.frameCount % 10 != 0) return;
        _predictorTf.Invoke(_viewPrepare.WebcamTexture);
        _results = _predictorTf.GetResults();
        SetResult(_results);
    }

    private void OnDestroy()
    {
        _predictorTf?.Dispose();
    }

    private void SetResult(PredictorTF.Result[] results)
    {
        var classId = 0;
        var maxScore = results.Max(res =>
        {
            classId = res.classID;
            return res.score;
        });
        
        if (!(maxScore > scoreThreshold)) return;
        var label = GetLabelName(classId);
        var outputText = $"{GetLabelName(classId)}\n{(int) (maxScore * 100)}%";
        menu.FindLabel(label);
        uiController.SetOutputText(outputText);
    }

    private string GetLabelName(int id)
    {
        if (id < 0 || id >= _labels.Length - 1)
        {
            return "?";
        }

        return _labels[id + 1];
    }
}
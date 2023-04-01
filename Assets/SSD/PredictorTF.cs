using UnityEngine;

namespace TensorFlowLite
{
    public class PredictorTF : BaseImagePredictor<sbyte>
    {
        public struct Result
        {
            public int classID;
            public float score;
        }

        private const int objectsCount = 10;

        private float[] _classesOutput = new float[objectsCount];
        private float[] _scoresOutput = new float[objectsCount];
        private Result[] _results = new Result[objectsCount];

        public PredictorTF(string modelPath) : base(modelPath, true)
        {
        }


        public override void Invoke(Texture inputTex)
        {
            ToTensor(inputTex, input0);

            interpreter.SetInputTensorData(0, input0);
            interpreter.Invoke();
            interpreter.GetOutputTensorData(1, _classesOutput);
            interpreter.GetOutputTensorData(2, _scoresOutput);
        }

        public Result[] GetResults()
        {
            for (var i = 0; i < objectsCount; i++)
            {
                _results[i] = new Result
                {
                    classID = (int) _classesOutput[i],
                    score = _scoresOutput[i]
                };
            }

            return _results;
        }
    }
}
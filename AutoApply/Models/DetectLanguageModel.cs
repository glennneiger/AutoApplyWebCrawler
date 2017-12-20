using System.Collections.Generic;

namespace AutoApply
{

    public class Detection
    {
        public string Language { get; set; }
        public bool IsReliable { get; set; }
        public float Confidence { get; set; }
    }

    public class ResultData
    {
        public List<Detection> Detections { get; set; }
    }

    public class Result
    {
        public ResultData Data { get; set; }
    }
}

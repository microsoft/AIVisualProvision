using System;
using System.Collections.Generic;

namespace VisualProvision.Services.Recognition
{
    public class PredictionResult
    {
        public string Id { get; set; }

        public string Project { get; set; }

        public string Iteration { get; set; }

        public DateTime Created { get; set; }

        public List<Prediction> Predictions { get; set; }
    }
}

namespace VisualProvision.Services.Recognition
{
    // <snippet_prediction_class>
    public class Prediction
    {
        public double Probability { get; set; }

        public string TagId { get; set; }

        public string TagName { get; set; }

        public BoundingBox BoundingBox { get; set; }
    }
     
    // </snippet_prediction_class>
}

namespace VisualProvision
{
    public static class AppSettings
    {
        // Photos storage
        public const string PhotoFolder = "VisualProvision";

        /* 
         * Service principal
         *
         * Note: This settings will only be used in Debug mode to avoid developer having to enter
         * ClientId and TenantId keys each time application is deployed.
         * In Release mode, all credentials will be introduced using UI input fields.
         */
        public const string ClientId = "INSERT YOUR CLIENTID HERE";
        public const string TenantId = "INSERT YOUR TENANTID HERE";

        // App Center (Feel free to change the following IDs with your App Center IDs).
        public const string AppCenterAndroid = "c8fbe0d5-f676-40b9-927d-19f70365f7de";
        public const string AppCenterIos = "a43c421b-70ac-4742-905a-24dc760696de";

        // Custom Vision
        // URL example: https://southcentralus.api.cognitive.microsoft.com/customvision/v2.0/Prediction/{GUID}/image
        public const string CustomVisionPredictionUrl = "INSERT YOUR COMPUTER VISION API URL HERE FOR MAGNETS RECOGNITION";
        public const string CustomVisionPredictionKey = "INSERT YOUR COMPUTER VISION PREDICTION KEY HERE FOR MAGNETS RECOGNITION";

        // Computer Vision
        // Endpoint example: https://westus.api.cognitive.microsoft.com/
        public const string ComputerVisionEndpoint = "INSERT COMPUTER VISION ENDPOINT HERE FOR HANDWRITING";
        public const string ComputerVisionKey = "INSERT YOUR COMPUTER VISION KEY HERE FOR HANDWRITING";
    }
}

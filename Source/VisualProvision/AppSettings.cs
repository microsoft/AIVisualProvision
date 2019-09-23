namespace VisualProvision
{
    public static class AppSettings
    {
        // Photos storage
        public const string PhotoFolder = "VisualProvision";

        // <snippet_serviceprincipal>
        /* 
         * Service principal
         *
         * Note: This settings will only be used in Debug mode to avoid developer having to enter
         * ClientId and TenantId keys each time application is deployed.
         * In Release mode, all credentials will be introduced using UI input fields.
         */
        public const string ClientId = "INSERT YOUR CLIENTID HERE";
        public const string TenantId = "INSERT YOUR TENANTID HERE";
        // </snippet_serviceprincipal>

        // App Center
        public const string AppCenterAndroid = "INSERT YOUR APP CENTER IDENTIFIER FOR ANDROID HERE";
        public const string AppCenterIos = "INSERT YOUR APP CENTER IDENTIFIER FOR IOS APP HERE";

        // <snippet_cusvis_keys>
        // Custom Vision
        // URL example: https://southcentralus.api.cognitive.microsoft.com/customvision/v2.0/Prediction/{GUID}/image
        public const string CustomVisionPredictionUrl = "INSERT YOUR COMPUTER VISION API URL HERE FOR MAGNETS RECOGNITION";
        public const string CustomVisionPredictionKey = "INSERT YOUR COMPUTER VISION PREDICTION KEY HERE FOR MAGNETS RECOGNITION";
        // </snippet_cusvis_keys>

        // <snippet_comvis_keys>
        // Computer Vision
        // Endpoint example: https://westus.api.cognitive.microsoft.com/
        public const string ComputerVisionEndpoint = "INSERT COMPUTER VISION ENDPOINT HERE FOR HANDWRITING";
        public const string ComputerVisionKey = "INSERT YOUR COMPUTER VISION KEY HERE FOR HANDWRITING";
        // </snippet_comvis_keys>

    }
}
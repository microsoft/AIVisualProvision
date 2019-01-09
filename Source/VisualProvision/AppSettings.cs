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
        // We are providing public endpoint in a free tier to showcase how the app works, feel free to replace these values with your own cognitive services.
        public const string CustomVisionPredictionUrl = "https://southcentralus.api.cognitive.microsoft.com/customvision/v2.0/Prediction/41deb215-1c77-4679-81dd-5025c3998dbf/image";
        public const string CustomVisionPredictionKey = "47c6b38bf1e245449f01405c144b9aef";

        // Computer Vision
        // Endpoint example: https://westus.api.cognitive.microsoft.com/
         // We are providing public endpoint in a free tier to showcase how the app works, feel free to replace these values with your own cognitive services.
        public const string ComputerVisionEndpoint = "https://westus.api.cognitive.microsoft.com";
        public const string ComputerVisionKey = "1579a0d8658044ad9d3d75a94935699a";
    }
}

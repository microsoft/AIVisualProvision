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
         * Note: This settings will only be used in Debug mode to avoid developer having to enter18
         * ClientId and TenantId keys each time application is deployed.
         * In Release mode, all credentials will be introduced using UI input fields.
         */
        public const string ClientId = "INSERT YOUR CLIENTID HERE";
        public const string TenantId = "INSERT YOUR TENANTID HERE";
        
        // </snippet_serviceprincipal>

        // App Center (Feel free to change the following IDs with your App Center IDs).
        public const string AppCenterAndroid = "c8fbe0d5-f676-40b9-927d-19f70365f7de";
        public const string AppCenterIos = "a43c421b-70ac-4742-905a-24dc760696de";

        // <snippet_cusvis_keys>
        // Custom Vision
        public const string CustomVisionPredictionUrl = "INSERT YOUR COMPUTER VISION API URL HERE FOR MAGNETS RECOGNITION";
        public const string CustomVisionPredictionKey = "INSERT YOUR COMPUTER VISION PREDICTION KEY HERE FOR MAGNETS RECOGNITION";
        
        // </snippet_cusvis_keys>

        // <snippet_comvis_keys>
        // Computer Vision
        public const string ComputerVisionEndpoint = "INSERT COMPUTER VISION ENDPOINT HERE FOR HANDWRITING";
        public const string ComputerVisionKey = "INSERT YOUR COMPUTER VISION KEY HERE FOR HANDWRITING";
        
        // </snippet_comvis_keys>
    }
}

# Developing Intelligent Apps with Azure

## Key Takeaway

   - With the flexible Azure platform and a wide portfolio of AI productivity tools, developers can build the next generation of smart applications where their data lives, in the intelligent cloud, on-premises, and on the intelligent edge.

   - Teams can achieve more with the comprehensive set of flexible and trusted AI services - from pre-built APIs, such as Cognitive Services and Conversational AI with Bot tools, to building custom models with Azure Machine Learning for any scenario.

 # Before you begin

1. **Visual Studio 2017 or 2019 Preview 1** with Xamarin workloads installed.

1. **Android Emulator** with Hyper-V Windows Feature installed as per this [document](https://docs.microsoft.com/en-us/xamarin/android/get-started/installation/android-emulator/hardware-acceleration?tabs=vswin&pivots=windows).

1. **Android SDK 28** installed (Android Pie 9).

1. Active Azure subscription.

1. Working on local machine is preferred in order to use camera through emulator. However, if you are working on a VM, the emulator will not be able to access your camera. Follow [these steps](https://stackoverflow.com/questions/50698027/how-can-i-use-my-webcam-inside-an-azure-windows-server-virtual-machine) to enable local camera access for your VM.

1. Install [Azure CLI](https://azurecliprod.blob.core.windows.net/msi/azure-cli-2.0.45.msi) version 2.0.45.

 ## Walkthrough: Configure AIVisualProvision App

  AI Visual Provision mobile app uses Azure Cognitive Services (Computer Vision and Custom Vision) to deploy whiteboard drawings to an Azure architecture. It uses Cognitive Services to detect the Azure services among Azure Service logos and handwriting using the phone camera. The captured image is analyzed and the identified services are deployed to Azure taking away all the pain and complexity from the process. You can see how to create Azure Cognitive Services and configure them in this mobile app.

 1. Open your Visual Studio 2019 Preview in Administrator mode and click on **Clone or checkout code**.

    ![](Images/LandingPage_VS2019_1.png)
 
 1. Copy and paste the URL: https://github.com/Microsoft/AIVisualProvision in **Code repository location** textbox and click **Clone** button.

    ![](Images/CloningRepo_2.png)
 
 1. While the cloning is still in progress, the code is already available for use. It has brought up the folder view. Before loading the solution in Visual Studio 2019 Preview, navigate to ***AIVisualProvision/Source/VisualProvision.iOS/*** and **delete** the folder ***Assets.xcassets*** as per workaround mentioned [here](https://developercommunity.visualstudio.com/content/problem/398522/vs-2019-preview-xamarin-load-fails.html) (which is supposed to be fixed in coming update). Double click on the solution ***VisualProvision.sln*** to load it in Solution Explorer.

    ![](Images/FolderView_3.png)

    ![](Images/DeleteAssets.xcassets_4.png)
 
 1. Now you can see that 4 projects are successfully loaded under the solution.

    ![](Images/LoadSolution_5.png)

1. Navigate to **AppSettings.cs** file under **VisualProvision** project. You need to provide the **Client ID** and **tenantId** if you are running the app in Emulator in the Debug mode so that you don't have to enter it all the time you debug. Also, the **CustomVisionPredictionUrl**, **CustomVisionPredictionKey**, **ComputerVisionEndpoint** and **ComputerVisionKey** has to be entered.

    ![](Images/AppSettingsFile_6.png)

1. In order to get the **Client ID** and **tenantId**, type **az login** in the command prompt and press Enter. Authorize your login in the browser.

   Type **az ad sp create-for-rbac -n “MySampleApp” -p P2SSWORD** in the command prompt to get the Service Principal Client ID and the Service Principal Client Secret.

   Copy and note down the **appId** which is the **Client ID**
   
   Copy and note down the **tenantId**

   **P2SSWORD** is the Client Secret or **Password**, which will be required to run the app.

   ![](Images/SPNDetails_7.png)

1. Switch to Visual Studio 2019 and paste the **Client ID** and **TenantId** in the **AppSettings.cs** file.

   ![](Images/PasteSPNDetails_8.png)

1. You need a Computer Vision service in order to use the handwriting recognition features of the app. To create your own Computer Vision instance navigate to this [page](https://azure.microsoft.com/en-us/try/cognitive-services). Click **Get API Key** button under Computer Vision.

   ![](Images/ComputerVision1_9.png)

   You can refer to this [link](https://docs.microsoft.com/en-us/azure/cognitive-services/computer-vision/vision-api-how-to-topics/howtosubscribe) for more details on Computer Vision service.

1. Click **Sign In** and login to your Azure account.

   ![](Images/ComputerVision2_10.png)

1. In the Azure portal, you will be prompted to create a new **Computer Vision** service. Provide **Name**, **Subscription**, **Location**, **Pricing tier** and **Resource group**. Click **Create**.

   ![](Images/CreateComputerVision_11.png) 

1. Once you have provisioned the service, note down the service endpoint **URL** and endpoint **Key** as per the Images below:

   ![](Images/ComputerVision3_12.png)

   ![](Images/ComputerVision4_13.png)

1. Switch to Visual Studio 2019 and paste the copied **URL** under **ComputerVisionEndpoint** and **Key** under **ComputerVisionKey** in the **AppSettings.cs** file.

   ![](Images/PasteComputerVision_14.png)

1. If you wish to use only the handwriting recognition service in your app, skip to the next section to Build and Run the app. But, if you wish to add in-app logo recognition to your app, continue with next step.

1. The in-app logo recognition is accomplished by using Azure Custom Vision. In order to use the service in the app, you need to create a new Custom Vision project and train it with the Images provided in the repo under the **documents/training_dataset** folder. Go to [Azure Custom Vision](https://azure.microsoft.com/en-us/services/cognitive-services/custom-vision-service/) page and click **Get started**, and **Sign In** using your Azure credentials.

   ![](Images/CustomVision1_15.png)

   ![](Images/CustomVision2_16.png)

1. Agree to terms by clicking **Yes** and **I agree** respectively.

   ![](Images/CustomVision3_17.png)

   ![](Images/CustomVision4_18.png)

1. You will be taken to the landing page of your Azure Custom Vision account. Click **New Project** and provide a **Name**. Leave rest options as it is and click **Create project**.

   ![](Images/CustomVision5_19.png)

1. Click on **Add Images**. Upload an image of **Azure App Service** logo and tag it as **APP_SERVICE**. If you don't have the magnets or Images handy, refer to included 2 PDFs ([sheet1](https://github.com/Microsoft/AIVisualProvision/blob/master/Documents/AzureMagnets1.pdf), [sheet2](https://github.com/Microsoft/AIVisualProvision/blob/master/Documents/AzureMagnets2.pdf)) with all the magnet logos. Make sure you cut only the App Service image from the pdf. Select **Done** once the Images have been uploaded.

   ![](Images/CustomVision6_20.png)

   ![](Images/CustomVision7_21.png)

   ![](Images/CustomVision8_22.png)

1. In order to **Train** you need to have at least 2 tags and 5 Images for every tag.  Return to the previous step of this section and repeat the step to add at least 5 Images for **APP_SERVICE** and **SQL_DATABASE** tags. To train the classifier, select the **Train** button.

   ![](Images/CustomVision9_23.png)

   You should use the following set of tags, as they are the expected tags in the application. Tags are located at **VisualProvision\Services\Recognition\RecognitionService.cs** file.

   ![](Images/CustomVision10_24.png)

1. The classifier uses all of the current Images to create a model that identifies the visual qualities of each tag.

   ![](Images/CustomVision11_25.png)

1. Optionally, to verify the accuracy click on **Quick Test** and provide any similar **Image URL** of either App Service or SQL Database. 

   ![](Images/CustomVision12_26.png)

1. Under **Performance** tab, click on **Prediction URL** and note down the service endpoint **URL** and endpoint **Key** as per the Images below:   

   ![](Images/CustomVision13_27.png)

1. Switch to Visual Studio 2019 and paste the copied **URL** under **CustomVisionPredictionUrl** appended by **/image**, and **Key** under **CustomVisionPredictionKey** in the **AppSettings.cs** file. Click **Save**.

   ![](Images/CustomVision14_28.png)

 
## Walkthrough: Build & Run AIVisualProvision App

1. Right click on the solution and select **Rebuild Solution**.

   ![](Images/BuildSolution.png)

 1. On a whiteboard, design a simple web app architecture consisting of below resources:
    - App service to host the app
    - SQL database for the data
    - Key Vault to store certificates and sensitive data

1. Go to **Tools > Android > Android Device Manager**. Right click on your Android emulator and select **Edit**.

   ![](Images/Run_AllowCamera1.png)

1. Under Property **hw.camera.back** and **hw.camera.front**, select Value **webcam0**  so that computer/laptop camera can be accessed by the emulator. Click **Save** and close the window.

   ![](Images/Run_AllowCamera2.png)

1. In the **Android Device Manager**, click **Start**. Android Emulator will show up.

   ![](Images/Run_ClickStartEmulator.png)

   ![](Images/Run_StartEmulator.png)

1. Click **Run** to deploy your app to **Android Emulator** (launches in Debug mode). Wait for a while until AI Visual Provision app shows up on the emulator.

   ![](Images/Run_Emulator.png)

1. Optionally, if you are having trouble with emulator you can navigate to ***AIVisualProvision\Source\VisualProvision.Android\bin\Debug*** folder and install **com.microsoft.aiprovision-Signed.apk** on your Android mobile device and continue with below steps.

   ![](Images/CopySignedAPK.png)

1. Click **Allow** in order to access camera and photos.

   ![](Images/Run_AllowCameraAccess1.png)

   ![](Images/Run_AllowCameraAccess2.png)

   Note: If you get the error: "Guest isn't online after 7 seconds, retrying..", open the **Xamarin Android SDK Manager** in Visual Studio by going to Tools > Android > SDK Manager. Update the Android SDK Tools and Android Emulator to the latest Version.

1. Enter the password as **P2ssword** which was noted down earlier. Client ID and Tenant ID will be auto-populated. Click **Login**.

   ![](Images/Run_EnterPassword.png)

   ![](Images/Run_Login.png)

1. Select your **Azure Subscription** and click **Continue**.

   ![](Images/Run_SelectSubscription.png)

   ![](Images/Run_SelectSubscription2.png)

1. Take a picture of whiteboard which has your architecture diagram consisting of Azure Service logos or handwritten **App Service** and **SQL Database**. Whereas the **Key Vault** is mentioned in handwriting. For example, refer below architecture.

   ![](Images/Whiteboard.png)

1. You can see that Azure resources have been identified by the app. Click **Next**. 

   ![](Images/Run_IdentifyResources.png)

1. Select **Region** and **Resource group**. Click **Deploy**.

   ![](Images/Run_Deploy.png)

1. You can see the progress of your deployment right from your emulator or mobile device.

   ![](Images/Run_DeployInProgress.png)

   ![](Images/Run_DeployInProgress2.png)

   ![](Images/Run_SuccessfulDeployment.png)

1. Switch to your **Azure portal** and navigate to the newly created resource group to see the Azure resources created through your emulator or mobile device.

   ![](Images/Run_PostDeploymentRG.png)

1. For some reason if you are unable to complete this lab, you can download the mobile apps from the App Center through the following links to try it out:

   - [AI Visual Provision iOS App](https://install.appcenter.ms/orgs/appcenterdemos/apps/aivisualprovisionios/distribution_groups/public)

   - [AI Visual Provision Android App](https://install.appcenter.ms/orgs/appcenterdemos/apps/aivisualprovisionandroid/distribution_groups/public)

## Summary

Today you have more power at your fingertips than entire generations that came before you. Powerful mobile applications can be built using the power of Azure, AI and .NET

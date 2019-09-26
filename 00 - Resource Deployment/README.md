# Resource Deployment

This folder contains a PowerShell script that can be used to provision the Azure resources required to build the Store In A Box prototype.  You may skip this folder if you prefer to provision your Azure resources via the Azure Portal (see [Create Custom Vision](https://portal.azure.com/?microsoft_azure_marketplace_ItemHideKey=microsoft_azure_cognitiveservices_customvision#create/Microsoft.CognitiveServicesCustomVision)).  The PowerShell script will provision the following resources to your Azure subscription:

 
| Resource              | Usage                                                                                     |
|-----------------------|-------------------------------------------------------------------------------------------|
| Resource Group | The resource group containing the custom vision resources - named CustomVisionStoreInABox_rg.          |
| [Azure Custom Vision Training Service](https://azure.microsoft.com/en-us/services/cognitive-services/custom-vision-service/)	| Used to recognize products in the store.  This is the heart of the solution. It is named myCustomVisionService. 	|
|[Azure Custom Vision Prediction Service](https://azure.microsoft.com/en-us/services/cognitive-services/custom-vision-service) | Takes the model produced by the training service and makes it available through REST APIs to make predictions. It is named myCustomVisionServ_Prediction.                                                     |


By default, this PowerShell script will provision Free Tier Custom Vision services for your solution. See [Custom Vision Pricing](https://azure.microsoft.com/en-us/pricing/details/cognitive-services/custom-vision-service/) for information on features, scaling limits and pricing and choose your desired tier. 

## Prerequisites
1. Access to an Azure Subscription

## Deploy via Azure Portal
As an alternative to running the PowerShell script, you can deploy the resources manually via the Azure Portal or click the button below to deploy the resources:

<a href="https://azuredeploy.net/?repository=https://github.com/microsoft/solution-accelerator-containerized-store/" target="_blank">
    <img src="http://azuredeploy.net/deploybutton.png"/>
</a> 

## Steps for Resource Deployment via PowerShell

To run the [PowerShell script](./deploy.ps1):

1. Modify the parameters at the top of **deploy.ps1** if you want to configure the names of your resources and other settings.   
2. Run the [PowerShell script](./deploy.ps1). If you have PowerShell opened to this folder run the command:
`./deploy.ps1`
3. You will then be prompted to login and provide additional information.

When you have finished deploying, the script will provide you with the 'training API Key', and the 'prediction API Key'. Please copy these for future use. If you failed to retrieve them, don't worry, you can also get them from the Azure Portal.

### *Notes*

You will be prompted to choose a location for the resources.  Custom Vision recommended US locations as of the time of this writing were East US, East US 2, SouthCentral US, West US 2, and North Central US. If you need to install to a different location you will need to modify  the allowedValues in the location setting in azuredeploy.json.

Please see [Azure Service Limits](https://docs.microsoft.com/en-us/azure/search/search-limits-quotas-capacity) for additional information and best practices on sizing.

Credit / Resources: This is a modified version of the template published at https://github.com/vivienchevallier.  A blog article is also available from the author at Article-AzureCognitive.CustomVisionArmTemplate

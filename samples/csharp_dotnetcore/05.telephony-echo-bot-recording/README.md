﻿# Telephony EchoBot with call recording

Bot Framework v4 telephony echo bot sample with call recording.

## To try this sample

### Create a Telephony bot which records all incoming calls

- To get started [Build a bot that answers phone calls](https://github.com/microsoft/botframework-telephony/blob/main/README.md#documentation-and-samples)
- Clone this repo:

    ```bash
    git clone https://github.com/microsoft/botframework-telephony.git
    ```
- Open samples\csharp_dotnetcore\05.telephony-call-recording-echo\telephony-echo-recording.sln in Visual Studio
- Build and publish telephony-echo-bot-recording to the telephony bot above
- This bot should now be able to record incoming calls.

### Download call recording files

ACS emits a RecordingFileStatusUpdated when a new recoding is available. This event is delivered via the Event Grid to the specified endpoint. For more information see [Azure Communication Services as an Event Grid source](https://docs.microsoft.com/en-us/azure/event-grid/event-schema-communication-services?tabs=event-grid-event-schema). 

#### Make sure Event Grid is registered for the subscription: 
- az provider show -n Microsoft.EventGrid
- az provider register --namespace Microsoft.EventGrid

#### Choose a storage location to download the call recording files
This sample uses blob storage to illustrate the usage. 

- Create a new Storage Container
- Create a container to store the recording files. See [Create a container](https://docs.microsoft.com/en-us/azure/storage/blobs/storage-quickstart-blobs-portal#create-a-container)
- Note the connection string to allow the Azure function to download recording files to this location.
- Note the name of the container above.

#### Setup an endpoint to receive RecordingFileStatusUpdated events 
Event grid can deliver events to a variety of endpoints. For a complete list see [Event handlers in Azure Event Grid](https://docs.microsoft.com/en-us/azure/event-grid/event-handlers). To learn about authenticating event delivery see [Authenticate event delivery to event handlers ](https://docs.microsoft.com/en-us/azure/event-grid/security-authentication)

This sample uses an Azure Function App to receive RecordingFileStatusUpdated events.

- Create a new Azure Function App
- Build and Publish telephony-recording-download-function as new function in the Function App above.
- Set the following Configuration --> Application settings:
        - AcsEndpoint - Set this to the endpoint of your ACS resource.
        - RecordingContainerName - Set this to the blob container name where the recording files will be saved.

- Set the following Configuration --> Connection strings:
        AcsAccessKey - Set this to the access key of your ACS resource.
        RecordingStorageConnectionString - Set this to the connection string of your recording storage account.

#### Subscribe the endpoint to receive RecordingFileStatusUpdated events 

The final step is to subscribe to the RecordingFileStatusUpdated event in the ACS resource
- Go to your ACS resource --> Events
- Create a new Event subscription (+ Event Subscription)
- Choose a name and Filter to Event type `Recording File Status Updated(Preview)`
- Set the endpoint Type as Azure Function and choose the function above.
- Save the new event subscription

The Azure function should now be able to receive notifications and save the recording to the blob container as they become available.

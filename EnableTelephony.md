# Telephony Channel
Telephony Channel in Microsoft Bot Framework allows you to bind a Azure Communication Services' PSTN phone number with Microsoft Bot Framework bot.

Telephony channel under the hood is built on Microsoft Speech Services to allow Speech to Text and Text to Speech capabilities crucial for enabling an audio interaction/conversation over phone lines.

Please follow these steps to enable a Telephony channel for your bot.

## Pre-requisites
* [Step 1: Create a new bot](CreateBot.md). You can skip this step if you already have a working bot.
* [Step 2: Get an Azure Communication Services Resource](https://docs.microsoft.com/azure/communication-services/quickstarts/create-communication-resource) and [acquire a phone number in it](https://docs.microsoft.com/azure/communication-services/quickstarts/telephony-sms/get-phone-number).
* [Step 3: Create a Cognitive Services Resource](CreateCogSvcsResource.md). Please note that Telephony channel is currently supported in WestUS2 and EastUS.

## Enable web sockets
You will need to make a small configuration change so that your bot can communicate with the Telephony channel using web sockets. Follow these steps to enable web sockets:

1. Navigate to the Azure portal > App Service hosting your bot > Configuration (in the left navigation pane) > open `General Settings` tab.
1. Locate the toggle for Web sockets and set it to `On`.
1. Click Save.
1. In addition to this, ensure your bot code have enabled communication over web socket, for example in `dotnet` it is `app.UseWebSockets();` in `Startup.cs`.

## Enable Telephony Channel in the bot

Once we have created a cognitive services resource, we are ready to use it and configure it using the information collected in previous sections.

Go to the [Azure portal](https://portal.azure.com) > Bot (Created in previous [step](CreateBot.md)) > Channels

![](images/create-a-bot/c015-click-on-channels.png)

Click on the Telephony channel:

![](images/create-a-bot/c016-click-on-telephony.png)

Configure the channel with following information:

* Azure Communication Services' PSTN number in [provisioning a new phone number for your bot in Azure Communication Services](https://docs.microsoft.com/azure/communication-services/quickstarts/telephony-sms/get-phone-number).
* Azure Communication Service Access Key and Endpoint - Acquired while [creating a Azure Communication Services Resource](https://docs.microsoft.com/azure/communication-services/quickstarts/create-communication-resource).
* Cognitive Service Subscription account created during [Cognitive Service account creation](CreateCogSvcsResource.md).

>Make sure to specify the Telephony number in the E.164 format shown below.(+11234567890)

![](images/create-a-bot/c017-fill-out-settings-click-save.png)

Click **Save**.


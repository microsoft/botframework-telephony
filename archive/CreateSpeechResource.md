# Create a Speech Services resource

Now that you have the bot, we need to give it ability to understand human voice (speech-to-text) and speak (text-to-speech). For that that, you will create a Speech resource in Azure.

**Please note that currently Telephony Channel is only supported in West US 2 and East US Azure regions.**

Go to the [Azure portal](https://portal.azure.com) and select **Create a resource** from the left navigation:

![](images/create-a-bot/c006-create-new-resource-again.png)

In the search bar, type "Speech" and press **Enter**:

![](images/create-a-bot/c007-enter-speech.png)

Click **Create**:

![](images/create-a-bot/c008-click-create-speech.png)

You'll be prompted to provide some information:
   * Give your resource a **Name** (for example, **TelephonyChannelSpeech**)
   * For **Subscription**, choose the appropriate subscription
   * For **Location**, choose the appropriate region.
   
Ideally, this should be same as Bot's Azure region for best latencies. 
        
   * For **Pricing tier**, select **F0** (Free Tier) to start with. Note that usage in Free tier is subjected to [Free tier Limits](https://azure.microsoft.com/en-us/pricing/details/cognitive-services/speech-services/)
   * For **Resource group**, select an existing resource group or create a new resource group.
After you've entered all required information, click **Create**. 

![](images/create-a-bot/c009-fill-out-speech-settings.png)

It may take a few minutes to create your resource. 

**Next step**:  [Enable Telephony as one of the channels in your bot](EnableTelephony.md)

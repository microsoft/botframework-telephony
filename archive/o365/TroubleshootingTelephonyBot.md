# Troubleshooting Telephony Bot

Having issues getting your bot to be chatty? Try the suggestions below.

## Call connects but bot never says anything

This indicates that the telephony service is picking up, but having trouble sending messages to your bot.

### Using SSML? Make sure to use the full name of the voice font

Currently [SSML](https://docs.microsoft.com/azure/cognitive-services/speech-service/speech-synthesis-markup) voice fonts need to be specified using their full name.

```csharp
protected override async Task OnMessageActivityAsync(ITurnContext<IMessageActivity> turnContext, CancellationToken cancellationToken)
{
    //Wait for the user to say something :)
    if(string.IsNullOrWhiteSpace(turnContext.Activity.Text)) {
        return;
    }

    //Echo what they say!
    var replyText = $"You said {turnContext.Activity.Text}";

    //Doesn't work
    //var ssmlText = $"<speak version='1.0' xmlns='https://www.w3.org/2001/10/synthesis' xml:lang='en-US'><voice name='Jessa24kRUS'>{replyText}</voice></speak>");

    //Works
    var ssmlText = $"<speak version='1.0' xmlns='https://www.w3.org/2001/10/synthesis' xml:lang='en-US'><voice name='Microsoft Server Speech Text to Speech Voice (en-US, Jessa24kRUS)'>{replyText}</voice></speak>");
    await turnContext.SendActivityAsync(MessageFactory.Text(replyText, ssmlText), cancellationToken);
}
```

Validate that your speech account is in a region that supports the voice you are attempting to use. [Regions](https://docs.microsoft.com/azure/cognitive-services/speech-service/regions#standard-and-neural-voices)

## Getting an error when binding a cognitive services key to a number. 

![](images/channelConfigurationError.png)

Cognitive service keys map to bots 1:1. This means that if a bot has a cognitive services key, no other bot can use the same key, and vice versa.
A phone number can only be associated with a single bot.

Violating either of the above conditions will result in an error on configuration of the channel.

# Processing speech inside the bot

With Microsoft Bot Framework, you can use over 80 different locales / voice fonts to interact with your customers:  [language and region support for the speech service](https://docs.microsoft.com/en-us/azure/cognitive-services/speech-service/language-support).

We strongly recommend using [neural voices](https://docs.microsoft.com/en-us/azure/cognitive-services/speech-service/language-support#neural-voices) in IVR for realistically sounding voices.

## Making your bot speak
To ensure your bot can speak, all you need is to populate the 'Speak' field in the response activities.

```csharp
protected override async Task OnConversationUpdateActivityAsync
  (ITurnContext<IConversationUpdateActivity> turnContext, CancellationToken cancellationToken)
{
  var displayText = $@"
  
      Welcome to the demo telephony bot. 
      For billing questions, press or say 1. 
      To sign up for the new service, press or say 2.
      Otherwise, just tell me something, and I will repeat it back.";
      
  var spokenEquivalent = displayText;

  // first paramter of MessageFactory.Text is what should be displayed in messaging
  //   channels like Skype, Facebook Messenger, Web Chat, etc
  // second paramter of MessageFactory.Text is the equivalent message that should be 
  //   read aloud with speech-only scenario, such as an IVR
  
  await turnContext.SendActivityAsync(MessageFactory.Text(displayText, spokenEquivalent), 
      cancellationToken);
}
```

## Advanced voice and speech control

For more advanced voice control, you can leverage the full power of [Speech Synthesis Markup Language (SSML)](https://docs.microsoft.com/en-us/azure/cognitive-services/speech-service/speech-synthesis-markup) to customize the speech output, including adding regional differences, specifying genders, speaking styles (cheerful, empathetic, etc).

```csharp
private string SimpleConvertToSSML(string text, string voiceId, string locale)
{
    string ssmlTemplate = @"
    <speak version='1.0' xmlns='http://www.w3.org/2001/10/synthesis' xml:lang='{2}'>
        <voice name='{1}'> {0} </voice>
    </speak>";

    return string.Format(ssmlTemplate, text, voiceId, locale);
}

protected override async Task OnConversationUpdateActivityAsync
  (ITurnContext<IConversationUpdateActivity> turnContext, CancellationToken cancellationToken)
{
  var displayText = $@"
  
      Welcome to demo telephony bot. 
      For billing questions, press or say 1. 
      To sign up for the new service, press or say 2.
      Otherwise, just tell me something, and I will repeat it back.";

  var spokenTextInSSMLFormat = SimpleConvertToSSML(intromessagetext, "en-US-GuyNeural", "en-us");

  await turnContext.SendActivityAsync(MessageFactory.Text(displayText, spokenTextInSSMLFormat), cancellationToken);
}
```

For customer service applications, you can use Customer Service style that is available for Jessa for the en-US market, if you are interested in a female voice.  The additional tag that you would add to the SSML construct is the <mstts:express-as> tag:

```xml
    <speak xmlns="http://www.w3.org/2001/10/synthesis" xmlns:mstts="http://www.w3.org/2001/mstts" xmlns:emo="http://www.w3.org/2009/10/emotionml" version="1.0" xml:lang="en-US">
      <voice name="Microsoft Server Speech Text to Speech Voice (en-US, JessaNeural)">
        <mstts:express-as type="CustomerService">
          Welcome to Contoso Corporation.  How can I help you today? 
        </mstts:express-as>
      </voice>
</speak>
```

You can test it using the [Audio Content Creation tool](http://speech.microsoft.com/audiocontentcreation).  Select JessaNeural voice and under "Voice style" select "CustomerService".

## Understanding the user input

You can use ```OnMessageActivityAsync``` method to process user voice input:

```csharp
protected override async Task OnMessageActivityAsync(ITurnContext<IMessageActivity> turnContext, CancellationToken cancellationToken)
{
    string requestMessage = turnContext.Activity.Text.Trim().ToLower();
    string responseMessage = null;

    if (string.Equals("1", requestMessage) ||
        string.Equals("one", requestMessage, System.StringComparison.OrdinalIgnoreCase))
    {
        var responseText = @"Hello and thank you for calling billing department.  
          If you are a current customer, press 1.  
          If you are a new customer, press 2.";
          
        responseMessage = SimpleConvertToSSML(responseText, "en-US", "en-US-JessaNeural");
    }
    else if (string.Equals("2", requestMessage) ||
        string.Equals("two", requestMessage, System.StringComparison.OrdinalIgnoreCase) ||
        string.Equals("to", requestMessage, System.StringComparison.OrdinalIgnoreCase))
    {
        var responseText = @"Thank you for calling new customer information line.  
            If you want to sign up as the customer, press 1. 
            For general questions, press 2.";
            
        responseMessage = SimpleConvertToSSML(responseText, "en-US", "en-US-GuyNeural");
    }
    else if (string.Equals("3", requestMessage) ||
        string.Equals("three", requestMessage, System.StringComparison.OrdinalIgnoreCase))
    {
        var responseText = @"Diese Telefonleitung beantwortet alle allgemeinen Fragen. 
          Sagen Sie mir bitte in Ihren eigenen Worten, worüber Sie anrufen.";
          
        responseMessage = SimpleConvertToSSML(responseText, "de-DE", "de-DE-KatjaNeural");
    }
    else
    {
        responseMessage = SimpleConvertToSSML("What I heard was \"" + requestMessage + "\"", "en-US", "en-US-GuyNeural");
    }

    if (!string.IsNullOrWhiteSpace(responseMessage))
    {
        await turnContext.SendActivityAsync(
            GetActivity(responseMessage, responseMessage),
            cancellationToken);
    }
}
```

## Playing pre-recorded audio to the customer.

Bot can also play pre-recorded audio to the customer using audio element in ssml. The audio element in SSML supports the insertion of recorded audio files and the insertion of other audio formats in conjunction with synthesized speech output. Here is the list of <a href = "https://docs.microsoft.com/en-us/azure/cognitive-services/speech-service/rest-text-to-speech#audio-outputs">supported audio formats</a>. If the audio element is not empty then the contents should be the marked-up text to be spoken. The content will be played in specified voice if the audio document is not available or media type of audio is unsupported. 

Note: This is a preview feature.

Here is example ssml:

```xml
    <speak version=\"1.0\" xml:lang=\"en-US\">
      <voice name ='Microsoft Server Speech Text to Speech Voice (en-US, JessaRUS)'>
        <audio src='http://commondatastorage.googleapis.com/codeskulptor-assets/jump.ogg'>this is fallback text</audio>
      </voice>
    </speak>
```
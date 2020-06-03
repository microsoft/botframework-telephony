# Processing speech inside the bot

With Azure Speech Services, you get access to best-in-class Speech-to-Text and Text-to-Speech capabilities. [Sign up using this document here](https://docs.microsoft.com/en-us/azure/cognitive-services/speech-service/get-started). 

Azure Speech Services includes ability to use a neural voice, i.e. synthesized speech that is nearly indistinguishable from the human recordings. Neural voices can be used to make interactions with chatbots and voice assistants more natural and engaging. [Learn more about Text-to-Speech and neural voices here](https://docs.microsoft.com/en-us/azure/cognitive-services/speech-service/language-support#text-to-speech).

## 2. Making your bot speak
Populate the Speak field in the response activities to send synthesized audio output back to the user

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

## 3. Advanced voice and speech control

  ### 3.1. SSML:

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

  ### 3.2. Playing pre-recorded audio to the customer:

Bot can also play pre-recorded audio to the customer using audio element in ssml. The audio element in SSML supports the insertion of recorded audio files and the insertion of other audio formats in conjunction with synthesized speech output. Here is the list of <a href = "https://docs.microsoft.com/en-us/azure/cognitive-services/speech-service/rest-text-to-speech#audio-outputs">supported audio formats</a>. If the audio element is not empty then the contents should be the marked-up text to be spoken. The content will be played in specified voice if the audio document is not available or media type of audio is unsupported. 

Note: This is a preview feature.

Here is example ssml:

```xml
    <speak version="1.0" xml:lang="en-US">
      <voice name ="Microsoft Server Speech Text to Speech Voice (en-US, JessaRUS)">
        <audio src="https://raw.githubusercontent.com/microsoft/botframework-ivr/master/media/whatstheweatherlike.mp3">this is fallback text</audio>
      </voice>
    </speak>
```

**Next step**:  [Transfer call to an agent](TransferCallOut.md)
# Call Recording

__Disclaimer__: Call recording is temporarily available through the Telephony Channel public preview at no charge. Be aware that Azure billing for Call Recording will begin in April, at a rate of $0.01/minute of recorded content. Services in public preview are subject to future pricing changes.

> Many countries and states have laws and regulations that apply to the recording of PSTN, voice, and video calls, which often require that users consent to the recording of their communications. It is your responsibility to use the call recording capabilities in compliance with the law. You must obtain consent from the parties of recorded communications in a manner that complies with the laws applicable to each participant.

## Start recording, stop recording, and attach metadata to the recording
Telephony extensions package offers _StartRecording_, _StopRecording_, and _ResumeRecording_ methods:

- _StartRecording_ starts recording of the conversation. 
- _StopRecording_ stops recording of the conversation.
- _ResumeRecording_ resumes recording of the conversation, appending the new section of the recording to the previously started recording for this conversation.

If _StopRecording_ is never called, the recording is stopped when the bot ends the conversation.

If a recording is started for a conversation, another recording for the same conversation cannot be started. In such case, Telephony channel returns an error indicating that the "Recording is already in progress".

If _StopRecording_ is called and there is no recording in progress, Telephony channel returns an error indicating that the "Recording has not started".

If a recording for a single conversation is paused and resumed again, the recordings are appended in storage.

If a recording for a single conversation is stopped and started again, the recordings appear as multiple recording sessions in the storage.

The following is an example of starting call recording at the beginning of the conversation:

```csharp
public class TelephonyExtensions
{
    public static readonly string RecordingStart = "recording_start_command";

    public static Activity CreateRecordingStartCommand()
    {
        var startRecordingActivity = new Activity(ActivityTypesWithCommand.Command)
        {
            Name = RecordingStart;
            Value = new CommandValue<RecordingStartSettings>()
        };

        return startRecordingActivity;
    }
}
```

```csharp
protected override async Task OnMembersAddedAsync(IList<ChannelAccount> membersAdded, ITurnContext<IConversationUpdateActivity> turnContext, CancellationToken cancellationToken)
{
    foreach (var member in membersAdded)
    {
        // Greet anyone that was not the target (recipient) of this message.
        if (member.Id != turnContext.Activity.Recipient.Id)
        {
            // Start recording when the call begins
            await turnContext.SendActivityAsync(TelephonyExtensions.CreateRecordingStartCommand(), cancellationToken);

            var response = VoiceFactory.TextAndVoice($"Welcome to {CompanyName}! This call may be recorded for quality assurance purposes.");
            await turnContext.SendActivityAsync(response, cancellationToken);
        }
    }
}
```

To ensure call recording has started successfully, bots should await the "recording start" command result. This is done by overriding the `OnRecordingStartResultAsync` method of bot:

```csharp
protected override async Task OnRecordingStartResultAsync(ITurnContext<ICommandResultActivity> turnContext, CancellationToken cancellationToken)
{
    var result = CommandExtensions.GetCommandResultValue<object>(turnContext.Activity);

    // Check if recording started successfully
    if (result.Error == null)
    {
        // Call recording has started successfully, bot can proceed
        // ...
    }
    else
    {
        var recordingFailed = VoiceFactory.TextAndVoice($"Recording has failed, but your call will continue.");
        await turnContext.SendActivityAsync(recordingStatusText, cancellationToken);
    }
}
```

## Retrieve call recordings
Azure Communication Services provides short term media storage for recordings, please export any recorded content, you wish to preserve, within 48 hours. After 48 hours, recordings will no longer be available.

An Event Grid notification `Call Recording File Status Updated` is published when a recording is ready for retrieval, typically 1-2 minutes after the recording process has completed (i.e. meeting ended, recording stopped). Recording event notifications include a document ID, which can be used to retrieve both recorded media and a recording meta-data file:
- <Azure_Communication_Service_Endpoint>/recording/download/{documentId}
- <Azure_Communication_Service_Endpoint>/recording/download/{documentId}/metadata

Sample code for handling event grid notifications and downloading recording and meta-data files can be found [here](https://github.com/microsoft/botframework-telephony/tree/main/samples/csharp_dotnetcore/05a.telephony-recording-download-function).

Regulations such as GDPR require the ability to export user data. In order to enable to support these requirements, recording meta data files include the participantId for each call participant in the participants[] array. You can cross-reference the MRIs in the participants[] array with your internal user identities to identify participants in a call. An example of an recording meta-data file is provided below for reference:

```json
{
  "resourceId": "29bd5ea9-e0fc-4d00-8f6f-339b1f0ee2e2",
  "callId": "43563209-4ec6-4119-be5b-88398cda2caf",
  "chunkDocumentId": "0-eus-d11-05bf22b166dfb2119e3ff85d98906121",
  "chunkIndex": 0,
  "chunkStartTime": "2021-03-24T18:52:14.1321393Z",
  "chunkDuration": 15000.0,
  "pauseResumeIntervals": [],
  "recordingInfo": {
    "contentType": "mixed",
    "channelType": "audioVideo",
    "format": "mp4",
    "audioConfiguration": {
        "sampleRate": "16000",
        "bitRate": 128000,
        "channels": 1
        },
     "videoConfiguration": {
        "longerSideLength": 1920,
        "shorterSideLength": 1080,
        "framerate": 8,
        "bitRate": 1000000
        }
    },
  "participants": [
    {
    "participantId": "8:acs:29bd5ea9-e0fc-4d00-8f6f-339b1f0ee2e2_00000008-6ad3-6540-99c6-593a0d00c367"
    },
    {
    "participantId": "8:acs:29bd5ea9-e0fc-4d00-8f6f-339b1f0ee2e2_31d5c120-90a9-409d-8f48-a3463c35f58e"
    }
  ]
}
```

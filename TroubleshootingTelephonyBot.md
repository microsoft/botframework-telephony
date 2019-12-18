# Troubleshooting Telephony Bot

Having issues getting your bot to be chatty? Try the suggestions below.

## Call connects but bot never says anything

This indicates that the telephony service is picking up, but having trouble sending messages to your bot.

### Currently auth needs to be disabled for telephony to work

Try disabling auth by registering an alternative credential provider.

```csharp
/// <summary>
/// This disables authentication for all incoming requests.
/// Do not use for production traffic!
/// </summary>
public class DisabledAuthCredentialProvider : ICredentialProvider
{
    /// <summary>
    /// This gets the application password
    /// </summary>
    /// <param name="appId">The app id we need the password for</param>
    /// <returns>The password</returns>
    public Task<string> GetAppPasswordAsync(string appId)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Checks to see if authentication is disabled or not (returns true to indicate disabled)
    /// </summary>
    /// <returns>true</returns>
    public Task<bool> IsAuthenticationDisabledAsync()
    {
        return Task.FromResult(true);
    }

    /// <summary>
    /// Checks to see if the app id is valid
    /// </summary>
    /// <param name="appId">The appid we need to check</param>
    /// <returns>True if the app id is valid false otherwise</returns>
    public Task<bool> IsValidAppIdAsync(string appId)
    {
        throw new NotImplementedException();
    }
}
```
Next, register this ICredential provider in the "ConfigureServices" section of the Startup.cs

```csharp
//In startup.cs
public void ConfigureServices(IServiceCollection services)
{
    services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

    services.AddSingleton<IBotFrameworkHttpAdapter, BotFrameworkHttpAdapter>(); //Make sure this is a BotFrameworkHtttpAdapter that accepts ICredentialProvider in the startup
    services.AddSingleton<ICredentialProvider, DisabledAuthCredentialProvider>(); //Add this line to register the class we previously created

    // Create the bot as a transient. In this case the ASP Controller is expecting an IBot.
    services.AddTransient<IBot, EchoBot>();
}
```

### Using SSML? Make sure it follows these very specific formatting rules

Ensure every tag or text element in the [SSML](https://docs.microsoft.com/en-us/azure/cognitive-services/speech-service/speech-synthesis-markup) is seperated by a whitespace character

```csharp
private string SimpleConvertToSSML(string text, string voiceId, string locale)
{
    //Doesn't work
    //string ssmlTemplate = @"<speak version='1.0' xmlns='http://www.w3.org/2001/10/synthesis' xml:lang='{2}'><voice name='{1}'>{0}</voice></speak>";

    //Works
    string ssmlTemplate = @"<speak version='1.0' xmlns='http://www.w3.org/2001/10/synthesis' xml:lang='{2}'> <voice name='{1}'> {0} </voice> </speak>";

    return string.Format(ssmlTemplate, text, voiceId, locale);
}
```

Validate that your speech account is in a region that supports the voice you are attempting to use. [Regions](https://docs.microsoft.com/en-us/azure/cognitive-services/speech-service/regions#standard-and-neural-voices)

## Getting an error when binding a cognitive services key to a number. 

![](images/channelConfigurationError.png)

Cognitive service keys map to bots 1:1. This means that if a bot has a cognitive services key, no other bot can use the same key, and vice versa.
A phone number can only be associated with a single bot.

Violating either of the above conditions will result in an error on configuration of the channel.

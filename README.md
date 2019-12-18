# Building an IVR with Microsoft Bot Framework 


```
NOTE: Ability to create intelligent IVR's with Microsoft Bot Framework is a 
private preview project available to the select partners only.
```

Telphony channel in Microsoft Bot Framework is the Microsoft technology for enabling PSTN calling capabilites in a Bot. Telephony channel allows you to build an IVR by leveraging Office 365 phon numbers, along with the full power of Microsoft Azure Bot Framework and Microsoft Speech Services.

 ![](images/telephonychannel.png)

Please note:  This is a Beta (preview) version of software, and as with any preview software, there may be initial risks and limitations you run into, such as a need to integrate with your existing IVR, etc.  We are actively working on and supporting this product and are here to help you in case you run into any issues.  Reach us at ms-ivr-preview@microsoft.com.

## Requirements
* **IVR Private Preview Approval** - To get started, your Tenant/Organization needs to be approved for a Private Preview of the Microsoft Intelligent Call Center / IVR project.  Send the e-mail to ms-ivr-preview [AT] microsoft.com with following information
  * Tenant/Organization name
  * Azure account e-mail that should be whitelisted,
  * Description of the bot. 
  
  Once approved for privare preview, your customer will see Telephony channel in their bot settings. 
* **Office 365 License** - A minimum of Office 365 E3 + calling plan or an E5 plan is required 
* **Azure Subscription** - You will need a valid Azure subscription. You can [signup](https://signup.azure.com/) here for a new Azure subscription if needed

# Enabling IVR 

After getting approved into the private preview, overall setup should take roughly an hour to enable a basic IVR bot callable using a phone number.

The following are the high-level steps needed you to enable IVR support in your bot:

* [Optional: create a new Office 365 account trial](CreateOfficeTrial.md)
* [Step 1: Provision a new phone number for your bot in Office 365](AcquirePhoneNumber.md)
* [Step 2: Create a new Azure Web App Bot](CreateBot.md)
* [Step 3: Enable your bot to speak and understand voice](CreateSpeechResource.md)
* [Step 4: Enable Telephony Channel](EnableTelephony.md)
* [Step 5: Process speech inside of the bot](ProcessSpeechInBotCode.md)
* [Troubleshooting](TroubleshootingTelelphonyBot.md)

Once setup, you should be able to simply dial the acquired phone number using any PSTN or mobile phone (subjected to cellular plan on the source phone).

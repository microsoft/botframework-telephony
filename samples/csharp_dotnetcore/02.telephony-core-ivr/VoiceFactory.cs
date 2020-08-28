// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Bot.Schema;
using Newtonsoft.Json.Linq;

namespace Microsoft.Bot.Builder
{
    /// <summary>
    /// Simple helper class to let us abstract away our choice of voice font from the rest of the code
    /// </summary>
    public class VoiceFactory
    {
        private readonly string VoiceName;
        private readonly string Locale;
        private readonly string ExpressAsType;
        public VoiceFactory(string voiceName, string locale = "en-US", string expressAsType = null)
        {
            VoiceName = voiceName;
            Locale = locale;
            ExpressAsType = expressAsType;
        }

        public Activity TextAndVoice(string text, string inputHint = null)
        {
            string ssml = $"<speak version='1.0' xmlns='https://www.w3.org/2001/10/synthesis' xmlns:mstts='https://www.w3.org/2001/mstts' xml:lang='{Locale}'><voice name='{VoiceName}'><mstts:express-as style='{ExpressAsType}'>{text}</mstts:express-as></voice></speak>";
            return MessageFactory.Text(text, ssml, inputHint);
        }
    }
}
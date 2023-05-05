﻿// ---------------------------------------------------------------------------------- 
// Copyright (c) The Standard Organization, a coalition of the Good-Hearted Engineers 
// ----------------------------------------------------------------------------------

using System;
using System.IO;
using System.Text;
using Standard.AI.OpenAI.Clients.OpenAIs;
using Standard.AI.OpenAI.Models.Configurations;

namespace Standard.AI.OpenAI.Tests.Integration.APIs.AIFiles
{
    public partial class AIFilesTests
    {
        private readonly IOpenAIClient openAIClient;

        public AIFilesTests()
        {
            var openAIConfigurations = new OpenAIConfigurations
            {
                ApiKey = "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",
                OrganizationId = "org-ZnnUBA47iF2DKorQ9GllX5Aw",
                ApiUrl = "https://api.openai.com/"
            };

            this.openAIClient = new OpenAIClient(openAIConfigurations);
        }

        private static MemoryStream CreateRandomStream()
        {
            string content = "{\"prompt\": \"<prompt text>\", \"completion\": \"<ideal generated text>\"}";
            var memoryStream = new MemoryStream(Encoding.UTF8.GetBytes(content));
            return memoryStream;
        }

    }
}

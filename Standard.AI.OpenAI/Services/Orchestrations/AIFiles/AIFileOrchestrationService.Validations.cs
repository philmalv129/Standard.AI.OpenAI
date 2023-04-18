﻿// ---------------------------------------------------------------------------------- 
// Copyright (c) The Standard Organization, a coalition of the Good-Hearted Engineers 
// ----------------------------------------------------------------------------------

using Standard.AI.OpenAI.Models.Services.Foundations.AIFiles;
using Standard.AI.OpenAI.Models.Services.Orchestrations.AIFiles.Exceptions;

namespace Standard.AI.OpenAI.Services.Orchestrations.AIFiles
{
    internal partial class AIFileOrchestrationService
    {
        private static void ValidateAIFile(AIFile aiFile)
        {
            ValidateAIFileNotNull(aiFile);

            Validate(
               (Rule: IsInvalid(aiFile.Request), Parameter: nameof(AIFileRequest)));
        }

        private static void ValidateAIFileNotNull(AIFile aiFile)
        {
            if (aiFile is null)
            {
                throw new NullAIFileOrchestrationException();
            }
        }

        private static void Validate(params (dynamic Rule, string Parameter)[] validations)
        {
            var invalidAIFileException = new InvalidAIFileOrchestrationException();

            foreach ((dynamic rule, string parameter) in validations)
            {
                if (rule.Condition)
                {
                    invalidAIFileException.UpsertDataList(
                        key: parameter,
                        value: rule.Message);
                }
            }

            invalidAIFileException.ThrowIfContainsErrors();
        }

        private static dynamic IsInvalid(object @object) => new
        {
            Condition = @object is null,
            Message = "Object is required"
        };
    }
}

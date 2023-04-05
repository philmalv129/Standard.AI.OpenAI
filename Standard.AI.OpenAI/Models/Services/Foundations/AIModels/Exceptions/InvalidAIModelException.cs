﻿using Xeptions;

namespace Standard.AI.OpenAI.Models.Services.Foundations.AIModels.Exceptions
{
    public class InvalidAIModelException : Xeption
    {
        public InvalidAIModelException()
            : base(message: "AI Model is invalid.")
        { }
    }
}

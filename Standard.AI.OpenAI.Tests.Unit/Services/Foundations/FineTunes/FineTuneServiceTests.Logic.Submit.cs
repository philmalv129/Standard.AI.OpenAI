﻿// ---------------------------------------------------------------------------------- 
// Copyright (c) The Standard Organization, a coalition of the Good-Hearted Engineers 
// ----------------------------------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Force.DeepCloner;
using Moq;
using Standard.AI.OpenAI.Models.Services.Foundations.ExternalFineTunes;
using Standard.AI.OpenAI.Models.Services.Foundations.FineTunes;
using Xunit;

namespace Standard.AI.OpenAI.Tests.Unit.Services.Foundations.FineTunes
{
    public partial class FineTuneServiceTests
    {
        [Fact]
        public async Task ShouldSubmitFineTuneAsync()
        {
            // given
            DateTimeOffset randomDate = GetRandomDate();
            DateTimeOffset inputDate = randomDate;
            int inputUnixTime = (int)inputDate.ToUnixTimeSeconds();

            dynamic randomFineTuneProperties = CreateRandomFineTuneProperties(inputDate, inputUnixTime);

            var inputFineTuneRequest = new FineTuneRequest
            {
                FileId = randomFineTuneProperties.FileId,
                ValidationFile = randomFineTuneProperties.ValidationFile,
                Model = randomFineTuneProperties.Model,
                NumberOfDatasetCycles = randomFineTuneProperties.NumberOfDatasetCycles,
                BatchSize = randomFineTuneProperties.BatchSize,
                LearningRateMultiplier = randomFineTuneProperties.LearningRateMultiplier,
                PromptLossWeight = randomFineTuneProperties.PromptLossWeight,
                ComputeClassificationMetrics = randomFineTuneProperties.ComputeClassificationMetrics,
                NumberOfClasses = randomFineTuneProperties.NumberOfClasses,
                ClassificationPositiveClass = randomFineTuneProperties.ClassificationPositiveClass,
                ClassificationBetas = randomFineTuneProperties.ClassificationBetas,
                Suffix = randomFineTuneProperties.Suffix
            };

            var externalFineTuneRequest = new ExternalFineTuneRequest
            {
                FileId = randomFineTuneProperties.FileId,
                ValidationFile = randomFineTuneProperties.ValidationFile,
                Model = randomFineTuneProperties.Model,
                NumberOfDatasetCycles = randomFineTuneProperties.NumberOfDatasetCycles,
                BatchSize = randomFineTuneProperties.BatchSize,
                LearningRateMultiplier = randomFineTuneProperties.LearningRateMultiplier,
                PromptLossWeight = randomFineTuneProperties.PromptLossWeight,
                ComputeClassificationMetrics = randomFineTuneProperties.ComputeClassificationMetrics,
                NumberOfClasses = randomFineTuneProperties.NumberOfClasses,
                ClassificationPositiveClass = randomFineTuneProperties.ClassificationPositiveClass,
                ClassificationBetas = randomFineTuneProperties.ClassificationBetas,
                Suffix = randomFineTuneProperties.Suffix
            };

            FineTune inputFineTune = new FineTune
            {
                Request = inputFineTuneRequest,
            };

            FineTune expectedFineTune = inputFineTune.DeepClone();

            dynamic[] trainingFileProperties = randomFineTuneProperties.TrainingFile;

            TrainingFile[] trainingFiles = trainingFileProperties.Select(trainingFileProperty => new TrainingFile
            {
                Id = trainingFileProperty.Id,
                Type = trainingFileProperty.Type,
                Purpose = trainingFileProperty.Purpose,
                Filename = trainingFileProperty.Filename,
                Bytes = trainingFileProperty.Bytes,
                CreatedDate = trainingFileProperty.CreatedDate,
                Status = trainingFileProperty.Status,
                StatusDetails = trainingFileProperty.StatusDetails
            }).ToArray();

            Event[] events = ((dynamic[])randomFineTuneProperties.Events).Select(eventProperties => new Event
            {
                CreatedDate = eventProperties.CreatedDate,
                Level = eventProperties.Level,
                Message = eventProperties.Message,
                Type = eventProperties.Type
            }).ToArray();

            expectedFineTune.Response = new FineTuneResponse
            {
                Id = randomFineTuneProperties.Id,
                Type = randomFineTuneProperties.Type,
                HyperParameters = new HyperParameter
                {
                    EpochsCount = randomFineTuneProperties.HyperParameters.EpochsCount,
                    BatchSize = randomFineTuneProperties.HyperParameters.BatchSize,
                    PromptLossWeight = randomFineTuneProperties.HyperParameters.PromptLossWeight,
                    LearningRateMultiplier = randomFineTuneProperties.HyperParameters.LearningRateMultiplier
                },
                OrganizationId = randomFineTuneProperties.OrganizationId,
                Model = randomFineTuneProperties.Model,
                TrainingFile = trainingFiles,
                ValidationFiles = randomFineTuneProperties.ValidationFiles,
                ResultFiles = randomFineTuneProperties.ResultFiles,
                CreatedDate = randomFineTuneProperties.Created,
                UpdatedDate = randomFineTuneProperties.Updated,
                Status = randomFineTuneProperties.Status,
                FineTunedModel = randomFineTuneProperties.FineTunedModel,
                Events = events
            };

            ExternalTrainingFile[] externalTrainingFiles = trainingFileProperties.Select(trainingFileProperty => new ExternalTrainingFile
            {
                Id = trainingFileProperty.Id,
                Object = trainingFileProperty.Type,
                Purpose = trainingFileProperty.Purpose,
                Filename = trainingFileProperty.Filename,
                Bytes = trainingFileProperty.Bytes,
                CreatedDate = trainingFileProperty.CreatedDate,
                Status = trainingFileProperty.Status,
                StatusDetails = trainingFileProperty.StatusDetails
            }).ToArray();

            ExternalEvent[] externalEvents = ((dynamic[])randomFineTuneProperties.Events).Select(eventProperties => new ExternalEvent
            {
                CreatedDate = eventProperties.CreatedDate,
                Level = eventProperties.Level,
                Message = eventProperties.Message,
                Object = eventProperties.Type
            }).ToArray();

            var externalHyperParameters = new ExternalHyperParameters
            {
                EpochsCount = randomFineTuneProperties.HyperParameters.EpochsCount,
                BatchSize = randomFineTuneProperties.HyperParameters.BatchSize,
                PromptLossWeight = randomFineTuneProperties.HyperParameters.PromptLossWeight,
                LearningRateMultiplier = randomFineTuneProperties.HyperParameters.LearningRateMultiplier
            };

            var externalFineTuneResponse = new ExternalFineTuneResponse
            {
                Object = randomFineTuneProperties.Type,
                Id = randomFineTuneProperties.Id,
                HyperParameters = externalHyperParameters,
                OrganizationId = randomFineTuneProperties.OrganizationId,
                Model = randomFineTuneProperties.Model,
                TrainingFile = externalTrainingFiles,
                ValidationFiles = randomFineTuneProperties.ValidationFiles,
                ResultFiles = randomFineTuneProperties.ResultFiles,
                CreatedDate = randomFineTuneProperties.Created,
                UpdatedDate = randomFineTuneProperties.Updated,
                Status = randomFineTuneProperties.Status,
                FineTunedModel = randomFineTuneProperties.FineTunedModel,
                Events = externalEvents
            };

            this.openAIBrokerMock.Setup(broker =>
                broker.PostFineTuneAsync(It.Is(
                    SameExternalFineTuneRequestAs(externalFineTuneRequest))))
                        .ReturnsAsync(value: externalFineTuneResponse);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.ConvertToDateTimeOffSet(inputUnixTime))
                    .Returns(inputDate);

            // when
            var actualFineTune = await this.fineTuneService.SubmitFineTuneAsync(inputFineTune);

            // then
            actualFineTune.Should().BeEquivalentTo(expectedFineTune);

            this.openAIBrokerMock.Verify(broker =>
                broker.PostFineTuneAsync(It.Is(
                    SameExternalFineTuneRequestAs(externalFineTuneRequest))),
                        Times.Once());

            this.dateTimeBrokerMock.Verify(broker =>
                broker.ConvertToDateTimeOffSet(inputUnixTime),
                    Times.Once());

            this.openAIBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}

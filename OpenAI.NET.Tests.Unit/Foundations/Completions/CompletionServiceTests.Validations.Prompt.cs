﻿// --------------------------------------------------------------- 
// Copyright (c) Coalition of the Good-Hearted Engineers 
// ---------------------------------------------------------------

using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using OpenAI.NET.Models.Completions;
using OpenAI.NET.Models.Completions.Exceptions;
using OpenAI.NET.Models.ExternalCompletions;
using Xunit;

namespace OpenAI.NET.Tests.Unit.Foundations.Completions
{
    public partial class CompletionServiceTests
    {
        [Fact]
        public async Task ShouldThrowValidationExceptionOnPromptIfCompletionIsNullAsync()
        {
            // given
            Completion nullCompletion = null;
            var nullCompletionException = new NullCompletionException();

            var exceptedCompletionValidationException =
                new CompletionValidationException(nullCompletionException);

            // when
            ValueTask<Completion> promptCompletionTask =
                this.completionService.PromptCompletionAsync(nullCompletion);

            CompletionValidationException actualCompletionValidationException =
                await Assert.ThrowsAsync<CompletionValidationException>(
                    promptCompletionTask.AsTask);

            // then
            actualCompletionValidationException.Should()
                .BeEquivalentTo(exceptedCompletionValidationException);

            this.openAiBrokerMock.Verify(broker =>
                broker.PostCompletionRequestAsync(
                    It.IsAny<ExternalCompletionRequest>()),
                        Times.Never);

            this.openAiBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnPromptIfCompletionIsInvalidAsync()
        {
            // given
            var invalidCompletion = new Completion();
            invalidCompletion.Request = null;

            var invalidCompletionException =
                new InvalidCompletionException();

            invalidCompletionException.AddData(
                key: nameof(Completion.Request),
                values: "Object is required");

            var expectedCompletionValidationException =
                new CompletionValidationException(
                    invalidCompletionException);

            // when
            ValueTask<Completion> promptCompletionTask =
                this.completionService.PromptCompletionAsync(invalidCompletion);

            CompletionValidationException actualCompletionValidationException =
                await Assert.ThrowsAsync<CompletionValidationException>(
                    promptCompletionTask.AsTask);

            // then
            actualCompletionValidationException.Should()
                .BeEquivalentTo(expectedCompletionValidationException);

            this.openAiBrokerMock.Verify(broker =>
                broker.PostCompletionRequestAsync(
                    It.IsAny<ExternalCompletionRequest>()),
                        Times.Never);

            this.openAiBrokerMock.VerifyNoOtherCalls();
        }
    }
}

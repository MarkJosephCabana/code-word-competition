using AutoFixture;
using CodeWord.Application.Common.Repositories;
using CodeWord.Application.Queries;
using CodeWord.Core.Domain.Entities;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NUnit.Framework;
using System;

namespace CodeWord.Application.Tests.UnitTests
{
    public class GetCompetitionTests : HandlerTestBase<GetCompetition.Handler, GetCompetition.Query, GetCompetition.GetCompetitionResponse>
    {
        public override void SetupHandler()
        {

        }

        public override IServiceCollection SetupServices(IServiceCollection services)
        {
            services.AddScoped<ILogger<GetCompetition.Handler>>(_ => Mocker.GetMock<ILogger<GetCompetition.Handler>>().Object);
            services.AddScoped<ICompetitionRepository>(_ => Mocker.GetMock<ICompetitionRepository>().Object);

            return services;
        }

        [Test]
        public async Task handler_invalid_guid_validation_error()
        {
            //Arrange
            var competitionGUID = Guid.Empty;
            //Act
            AsyncTestDelegate td = () => Mediator.Send(new GetCompetition.Query(competitionGUID));
            //Assert
            var assertion = Assert.ThrowsAsync<FluentValidation.ValidationException>(td);
            assertion.Errors.Select(e => e.PropertyName).Should().Contain("CompetitionGUID");
        }

        [Test]
        public async Task handler_missing_guid_validation_error()
        {
            //Arrange
            Mocker.GetMock<ICompetitionRepository>()
                .Setup(x => x.CompetitionExists(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(false);

            //Act
            AsyncTestDelegate td = () => Mediator.Send(new GetCompetition.Query(Guid.NewGuid()));
            //Assert
            var assertion = Assert.ThrowsAsync<FluentValidation.ValidationException>(td);
            assertion.Errors.Select(e => e.PropertyName).Should().Contain("CompetitionGUID");
        }
    }
}

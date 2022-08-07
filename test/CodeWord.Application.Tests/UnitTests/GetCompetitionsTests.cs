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
    public class GetCompetitionsTests : HandlerTestBase<GetCompetitions.Handler, GetCompetitions.Query, GetCompetitions.GetCompetitionsResponse>
    {
        public override void SetupHandler()
        {

        }

        public override IServiceCollection SetupServices(IServiceCollection services)
        {
            services.AddScoped<ILogger<GetCompetitions.Handler>>(_ => Mocker.GetMock<ILogger<GetCompetitions.Handler>>().Object);
            services.AddScoped<ICompetitionRepository>(_ => Mocker.GetMock<ICompetitionRepository>().Object);

            return services;
        }

        [TestCase(-1)]
        public async Task handler_pageIndex_validation_error(int pageIndex)
        {
            //Arrange
            //Act
            AsyncTestDelegate td = () => Mediator.Send(new GetCompetitions.Query(pageIndex, Randomizer.Next(1, 100)));
            //Assert
            var assertion = Assert.ThrowsAsync<FluentValidation.ValidationException>(td);
            assertion.Errors.Select(e => e.PropertyName).Should().Contain("pageIndex");
        }

        [TestCase(-1)]
        [TestCase(0)]
        public async Task handler_pageSize_validation_error(int pageSize)
        {
            //Arrange
            //Act
            AsyncTestDelegate td = () => Mediator.Send(new GetCompetitions.Query(Randomizer.Next(1, 100), pageSize));
            //Assert
            var assertion = Assert.ThrowsAsync<FluentValidation.ValidationException>(td);
            assertion.Errors.Select(e => e.PropertyName).Should().Contain("pageSize");
        }

        [Test]
        public async Task handler_succeed()
        {
            //Arrange
            var pageIndex = Randomizer.Next(1, 100);
            var pageSize = Randomizer.Next(1, 100);
            IEnumerable<Competition> competitions = _GetTestData();

            Mocker.GetMock<ICompetitionRepository>()
                .Setup(x => x.GetCompetitions(pageIndex, pageSize, It.IsAny<CancellationToken>()))
                .ReturnsAsync(competitions);

            //Act
            var response = await Mediator.Send(new GetCompetitions.Query(pageIndex, pageSize));

            //Assert
            response.Competitions.Count().Should().Be(competitions.Count());
        }

        #region Helpers

        private IEnumerable<Competition> _GetTestData()
        {
            var competitions = new List<Competition>();

            for(var i = 0; i < 10; i++)
            {
                var entry = new Competition(Guid.NewGuid());
                entry.StartCompetition(DateTime.Now, DateTime.Now.AddDays(3), Fixture.CreateMany<string>(3));
                competitions.Add(entry);
            }

            return competitions;
        }

        #endregion
    }
}

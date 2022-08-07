using AutoFixture;
using CodeWord.Application.Commands;
using CodeWord.Application.Common.Repositories;
using CodeWord.Application.Queries;
using CodeWord.Core.Domain.Entities;
using CodeWord.Shared.SeedWork;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NUnit.Framework;
using System;
using System.Net.Mail;

namespace CodeWord.Application.Tests.UnitTests
{
    public class SubmitCompetitionEntryTests : HandlerTestBase<SubmitCompetitionEntry.Handler, SubmitCompetitionEntry.Command, SubmitCompetitionEntry.CompetitionEntryResponse>
    {
        Guid _competitionGUID;
        Guid _competitionRoundGUID;
        string _answer;
        string _firstName;
        string _lastName;
        string _email;
        string _addressLine1;
        string _addressLine2;
        string _suburb;
        string _state;
        string _postCode;
        string _phoneNumber;
        bool _optIn;
        AsyncTestDelegate _asyncTestDelegate;

        public override void Setup()
        {
            base.Setup();

            _competitionGUID = Guid.NewGuid();
            _competitionRoundGUID = Guid.NewGuid();
            _answer = Fixture.Create<string>().Substring(0, 25);
            _email = Fixture.Create<MailAddress>().Address;
            _firstName = _lastName = _addressLine1 = _addressLine2 = _suburb = _state = _postCode = _phoneNumber = Fixture.Create<string>();
            _asyncTestDelegate = () => Mediator.Send(new SubmitCompetitionEntry.Command(_competitionGUID, _competitionRoundGUID, _answer,
                _firstName, _lastName, _email, _addressLine1, _addressLine2, _suburb, _state, _postCode, _phoneNumber, _optIn));
        }

        public override void SetupHandler()
        {

        }


        public override IServiceCollection SetupServices(IServiceCollection services)
        {
            services.AddScoped<ILogger<SubmitCompetitionEntry.Handler>>(_ => Mocker.GetMock<ILogger<SubmitCompetitionEntry.Handler>>().Object);
            services.AddScoped<IUserRepository>(_ => Mocker.GetMock<IUserRepository>().Object);
            services.AddScoped<ICompetitionRepository>(_ => Mocker.GetMock<ICompetitionRepository>().Object);

            return services;
        }

        [Test]
        public async Task handler_invalid_competition_guid_validation_error()
        {
            //Arrange
            _competitionGUID = Guid.Empty;
            //Act
            //Assert
            var assertion = Assert.ThrowsAsync<FluentValidation.ValidationException>(_asyncTestDelegate);
            assertion.Errors.Select(e => e.PropertyName).Should().Contain("CompetitionGUID");
        }

        [Test]
        public async Task handler_invalid_competition__round_guid_validation_error()
        {
            //Arrange
            _competitionRoundGUID = Guid.Empty;
            //Act
            //Assert
            var assertion = Assert.ThrowsAsync<FluentValidation.ValidationException>(_asyncTestDelegate);
            assertion.Errors.Select(e => e.PropertyName).Should().Contain("CompetitionRoundGUID");
        }

        [TestCase(null)]
        [TestCase("")]
        [TestCase("12345678901234567890123456", Reason = "25 characters or less")]
        public async Task handler_invalid_answer_validation_error(string answer)
        {
            //Arrange
            _answer = answer;
            //Act
            //Assert
            var assertion = Assert.ThrowsAsync<FluentValidation.ValidationException>(_asyncTestDelegate);
            assertion.Errors.Select(e => e.PropertyName).Should().Contain("Answer");
        }

        [TestCase(null)]
        [TestCase("")]
        public async Task handler_invalid_firstname_validation_error(string firstName)
        {
            //Arrange
            _firstName = firstName;
            //Act
            //Assert
            var assertion = Assert.ThrowsAsync<FluentValidation.ValidationException>(_asyncTestDelegate);
            assertion.Errors.Select(e => e.PropertyName).Should().Contain("FirstName");
        }

        [TestCase(null)]
        [TestCase("")]
        public async Task handler_invalid_lastname_validation_error(string lastName)
        {
            //Arrange
            _lastName = lastName;
            //Act
            //Assert
            var assertion = Assert.ThrowsAsync<FluentValidation.ValidationException>(_asyncTestDelegate);
            assertion.Errors.Select(e => e.PropertyName).Should().Contain("LastName");
        }

        [TestCase(null)]
        [TestCase("")]
        public async Task handler_invalid_email_validation_error(string email)
        {
            //Arrange
            _email = email;
            //Act
            //Assert
            var assertion = Assert.ThrowsAsync<FluentValidation.ValidationException>(_asyncTestDelegate);
            assertion.Errors.Select(e => e.PropertyName).Should().Contain("Email");
        }

        [TestCase(null)]
        [TestCase("")]
        public async Task handler_invalid_addressLine1_validation_error(string addressLine1)
        {
            //Arrange
            _addressLine1 = addressLine1;
            //Act
            //Assert
            var assertion = Assert.ThrowsAsync<FluentValidation.ValidationException>(_asyncTestDelegate);
            assertion.Errors.Select(e => e.PropertyName).Should().Contain("AddressLine1");
        }

        [TestCase(null)]
        [TestCase("")]
        public async Task handler_invalid_suburb_validation_error(string suburb)
        {
            //Arrange
            _suburb = suburb;
            //Act
            //Assert
            var assertion = Assert.ThrowsAsync<FluentValidation.ValidationException>(_asyncTestDelegate);
            assertion.Errors.Select(e => e.PropertyName).Should().Contain("Suburb");
        }

        [TestCase(null)]
        [TestCase("")]
        public async Task handler_invalid_state_validation_error(string state)
        {
            //Arrange
            _state = state;
            //Act
            //Assert
            var assertion = Assert.ThrowsAsync<FluentValidation.ValidationException>(_asyncTestDelegate);
            assertion.Errors.Select(e => e.PropertyName).Should().Contain("State");
        }

        [TestCase(null)]
        [TestCase("")]
        public async Task handler_invalid_postcode_validation_error(string postCode)
        {
            //Arrange
            _postCode = postCode;
            //Act
            //Assert
            var assertion = Assert.ThrowsAsync<FluentValidation.ValidationException>(_asyncTestDelegate);
            assertion.Errors.Select(e => e.PropertyName).Should().Contain("PostCode");
        }

        [Test]
        public async Task handler_invalid_competition_round_validation_error()
        {
            //Arrange
            Mocker.GetMock<ICompetitionRepository>()
                .Setup(x => x.CompetitionRoundExists(It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(false));
            //Act
            //Assert
            var assertion = Assert.ThrowsAsync<FluentValidation.ValidationException>(_asyncTestDelegate);
            assertion.Errors.Select(e => e.PropertyName).Should().Contain("CompetitionRoundGUID");
        }

        [Test]
        public async Task handler_invalid_competition_round_answer_validation_error()
        {
            //Arrange
            Mocker.GetMock<ICompetitionRepository>()
                .Setup(x => x.CompetitionRoundExists(It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(false));
            //Act
            //Assert
            var assertion = Assert.ThrowsAsync<FluentValidation.ValidationException>(_asyncTestDelegate);
            assertion.Errors.Select(e => e.PropertyName).Should().Contain("CompetitionRoundGUID");
        }

        [Test]
        public async Task handler_succeed()
        {
            //Arrange
            var testData = _GetTestData();
            var userMock = new Mock<User>();
            userMock.SetupGet(p => p.Id).Returns(Randomizer.Next(1, 100));

            Mocker.GetMock<ICompetitionRepository>()
                .Setup(x => x.GetCompetition(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(() => testData);

            Mocker.GetMock<ICompetitionRepository>()
               .Setup(x => x.CompetitionRoundExists(It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
               .Returns(Task.FromResult(true));

            Mocker.GetMock<ICompetitionRepository>()
               .Setup(x => x.CompetitionRoundExists(It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
               .Returns(Task.FromResult(true));

            Mocker.GetMock<IUserRepository>()
                .Setup(x => x.Add(It.IsAny<User>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(() => userMock.Object);

            Mocker.GetMock<IUserRepository>()
                .SetupGet(x => x.UnitOfWork)
                .Returns(new UnitOfWorkMock());

            //Act
            var response = await Mediator.Send(new SubmitCompetitionEntry.Command(_competitionGUID, _competitionRoundGUID, _answer,
                _firstName, _lastName, _email, _addressLine1, _addressLine2, _suburb, _state, _postCode, _phoneNumber, _optIn));

            //Assert
            Mocker.GetMock<IUserRepository>()
                .Verify(x => x.Add(It.IsAny<User>(), It.IsAny<CancellationToken>()), Times.Once);

            Mocker.GetMock<IUserRepository>()
                .VerifyGet(x => x.UnitOfWork, Times.Once);

            response.Id.Should().BeGreaterThan(0);
        }

        #region Helpers
        private class UnitOfWorkMock : IUnitOfWork
        {
            public Task SaveChanges(CancellationToken cancellationToken = default)
            {
                return Task.CompletedTask;
            }
        }

        private Competition _GetTestData()
        {
            var competitionRound = new Mock<CompetitionRound>(_competitionRoundGUID, DateTime.Now, _answer);
            competitionRound.SetupGet(x => x.Id)
                .Returns(Randomizer.Next(1, 1000));

            var competition = new Mock<Competition>(_competitionGUID);
            competition.SetupGet(x => x.CompetitionRounds)
                .Returns(new List<CompetitionRound> { competitionRound.Object });

            return competition.Object;
        }

        #endregion
    }
}

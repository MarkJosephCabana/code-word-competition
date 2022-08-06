using CodeWord.Core.Domain.Entities;
using NUnit.Framework;
using System;
using FluentAssertions;
using AutoFixture;
using System.Net.Mail;

namespace CodeWord.Core.Tests.UnitTests
{
    public class CompetitionRoundTests
    {
        CompetitionRound _sut;
        Guid _competitionRoundGUID;
        DateTime _competitionRoundDate;
        string _codeWord;
        Fixture _fixture;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            _fixture = new Fixture();
        }

        [SetUp]
        public void Setup()
        {
            _competitionRoundGUID = Guid.NewGuid();
            _competitionRoundDate = DateTime.Now;
            _codeWord = _fixture.Create<string>();

            _sut = new CompetitionRound(_competitionRoundGUID, _competitionRoundDate, _codeWord);
        }

        [Test]
        public void initilize_should_succeed()
        {
            //Arrange
            //Act
            //Assert
            _sut.CompetitionRoundGUID.Should().Be(_competitionRoundGUID);
            _sut.Date.Day.Should().Be(_competitionRoundDate.Day);
            _sut.Date.Month.Should().Be(_competitionRoundDate.Month);
            _sut.Date.Year.Should().Be(_competitionRoundDate.Year);
            _sut.CodeWord.Should().Be(_codeWord);
        }

        [Test]
        public void initilize_guid_should_fail()
        {
            //Arrange
            _competitionRoundGUID = Guid.Empty;
            //Act
            TestDelegate sut = () => new CompetitionRound(_competitionRoundGUID, _competitionRoundDate, _codeWord);
            //Assert
            var assetion = Assert.Throws<ArgumentException>(sut);
            assetion.ParamName.Should().Be("competitionRoundGUID");
        }


        [Test]
        public void initilize_date_should_fail()
        {
            //Arrange
            _competitionRoundDate = default(DateTime);
            //Act
            TestDelegate sut = () => new CompetitionRound(_competitionRoundGUID, _competitionRoundDate, _codeWord);
            //Assert
            var assetion = Assert.Throws<ArgumentException>(sut);
            assetion.ParamName.Should().Be("date");
        }

        [TestCase("")]
        [TestCase(null)]
        public void initilize_codeword_should_fail(string codeWord)
        {
            //Arrange
            _codeWord = codeWord;
            //Act
            TestDelegate sut = () => new CompetitionRound(_competitionRoundGUID, _competitionRoundDate, _codeWord);
            //Assert
            var assetion = codeWord == null ? Assert.Throws<ArgumentNullException>(sut) : Assert.Throws<ArgumentException>(sut);

            assetion.ParamName.Should().Be("codeWord");
        }
    }
}
using AutoFixture;
using CodeWord.Core.Domain.Entities;
using FluentAssertions;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CodeWord.Core.Tests.UnitTests
{
    public class CompetitionTests
    {
        Competition _sut;
        Guid _competitionGUID;
        Fixture _fixture;
        Random _random;
        DateTime _startDate, _endDate;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            _fixture = new Fixture();
            _random = new Random();
        }

        [SetUp]
        public void Setup()
        {
            _competitionGUID = Guid.NewGuid();
            _sut = new Competition(_competitionGUID);
            _startDate = DateTime.Now;
            _endDate = DateTime.Now;
        }

        [Test]
        public void initilize_should_succeed()
        {
            //Arrange
            //Act
            //Assert
            _sut.CompetitionGUID.Should().Be(_competitionGUID);
        }

        [Test]
        public void initilize_should_fail()
        {
            //Arrange
            _competitionGUID = Guid.Empty;
            //Act
            TestDelegate sut = () => new Competition(_competitionGUID);
            //Assert
            var assetion = Assert.Throws<ArgumentException>(sut);
            assetion.ParamName.Should().Be("competitionGUID");
        }

        [Test]
        public void startround_should_fail_on_startdate_invalid()
        {
            //Arrange
            //Act
            TestDelegate sut = () => _sut.StartCompetition(default(DateTime), DateTime.Now, _fixture.CreateMany<string>(_random.Next(1, 10)));
            //Assert
            var assetion = Assert.Throws<ArgumentException>(sut);
            assetion.ParamName.Should().Be("startDate");
        }

        [Test]
        public void startround_should_fail_on_enddate_invalid()
        {
            //Arrange
            //Act
            TestDelegate sut = () => _sut.StartCompetition(DateTime.Now, default(DateTime), _fixture.CreateMany<string>(_random.Next(1, 10)));
            //Assert
            var assetion = Assert.Throws<ArgumentException>(sut);
            assetion.ParamName.Should().Be("endDate");
        }

        [Test]
        public void startround_should_fail_on_startdate_after_enddate()
        {
            //Arrange
            _endDate = DateTime.Now;
            _startDate = _endDate.AddDays(_random.Next(1, 100));
            //Act
            TestDelegate sut = () => _sut.StartCompetition(_startDate, _endDate, _fixture.CreateMany<string>(_random.Next(1, 10)));
            //Assert
            var assetion = Assert.Throws<ArgumentException>(sut);
            assetion.ParamName.Should().Be("startDate");
        }


        [Test]
        public void startround_should_fail_on_enddate_before_startdate()
        {
            //Arrange
            _startDate = DateTime.Now;
            _endDate = _startDate.AddDays(_random.Next(1, 100) * -1);
            //Act
            TestDelegate sut = () => _sut.StartCompetition(_startDate, _endDate, _fixture.CreateMany<string>(_random.Next(1, 10)));
            //Assert
            var assetion = Assert.Throws<ArgumentException>(sut);
            assetion.ParamName.Should().Be("startDate");
        }

        [TestCase(true)]
        [TestCase(false)]
        public void startround_should_fail_on_empty_codewords(bool isEmpty)
        {
            //Arrange
            IEnumerable<string> codeWords = isEmpty ? Enumerable.Empty<string>() :
                Enumerable.Range(0, _random.Next(1, 100)).Select(x => string.Empty);
            //Act
            TestDelegate sut = () => _sut.StartCompetition(_startDate, _endDate, codeWords);
            //Assert
            var assetion = Assert.Throws<ArgumentException>(sut);
            assetion.ParamName.Should().Be("codeWords");
        }

        [Test]
        public void startround_should_fail_on_null_codewords()
        {
            //Arrange
            //Act
            TestDelegate sut = () => _sut.StartCompetition(_startDate, _endDate, null);
            //Assert
            var assetion = Assert.Throws<ArgumentNullException>(sut);
            assetion.ParamName.Should().Be("codeWords");
        }

        [Test]
        public void startround_should_fail_on_dates_codewords_mismatch()
        {
            //Arrange
            IEnumerable<string> codeWords = _fixture.CreateMany<string>(5);
            _startDate = DateTime.Now;
            _endDate = _startDate.AddDays(2);

            //Act
            TestDelegate sut = () => _sut.StartCompetition(_startDate, _endDate, codeWords);
            //Assert
            var assetion = Assert.Throws<ArgumentException>(sut);
            assetion.ParamName.Should().Be("codeWords");
        }

        [Test]
        public void startround_should_succeed()
        {
            //Arrange
            IEnumerable<string> codeWords = _fixture.CreateMany<string>(5);
            _startDate = DateTime.Now;
            _endDate = _startDate.AddDays(5);

            //Act
            _sut.StartCompetition(_startDate, _endDate, codeWords);
            //Assert
            _sut.CompetitionDates.StartDate.Should().Be(_startDate);
            _sut.CompetitionDates.EndDate.Should().Be(_endDate);
            _sut.CompetitionRounds.Count.Should().Be(5);
        }
    }
}
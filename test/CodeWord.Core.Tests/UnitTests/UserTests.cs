using CodeWord.Core.Domain.Entities;
using NUnit.Framework;
using System;
using FluentAssertions;
using AutoFixture;
using System.Net.Mail;

namespace CodeWord.Core.Tests.UnitTests
{
    public class UserTests
    {
        User _sut;
        Guid _userGUID;
        Fixture _fixture;
        Random _random;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            _random = new Random();
            _fixture = new Fixture();
        }

        [SetUp]
        public void Setup()
        {
            _userGUID = Guid.NewGuid();
            _sut = new User(_userGUID, _random.Next(1,100));

        }

        [Test]
        public void initilize_should_succeed()
        {
            //Arrange
            //Act
            //Assert
            _sut.UserGUID.Should().Be(_userGUID);
        }

        [Test]
        public void initilize_guid_should_fail()
        {
            //Arrange
            _userGUID = Guid.Empty;
            //Act
            TestDelegate sut = () => new User(_userGUID, _random.Next(1, 100));
            //Assert
            var assetion = Assert.Throws<ArgumentException>(sut);
            assetion.ParamName.Should().Be("userGUID");
        }

        [TestCase(0)]
        [TestCase(-1)]
        public void initilize_competitionroundid_should_fail(int competitionRoundId)
        {
            //Arrange
            //Act
            TestDelegate sut = () => new User(_userGUID, competitionRoundId);
            //Assert
            var assetion = Assert.Throws<ArgumentException>(sut);
            assetion.ParamName.Should().Be("competitionRoundId");
        }

        [TestCase("")]
        [TestCase(null)]
        public void setpersonaldetails_fistname_should_fail(string firstName)
        {
            //Arrange
            //Act
            TestDelegate sut = () => _sut.SetPersonalDetails(firstName, _fixture.Create<string>());
            //Assert
            var assetion = firstName == null ? Assert.Throws<ArgumentNullException>(sut) : 
                Assert.Throws<ArgumentException>(sut);
            assetion.ParamName.Should().Be("firstName");
        }

        [TestCase("")]
        [TestCase(null)]
        public void setpersonaldetails_lastname_should_fail(string lastName)
        {
            //Arrange
            //Act
            TestDelegate sut = () => _sut.SetPersonalDetails(_fixture.Create<string>(), lastName);
            //Assert
            var assetion = lastName == null ? Assert.Throws<ArgumentNullException>(sut) :
                Assert.Throws<ArgumentException>(sut);
            assetion.ParamName.Should().Be("lastName");
        }

        [Test]
        public void setpersonaldetails_lastname_should_succeed()
        {
            //Arrange
            var firstName = _fixture.Create<string>();
            var lastName = _fixture.Create<string>();   
            //Act
            _sut.SetPersonalDetails(firstName, lastName);
            //Assert
            _sut.FirstName.Should().Be(firstName);
            _sut.LastName.Should().Be(lastName);
        }

        [TestCase("")]
        [TestCase(null)]
        [TestCase("qweqweqweqwwqeqw123")]
        public void setcontactdetails_email_should_fail(string email)
        {
            //Arrange

            //Act
            TestDelegate sut = () => _sut.SetContactDetails(email, _fixture.Create<string>());
            //Assert
            var assetion = email == null ? Assert.Throws<ArgumentNullException>(sut) :
                Assert.Throws<ArgumentException>(sut);
            assetion.ParamName.Should().Be("email");
        }

        [Test]
        public void setcontactdetails_email_should_succeed()
        {
            //Arrange
            var email = _fixture.Create<MailAddress>();
            var phoneNumber = _fixture.Create<string>();
            //Act
            _sut.SetContactDetails(email.Address, phoneNumber);
            //Assert
            _sut.Email.Should().Be(email.Address);
            _sut.PhoneNumber.Should().Be(phoneNumber);
        }

        [TestCase("")]
        [TestCase(null)]
        public void setaddress_addressLine1_should_fail(string addressLine1)
        {
            //Arrange

            //Act
            TestDelegate sut = () => _sut.SetAddress(addressLine1, _fixture.Create<string>(), _fixture.Create<string>(), 
                _fixture.Create<string>(), _fixture.Create<string>());

            //Assert
            var assetion = addressLine1 == null ? Assert.Throws<ArgumentNullException>(sut) :
                Assert.Throws<ArgumentException>(sut);
            assetion.ParamName.Should().Be("addressLine1");
        }

        [Test]
        public void setaddress_addressLine1_should_succeed()
        {
            //Arrange
            var addressLine1 = _fixture.Create<string>();
            //Act
            _sut.SetAddress(addressLine1, _fixture.Create<string>(), _fixture.Create<string>(),
                _fixture.Create<string>(), _fixture.Create<string>());
            //Assert
            _sut.HomeAddress.AddressLine1.Should().Be(addressLine1);
        }

        [TestCase(true)]
        [TestCase(false)]
        public void setaddress_addressLine2_should_succeed(bool isNull)
        {
            //Arrange
            var addressLine2 = isNull ? null : _fixture.Create<string>();
            //Act
            _sut.SetAddress(_fixture.Create<string>(), addressLine2, _fixture.Create<string>(),
                _fixture.Create<string>(), _fixture.Create<string>());

            //Assert
            _sut.HomeAddress.AddressLine2.Should().Be(addressLine2);
        }

        [TestCase("")]
        [TestCase(null)]
        public void setaddress_suburb_should_fail(string suburb)
        {
            //Arrange

            //Act
            TestDelegate sut = () => _sut.SetAddress(_fixture.Create<string>(), _fixture.Create<string>(), suburb,
                _fixture.Create<string>(), _fixture.Create<string>());

            //Assert
            var assetion = suburb == null ? Assert.Throws<ArgumentNullException>(sut) :
                Assert.Throws<ArgumentException>(sut);
            assetion.ParamName.Should().Be("suburb");
        }

        [Test]
        public void setaddress_suburb_should_succeed()
        {
            //Arrange
            var suburb = _fixture.Create<string>();
            //Act
            _sut.SetAddress(_fixture.Create<string>(), _fixture.Create<string>(), suburb,
                _fixture.Create<string>(), _fixture.Create<string>());
            //Assert
            _sut.HomeAddress.Suburb.Should().Be(suburb);
        }

        [TestCase("")]
        [TestCase(null)]
        public void setaddress_state_should_fail(string state)
        {
            //Arrange

            //Act
            TestDelegate sut = () => _sut.SetAddress(_fixture.Create<string>(), _fixture.Create<string>(), _fixture.Create<string>(),
                state, _fixture.Create<string>());

            //Assert
            var assetion = state == null ? Assert.Throws<ArgumentNullException>(sut) :
                Assert.Throws<ArgumentException>(sut);
            assetion.ParamName.Should().Be("state");
        }

        [Test]
        public void setaddress_state_should_succeed()
        {
            //Arrange
            var state = _fixture.Create<string>();
            //Act
            _sut.SetAddress(_fixture.Create<string>(), _fixture.Create<string>(), _fixture.Create<string>(),
                state, _fixture.Create<string>());
            //Assert
            _sut.HomeAddress.State.Should().Be(state);
        }

        [TestCase("")]
        [TestCase(null)]
        public void setaddress_postcode_should_fail(string postCode)
        {
            //Arrange

            //Act
            TestDelegate sut = () => _sut.SetAddress(_fixture.Create<string>(), _fixture.Create<string>(), _fixture.Create<string>(),
                _fixture.Create<string>(), postCode);

            //Assert
            var assetion = postCode == null ? Assert.Throws<ArgumentNullException>(sut) :
                Assert.Throws<ArgumentException>(sut);
            assetion.ParamName.Should().Be("postCode");
        }

        [Test]
        public void setaddress_postcode_should_succeed()
        {
            //Arrange
            var postCode = _fixture.Create<string>();
            //Act
            _sut.SetAddress(_fixture.Create<string>(), _fixture.Create<string>(), _fixture.Create<string>(),
                _fixture.Create<string>(), postCode);
            //Assert
            _sut.HomeAddress.PostCode.Should().Be(postCode);
        }

        [TestCase(true)]
        [TestCase(false)]
        public void toggle_optin_succeed(bool hasOptIn)
        {
            //Arrange
            //Act
            _sut.ToggleOptIn(hasOptIn);

            //Assert
            _sut.HasOptIn.Should().Be(hasOptIn);
        }
    }
}
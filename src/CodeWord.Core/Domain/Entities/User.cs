using System.Net.Mail;

namespace CodeWord.Core.Domain.Entities
{
    public class User : Entity, IAggregteRoot
    {
        protected User() { }

        public User(Guid userGUID)
        {
            Guard.Against.Default<Guid>(userGUID);

            this.UserGUID = userGUID;
        }

        public Guid UserGUID { get; private set; }
        public string FirstName { get; private set; }
        public string LastName { get; private set; }
        public string Email { get; private set; }
        public Address HomeAddress { get; private set; }
        public string PhoneNumber { get; private set; }
        public bool HasOptIn { get; private set; }

        public void SetPersonalDetails(string firstName, string lastName)
        {
            Guard.Against.NullOrWhiteSpace(firstName);
            Guard.Against.NullOrWhiteSpace(lastName);

            FirstName = firstName;
            LastName = lastName;
        }

        public void SetContactDetails(string email, string phoneNumber)
        {
            Guard.Against.NullOrWhiteSpace(email);
            Guard.Against.InvalidInput(email, nameof(email), e => MailAddress.TryCreate(e, out MailAddress mailAddress));

            Email = email;
            PhoneNumber = phoneNumber;
        }

        public void SetAddress(string addressLine1, string addressLine2, string suburb, string state, string postCode)
        {
            Guard.Against.NullOrWhiteSpace(addressLine1);
            Guard.Against.NullOrWhiteSpace(suburb);
            Guard.Against.NullOrWhiteSpace(state);
            Guard.Against.NullOrWhiteSpace(postCode);

            HomeAddress = new Address(addressLine1, addressLine2, suburb, state, postCode);
        }

        public void ToggleOptIn(bool hasOptIn)
        {
            HasOptIn = hasOptIn;
        }
    }
}

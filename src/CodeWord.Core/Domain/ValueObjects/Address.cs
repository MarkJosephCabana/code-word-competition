namespace CodeWord.Core.Domain.ValueObjects
{
    public record Address
    {
        protected Address() { }

        public Address(string addressLine1, string addressLine2, string suburb, string state, string postCode)
        {
            AddressLine1 = addressLine1;
            AddressLine2 = addressLine2;
            Suburb = suburb;
            State = state;
            PostCode = postCode;
        }
        public string AddressLine1 { get; private set; }
        public string AddressLine2 { get; private set; }
        public string Suburb { get; private set; }
        public string State { get; private set; }
        public string PostCode { get; private set; }
    }
}

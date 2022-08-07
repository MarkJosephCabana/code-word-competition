namespace CodeWord.API.Models
{
    public class CompetitionEntryDTO 
    {
        [System.Text.Json.Serialization.JsonIgnore]
        public Guid CompetitionGUID { get; set; }
        public Guid CompetitionRoundGUID { get; set; }
        public string Answer { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string Suburb { get; set; }
        public string State { get; set; }
        public string PostCode { get; set; }
        public string PhoneNumber { get; set; }
        public bool OptIn { get; set; }
    }
}

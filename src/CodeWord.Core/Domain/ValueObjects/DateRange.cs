namespace CodeWord.Core.Domain.ValueObjects
{
    public record DateRange
    {
        protected DateRange() { }

        public DateRange(DateTime startDate, DateTime endDate)
        {
            StartDate = startDate;
            EndDate = endDate;
        }
        public DateTime StartDate { get; private set; }
        public DateTime EndDate { get; private set; }
    }
}

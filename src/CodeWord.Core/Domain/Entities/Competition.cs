namespace CodeWord.Core.Domain.Entities
{
    public class Competition : Entity, IAggregteRoot
    {
        protected Competition() { }
        public Competition(Guid competitionGUID)
        {
            Guard.Against.Default<Guid>(competitionGUID);

            CompetitionGUID = competitionGUID;
            _competitionRounds = new List<CompetitionRound>();
            CompetitionDates = new DateRange(DateTime.Now, DateTime.Now);
        }

        public Guid CompetitionGUID { get; private set; }
        public DateRange CompetitionDates { get; private set; }

        private List<CompetitionRound> _competitionRounds;
        public IReadOnlyList<CompetitionRound> CompetitionRounds
            => _competitionRounds?.AsReadOnly();

        public void StartCompetition(DateTime startDate, DateTime endDate, IEnumerable<string> codeWords)
        {
            Guard.Against.Default<DateTime>(startDate);
            Guard.Against.Default<DateTime>(endDate);
            Guard.Against.Null(codeWords);
            Guard.Against.InvalidInput<DateTime>(startDate, nameof(startDate), d => endDate.Date >= d.Date, "Start should be earlier than end date");
            Guard.Against.InvalidInput<IEnumerable<string>>(codeWords, nameof(codeWords), cw => cw.Count() > 0 && cw.All(e => !string.IsNullOrWhiteSpace(e)), "Invalid code word entry");
           
            //One code word is assigned per day.
            //Ignore any excess codewords
            //but a code word must exist per day
            if((endDate - startDate).TotalDays > codeWords.Count())
                throw new ArgumentException("Code word count does not match the date range", nameof(codeWords));

            CompetitionDates = new DateRange(startDate, endDate);
                
            _competitionRounds.Clear();
            
            var codeWordArray = codeWords.ToArray();
            var index = 0;

            foreach(var date in Enumerable.Range(0, (int)(endDate.Date - startDate.Date).TotalDays).Select(d => startDate.AddDays(d)))
            {
                var competitionRound = new CompetitionRound(Guid.NewGuid(), date, codeWordArray[index]);
                _competitionRounds.Add(competitionRound);
                index++;
            }
        }
    }
}

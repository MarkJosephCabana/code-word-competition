namespace CodeWord.Core.Domain.Entities
{
    public class CompetitionRound : Entity
    {
        protected CompetitionRound() { }
        
        public CompetitionRound(Guid competitionRoundGUID, DateTime date, string codeWord)
        {
            Guard.Against.Default<Guid>(competitionRoundGUID);
            Guard.Against.Default<DateTime>(date);
            Guard.Against.NullOrWhiteSpace(codeWord);

            this.CompetitionRoundGUID = competitionRoundGUID;
            this.Date = date;
            this.CodeWord = codeWord;
        }

        public Guid CompetitionRoundGUID { get; private set; }
        public DateTime Date { get; private set; }
        public string CodeWord { get; private set; }
    }
}
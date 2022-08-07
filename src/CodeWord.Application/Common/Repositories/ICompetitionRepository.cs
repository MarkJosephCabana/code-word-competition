namespace CodeWord.Application.Common.Repositories
{
    public interface ICompetitionRepository : IRepository
    {
        Task<IEnumerable<Competition>> GetCompetitions(int index, int pageSize, CancellationToken cancellationToken = default);
        Task<Competition> GetCompetition(Guid competitionGUID, CancellationToken cancellationToken = default);
        Task<bool> CompetitionExists(Guid competitionGUID, CancellationToken cancellationToken = default);
        Task<bool> CompetitionRoundExists(Guid competitionGUID, Guid competitionRoundGUID, CancellationToken cancellationToken = default);
        Task<bool> CompetitionRoundExists(Guid competitionGUID, Guid competitionRoundGUID, string answer, CancellationToken cancellationToken = default);
    }
}

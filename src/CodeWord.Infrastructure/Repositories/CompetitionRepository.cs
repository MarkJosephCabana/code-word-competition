using CodeWord.Shared.SeedWork;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace CodeWord.Infrastructure.Repositories
{
    public class CompetitionRepository : ICompetitionRepository
    {
        private readonly ILogger<CompetitionRepository> _logger;
        private readonly CodeWordDBContext _context;

        public CompetitionRepository(ILogger<CompetitionRepository> logger, CodeWordDBContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IUnitOfWork UnitOfWork => _context;

        public async Task<bool> CompetitionExists(Guid competitionGUID, CancellationToken cancellationToken = default)
        {
            try
            {
                return await _context.Competitions
                    .AsNoTracking()
                    .AnyAsync(c => c.CompetitionGUID == competitionGUID, cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "{competitionGUID]", competitionGUID);
                throw;
            }
        }

        public async Task<bool> CompetitionRoundExists(Guid competitionGUID, Guid competitionRoundGUID, CancellationToken cancellationToken = default)
        {
            try
            {
                return await _context.Competitions
                    .AsNoTracking()
                    .AnyAsync(c => c.CompetitionGUID == competitionGUID && c.CompetitionRounds.Any(cr => cr.CompetitionRoundGUID == competitionRoundGUID), cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "{competitionGUID} {competitionRoundGUID}", competitionGUID, competitionRoundGUID);
                throw;
            }
        }

        public async Task<bool> CompetitionRoundExists(Guid competitionGUID, Guid competitionRoundGUID, string answer, CancellationToken cancellationToken = default)
        {
            try
            {
                return await _context.Competitions
                    .AsNoTracking()
                    .AnyAsync(c => c.CompetitionGUID == competitionGUID && c.CompetitionRounds.Any(cr => cr.CompetitionRoundGUID == competitionRoundGUID && cr.CodeWord == answer), cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "{competitionGUID} {competitionRoundGUID} {answer}", competitionGUID, competitionRoundGUID, answer);
                throw;
            }
        }

        public async Task<Competition> GetCompetition(Guid competitionGUID, CancellationToken cancellationToken = default)
        {
            try
            {
                return await _context.Competitions
                    .Include(c => c.CompetitionRounds)
                     .FirstOrDefaultAsync(c => c.CompetitionGUID == competitionGUID, cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "{competitionGUID}", competitionGUID);
                throw;
            }
        }

        public async Task<IEnumerable<Competition>> GetCompetitions(int index, int pageSize, CancellationToken cancellationToken = default)
        {
            try
            {
                return await _context.Competitions
                    .AsNoTracking()
                    .ToListAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "{index} {pageSize}", index, pageSize);
                throw;
            }
        }
    }
}

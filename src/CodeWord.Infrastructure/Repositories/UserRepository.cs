using CodeWord.Shared.SeedWork;
using Microsoft.Extensions.Logging;

namespace CodeWord.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly ILogger<UserRepository> _logger;
        private readonly CodeWordDBContext _context;

        public UserRepository(ILogger<UserRepository> logger, CodeWordDBContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IUnitOfWork UnitOfWork => _context;

        public async Task<User> Add(User entry, CancellationToken cancellationToken = default)
        {
            try
            {
                await _context.Users.AddAsync(entry, cancellationToken);
                return entry;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "{@user}", entry);
                throw;
            }
        }
    }
}

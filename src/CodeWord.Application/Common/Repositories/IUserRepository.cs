namespace CodeWord.Application.Common.Repositories
{
    public interface IUserRepository : IRepository
    {
        Task<User> Add(User entry, CancellationToken cancellationToken = default);
    }
}

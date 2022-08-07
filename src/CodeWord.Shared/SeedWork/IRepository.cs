namespace CodeWord.Shared.SeedWork
{
    public interface IRepository
    {
        IUnitOfWork UnitOfWork { get; }
    }

    public interface IUnitOfWork
    {
        Task SaveChanges(CancellationToken cancellationToken = default);
    }
}

using CodeWord.Infrastructure.EntityConfigurations;
using CodeWord.Shared.SeedWork;
using Microsoft.EntityFrameworkCore;

namespace CodeWord.Infrastructure
{
    public class CodeWordDBContext : DbContext, IUnitOfWork
    {
        public DbSet<Competition> Competitions { get; set; }
        public DbSet<User> Users { get; set; }

        public CodeWordDBContext(DbContextOptions<CodeWordDBContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfiguration(new CompetitionEntityConfiguration())
                .ApplyConfiguration(new CompetitionRoundEntityConfiguration())
                .ApplyConfiguration(new UserEntityConfiguration());
        }

        public async Task SaveChanges(CancellationToken cancellationToken = default)
        {
            await this.SaveChangesAsync(cancellationToken);
        }
    }
}

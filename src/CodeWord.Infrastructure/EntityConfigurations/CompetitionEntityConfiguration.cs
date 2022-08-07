using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CodeWord.Infrastructure.EntityConfigurations
{
    internal class CompetitionEntityConfiguration : IEntityTypeConfiguration<Competition>
    {
        public void Configure(EntityTypeBuilder<Competition> builder)
        {
            builder.ToTable("competitions", "cdwrd");
            
            builder.HasKey(t => t.Id);
            builder.Property(t => t.Id)
                .UseIdentityColumn();
            
            builder.Property(t => t.CompetitionGUID)
                .IsRequired();

            builder.HasIndex(t => t.CompetitionGUID)
                .IsUnique();


            builder.OwnsOne(t => t.CompetitionDates,
                sa => {
                    sa.Property(c => c.StartDate).IsRequired();
                    sa.Property(c => c.EndDate).IsRequired();
                });

            builder.HasMany(t => t.CompetitionRounds)
                .WithOne()
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}

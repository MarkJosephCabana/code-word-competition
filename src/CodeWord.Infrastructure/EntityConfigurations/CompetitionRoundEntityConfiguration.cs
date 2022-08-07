using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CodeWord.Infrastructure.EntityConfigurations
{
    internal class CompetitionRoundEntityConfiguration : IEntityTypeConfiguration<CompetitionRound>
    {
        public void Configure(EntityTypeBuilder<CompetitionRound> builder)
        {
            builder.ToTable("competition_rounds", "cdwrd");
            
            builder.HasKey(t => t.Id);
            builder.Property(t => t.Id)
                .UseIdentityColumn();
            
            builder.Property(t => t.CompetitionRoundGUID)
                .IsRequired();

            builder.HasIndex(t => t.CompetitionRoundGUID)
                .IsUnique();

            builder.Property(t => t.Date)
                .IsRequired();

            builder.Property(t => t.CodeWord)
                .IsRequired()
                .HasMaxLength(25);
        }
    }
}

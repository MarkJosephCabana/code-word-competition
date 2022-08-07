using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CodeWord.Infrastructure.EntityConfigurations
{
    internal class UserEntityConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("users", "cdwrd");
            
            builder.HasKey(t => t.Id);
            builder.Property(t => t.Id)
                .UseIdentityColumn();
            
            builder.Property(t => t.UserGUID)
                .IsRequired();

            builder.HasIndex(t => t.UserGUID)
                .IsUnique();

            builder.Property(t => t.FirstName)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(t => t.LastName)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(t => t.Email)
                .IsRequired()
                .HasMaxLength(200);

            builder.OwnsOne(t => t.HomeAddress,
                sa => {
                    
                    sa.Property(c => c.AddressLine1)
                        .IsRequired()
                        .HasMaxLength(300);

                    sa.Property(c => c.AddressLine2)
                        .IsRequired(false)
                        .HasMaxLength(300);

                    sa.Property(c => c.Suburb)
                        .IsRequired()
                        .HasMaxLength(200);

                    sa.Property(c => c.State)
                        .IsRequired()
                        .HasMaxLength(100);

                    sa.Property(c => c.PostCode)
                        .IsRequired()
                        .HasMaxLength(20);
                });

            builder.Property(t => t.PhoneNumber)
                .IsRequired()
                .HasMaxLength(200);


            builder.Property(t => t.HasOptIn)
                .IsRequired()
                .HasDefaultValue(false);

            builder.HasOne<CompetitionRound>()
                .WithMany()
                .HasForeignKey(t => t.CompetitionRoundId)
                .OnDelete(DeleteBehavior.Cascade);

        }
    }
}

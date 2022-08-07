using Microsoft.EntityFrameworkCore;

namespace CodeWord.Infrastructure
{
    public class CodeWordDBContextSeeder
    {
        public async Task SeedAsync(CodeWordDBContext context)
        {
            using (context)
            {
                if (!await context.Competitions.AsNoTracking().AnyAsync())
                {
                    var seedData = new Competition(Guid.NewGuid());
                    var codeWords = new string[] { "Album", "Single", "Record", "Music", "Sound", "Chart", "Bass", "Vocal", "Drums", "Guitar",
                        "Vinyl", "Stage", "Microphone", "Beat", "Concert"};
                    seedData.StartCompetition(new DateTime(2022, 08, 1, 0, 0, 0, DateTimeKind.Utc), new DateTime(2022, 08, 15, 0, 0, 0, DateTimeKind.Utc), codeWords);
                    await context.Competitions.AddAsync(seedData);
                    await context.SaveChangesAsync();
                }
            }
        }
    }
}

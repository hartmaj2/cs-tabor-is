using Microsoft.EntityFrameworkCore;
using Shared;

namespace Server.Data
{
    public class ParticipantsDbContext : DbContext
    {
        public ParticipantsDbContext(DbContextOptions<ParticipantsDbContext> options)
            : base(options)
        {
        }

        public DbSet<Participant> Participants { get; set; }
    }
}

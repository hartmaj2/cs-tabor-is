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

        // DbSet is a substitute for my Participants SQL Table, which contains entries of type Participant (attributes = fields of Participant.cs model)
        public DbSet<Participant> Participants { get; set; }
    }
}

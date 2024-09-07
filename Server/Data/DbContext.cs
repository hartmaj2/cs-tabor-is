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
        public DbSet<Meal> Meals { get; set; }
        public DbSet<Allergen> Allergens { get; set; }
        public DbSet<MealAllergen> MealAllergens { get; set; }

        public DbSet<ParticipantAllergen> ParticipantAllergens { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<MealAllergen>()
            .HasKey(ma => new { ma.MealId, ma.AllergenId });

    }
    }
}

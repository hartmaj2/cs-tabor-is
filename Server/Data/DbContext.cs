using Microsoft.EntityFrameworkCore;

namespace Server.Data
{

    // DB Context used by Entity Framework to allow me mapping the DbSets to my Tables
    public class TaborIsDbContext : DbContext
    {
        public TaborIsDbContext(DbContextOptions<TaborIsDbContext> options)
            : base(options)
        {
        }

        // DbSet of a type Type is a substitute for an Table in my TaborIS database, which contains entries of type Type
        public DbSet<Participant> Participants { get; set; }
        public DbSet<Meal> Meals { get; set; }
        public DbSet<Allergen> Allergens { get; set; }
        public DbSet<MealAllergen> MealAllergens { get; set; }

        public DbSet<ParticipantAllergen> ParticipantAllergens { get; set; }

        public DbSet<Order> Orders { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<MealAllergen>()
                .HasKey(ma => new { ma.MealId, ma.AllergenId });

        }
    }
}

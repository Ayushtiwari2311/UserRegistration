using Domain.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.DatabaseContext
{
    public class AppDbContext : IdentityDbContext<ApplicationUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<TrnUserRegistration> TrnUserRegistrations { get; set; }
        public DbSet<MGender> MGenders { get; set; }
        public DbSet<MState> MStates { get; set; }
        public DbSet<MCity> MCities { get; set; }
        public DbSet<MHobby> MHobbies { get; set; }
        public DbSet<TrnUserHobby> MUserHobbies { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<TrnUserHobby>()
                .HasKey(uh => new { uh.UserId, uh.HobbyId });

            modelBuilder.Entity<TrnUserHobby>()
                .HasOne(uh => uh.User)
                .WithMany(u => u.UserHobbies)
                .HasForeignKey(uh => uh.UserId);

            modelBuilder.Entity<TrnUserHobby>()
                .HasOne(uh => uh.Hobby)
                .WithMany()
                .HasForeignKey(uh => uh.HobbyId);
            modelBuilder.Entity<TrnUserRegistration>()
                .HasOne(u => u.State)
                .WithMany()
                .HasForeignKey(u => u.StateId)
                .OnDelete(DeleteBehavior.Restrict); // <- Important

            modelBuilder.Entity<TrnUserRegistration>()
                .HasOne(u => u.City)
                .WithMany()
                .HasForeignKey(u => u.CityId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<TrnUserRegistration>()
                .HasOne(u => u.Gender)
                .WithMany()
                .HasForeignKey(u => u.GenderId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<TrnUserRegistration>()
                .HasIndex(u => u.Email)
                .IsUnique();
        }
    }
}

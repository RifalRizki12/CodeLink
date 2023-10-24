using API.Models;
using Microsoft.EntityFrameworkCore;

namespace API.Data
{
    public class CodeLinkDbContext : DbContext
    {
        public CodeLinkDbContext(DbContextOptions<CodeLinkDbContext> options) : base(options) { }

        //add model to migrate
        public DbSet<Account> Accounts { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Experience> Experiences { get; set; }
        public DbSet<CurriculumVitae> CurriculumVitaes { get; set; }
        public DbSet<Rating> Ratings { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Skill> Skills { get; set; }
        public DbSet<Interview> Tests { get; set; }
        public DbSet<Company> Companies { get; set; }

        //pembutan method overrid untuk atribut uniq
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Employee>().HasIndex(e => e.Email).IsUnique();
            modelBuilder.Entity<Employee>().HasIndex(e => e.PhoneNumber).IsUnique();

            // One Skill has many CurriculumVitaes
            modelBuilder.Entity<Skill>()
                .HasOne(c => c.CurriculumVitae)
                .WithMany(s => s.Skills)
                .HasForeignKey(s => s.CvGuid)
                .OnDelete(DeleteBehavior.Restrict);

            // One Experience has many CurriculumVitaes
            modelBuilder.Entity<Experience>()
                .HasOne(c => c.CurriculumVitae)
                .WithMany(ex => ex.Experiences)
                .HasForeignKey(ex => ex.CvGuid)
                .OnDelete(DeleteBehavior.Restrict);

            // One Employee has one CuriculumVitae
            modelBuilder.Entity<Employee>()
                .HasOne(e => e.CurriculumVitae)
                .WithOne(em => em.Employee)
                .HasForeignKey<CurriculumVitae>(e => e.Guid)
                .OnDelete(DeleteBehavior.Restrict);

            // One employee has One Account
            modelBuilder.Entity<Employee>()
                .HasOne(a => a.Account)
                .WithOne(e => e.Employee)
                .HasForeignKey<Account>(a => a.Guid)
                .OnDelete(DeleteBehavior.Restrict);

            // One Role has Many Account
            modelBuilder.Entity<Role>()
            .HasMany(ar => ar.Accounts)
            .WithOne(e => e.Role)
            .HasForeignKey(ar => ar.RoleGuid)
            .OnDelete(DeleteBehavior.Restrict);

            // One Employee has Many Rating
            modelBuilder.Entity<Rating>()
                .HasOne(i => i.Interview)
                .WithOne(r => r.Rating)
                .HasForeignKey<Rating>(r => r.Guid)
                .OnDelete(DeleteBehavior.Restrict);

            // One Employee has Many Test
            modelBuilder.Entity<Employee>()
                .HasMany(t => t.Tests)
                .WithOne(e => e.Employee)
                .HasForeignKey(t => t.EmployeeGuid)
                .OnDelete(DeleteBehavior.Restrict);

            // One Employee has Many Company
            modelBuilder.Entity<Employee>()
                .HasMany(c => c.Companies)
                .WithOne(e => e.Employee)
                .HasForeignKey(c => c.EmployeeGuid)
                .OnDelete(DeleteBehavior.Restrict);

            // One Company has Many Employee
            modelBuilder.Entity<Company>()
                .HasMany(e => e.Employees)
                .WithOne(c => c.Company)
                .HasForeignKey(e => e.CompanyGuid)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}

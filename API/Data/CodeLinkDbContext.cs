using API.Models;
using Microsoft.EntityFrameworkCore;

namespace API.Data
{
    public class CodeLinkDbContext : DbContext
    {
        public CodeLinkDbContext(DbContextOptions<CodeLinkDbContext> options) : base(options) { }

        //add model to migrate
        public DbSet<Account> Accounts { get; set; }
        public DbSet<AccountRole> AccountRoles { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Experience> Experiences { get; set; }
        public DbSet<ExperienceSkill> ExperienceSkills { get; set; }
        public DbSet<Rating> Ratings { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Salary> Salaries { get; set; }
        public DbSet<Skill> Skills { get; set; }
        public DbSet<Test> Tests { get; set; }

        //pembutan method overrid untuk atribut uniq

        //pembutan method overrid untuk atribut uniq
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Employee>().HasIndex(e => e.Email).IsUnique();
            modelBuilder.Entity<Employee>().HasIndex(e => e.PhoneNumber).IsUnique();

            // One Skill has many ExperienceSkills
            modelBuilder.Entity<Skill>()
                .HasMany(e => e.ExperienceSkills)
                .WithOne(s => s.Skill)
                .HasForeignKey(e => e.SkillGuid)
                .OnDelete(DeleteBehavior.Restrict);

            // One Experience has many ExperienceSkills
            modelBuilder.Entity<Experience>()
                .HasMany(e => e.ExperienceSkills)
                .WithOne(ex => ex.Experience)
                .HasForeignKey(e => e.ExperienceGuid)
                .OnDelete(DeleteBehavior.Restrict);

            // One Employee has many ExperienceSkills
            modelBuilder.Entity<Employee>()
                .HasMany(e => e.ExperienceSkills)
                .WithOne(em => em.Employee)
                .HasForeignKey(e => e.EmployeeGuid)
                .OnDelete(DeleteBehavior.Restrict);

            // One employee has One Account
            modelBuilder.Entity<Employee>()
                .HasOne(a => a.Account)
                .WithOne(e => e.Employee)
                .HasForeignKey<Account>(a => a.Guid)
                .OnDelete(DeleteBehavior.Restrict);

            // One Account has Many AccountRole
            modelBuilder.Entity<Account>()
            .HasMany(ar => ar.AccountRoles)
            .WithOne(e => e.Account)
            .HasForeignKey(ar => ar.AccountGuid)
            .OnDelete(DeleteBehavior.Restrict);

            // One Role has Many AccountRole
            modelBuilder.Entity<AccountRole>()
                .HasOne(r => r.Role)
                .WithMany(ar => ar.AccountRoles)
                .HasForeignKey(ar => ar.RoleGuid)
                .OnDelete(DeleteBehavior.Restrict);

            // One Employee has One Salary
            modelBuilder.Entity<Employee>()
                .HasOne(s => s.Salary)
                .WithOne(e => e.Employee)
                .HasForeignKey<Employee>(s => s.Guid)
                .OnDelete(DeleteBehavior.Restrict);

            // One Employee has Many Rating
            modelBuilder.Entity<Employee>()
                .HasMany(r => r.Ratings)
                .WithOne(e => e.Employee)
                .HasForeignKey(r => r.EmployeeGuid)
                .OnDelete(DeleteBehavior.Restrict);

            // One Employee has Many Test
            modelBuilder.Entity<Employee>()
                .HasMany(t => t.Tests)
                .WithOne(e => e.Employee)
                .HasForeignKey(t => t.EmployeeGuid)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}

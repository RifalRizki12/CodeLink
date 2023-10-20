using Microsoft.EntityFrameworkCore;

namespace API.Data
{
    public class CodeLinkDbContext : DbContext
    {
        public CodeLinkDbContext(DbContextOptions<CodeLinkDbContext> options) : base(options) { }

        //add model to migrate


        //pembutan method overrid untuk atribut uniq
/*        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Employee>().HasIndex(e => new
            {
                e.Nik,
                e.Email,
                e.PhoneNumber
            }).IsUnique();
        }*/
    }
}

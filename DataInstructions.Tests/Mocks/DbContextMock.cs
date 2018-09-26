namespace EEFApps.ApiInstructions.DataInstructions.Tests.Mocks
{
    using Microsoft.EntityFrameworkCore;

    public class DbContextMock: DbContext
    {
        public DbContextMock() : base()
        {

        }

        public DbContextMock(DbContextOptions options): base(options)
        {

        }

        public virtual DbSet<CountryEntityMock> Countries { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            
        }

    }
}

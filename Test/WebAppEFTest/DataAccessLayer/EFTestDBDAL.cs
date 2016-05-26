using BusinessEntities;
using System.Data.Entity;

namespace DataAccessLayer
{
    public class EFTestDBDAL : DbContext
    {
        public EFTestDBDAL() : base("connectionString")
        {

        }

        public DbSet<Employee> Employees { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Employee>().ToTable("TblEmployee");
            base.OnModelCreating(modelBuilder);
        }
    }
}
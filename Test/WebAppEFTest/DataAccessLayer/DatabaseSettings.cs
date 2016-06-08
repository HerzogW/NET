
namespace DataAccessLayer
{
    using System.Data.Entity;

    /// <summary>
    /// 
    /// </summary>
    public class DatabaseSettings
    {
        /// <summary>
        /// Sets the database.
        /// </summary>
        public static void SetDatabase()
        {
            Database.SetInitializer(new DropCreateDatabaseIfModelChanges<EFTestDBDAL>());
        }
    }
}

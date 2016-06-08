using DataAccessLayer;

namespace BusinessLayer
{
    /// <summary>
    /// 
    /// </summary>
    public class BusinessSettings
    {

        /// <summary>
        /// Sets the business.
        /// </summary>
        public static void SetBusiness()
        {
            DatabaseSettings.SetDatabase();
        }
    }
}

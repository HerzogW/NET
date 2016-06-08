using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebAppEFTest.Areas.SPA.Model
{
    /// <summary>
    /// 
    /// </summary>
    public class CreateEmployeeViewModel
    {

        /// <summary>
        /// Gets or sets the first name.
        /// </summary>
        /// <value>
        /// The first name.
        /// </value>
        public string FirstName { get; set; }

        /// <summary>
        /// Gets or sets the last name.
        /// </summary>
        /// <value>
        /// The last name.
        /// </value>
        public string LastName { get; set; }

        /// <summary>
        /// Gets or sets the salary.
        /// </summary>
        /// <value>
        /// The salary.
        /// </value>
        public string Salary { get; set; }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using ViewModel;

namespace BusinessEntities
{
    public class Employee
    {
        [Key]
        public string EmployeeId { get; set; }

        [Required(ErrorMessage = "Enter First Name")]
        public string FirstName { get; set; }

        [StringLength(5, ErrorMessage = "Last Name length should not be greater than 5")]
        public string LastName { get; set; }

        public int Salary { get; set; }
    }
}
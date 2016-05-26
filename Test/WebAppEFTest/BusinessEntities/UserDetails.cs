using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BusinessEntities
{
    public class UserDetails
    {
        [StringLength(7, MinimumLength = 2, ErrorMessage = "UserName length should be between 2 and 7")]
        public string UserName { get; set; }

        [StringLength(16, MinimumLength = 5, ErrorMessage = "Password length should be between 5 and 16")]
        public string Password { get; set; }
    }
}
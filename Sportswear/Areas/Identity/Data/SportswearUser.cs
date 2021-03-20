using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace Sportswear.Areas.Identity.Data
{
    // Add profile data for application users by adding properties to the SportswearUser class
    public class SportswearUser : IdentityUser
    {
        //[PersonalData]
        //public string UserName { get; set; }

        //[PersonalData]
        //public string Email { get; set; }

        [PersonalData]
        public string userRole { get; set; }
    }
}

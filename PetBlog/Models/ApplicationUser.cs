using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace PetBlog.Models
{
    // Add profile data for application users by adding properties to the ApplicationUser class
    public class ApplicationUser : IdentityUser
    {
        //one to one relationship between user and owner
        [ForeignKey("OwnerID")]
        public int? OwnerID { get; set; }

        //An application user is tied to an owner
        public virtual Owner owner { get; set; }
    }
}

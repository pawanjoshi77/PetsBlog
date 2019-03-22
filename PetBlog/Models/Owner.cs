using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PetBlog.Models
{
    public class Owner
    {
        [Key, ScaffoldColumn(false)]
        public int OwnerID { get; set; }

        [Required, StringLength(255), Display(Name = "Name")]
        public string OwnerName { get; set; }

        [Required, StringLength(255), Display(Name = "Name")]
        public string OwnerAddress { get; set; }

        [Required, Display(Name = "Member Since")]
        public DateTime MemberSince { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
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

        [Required, StringLength(255), Display(Name = "Address")]
        public string OwnerAddress { get; set; }

        [Required, Display(Name = "Member Since")]
        public DateTime MemberSince { get; set; }

        //Representing one owner to many pets.
        [InverseProperty("Owner")]
        public List<Pet> Pets { get; set; }

        //one to one relation between owners and users
        [ForeignKey("UserID")]
        public string UserID { get; set; }

        public virtual ApplicationUser user { get; set; }
}
}

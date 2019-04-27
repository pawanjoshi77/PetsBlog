using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace PetBlog.Models
{
    public class Pet
    {
        
        [Key, ScaffoldColumn(false)]
        public int PetID { get; set; }

        [Required, StringLength(255), Display(Name = "Pet Name")]
        public string PetName { get; set; }

        [Required, StringLength(255), Display(Name = "Pet Type")]
        public string PetType { get; set; }

        [Required, StringLength(255), Display(Name = "Size")]
        public string PetSize { get; set; }

        [Required, Display(Name = "Date Of Birth")]
        public DateTime PetDOB { get; set; }

        [Required, StringLength(255), Display(Name = "Species Gender")]
        public string PetGender { get; set; }

        public int HasPic { get; set; }

        public string ImgType { get; set; }

        //Pet has species ID
        [ForeignKey("SpeciesID")]
        public int SpeciesID { get; set; }

        //Pet Species
        public virtual Species Species { get; set; }

        //Pet has OwnerID
        [ForeignKey("OwnerID")]
        public int OwnerID { get; set; }

        //Pet Owners
        public virtual Owner Owner { get; set; }
    }
}

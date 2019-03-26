using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace PetBlog.Models
{
    public class Species
    {
        [Key, ScaffoldColumn(false)]
        public int SpeciesID { get; set; }

        [Required, StringLength(255), Display(Name = "Species Name")]
        public string SpeciesName { get; set; }

        //Species has PetID
        [ForeignKey("PetID")]
        public int PetID { get; set; }

        //One species to many pets.
        [InverseProperty("Species")]
        public virtual List<Pet> Pets { get; set; }
    }
}

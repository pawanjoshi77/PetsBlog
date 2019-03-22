using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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

        [Required, StringLength(255), Display(Name = "Species")]
        public string PetSpecies { get; set; }

        [Required, StringLength(255), Display(Name = "Size")]
        public string PetSize { get; set; }

        [Required, Display(Name = "DateOfBirth")]
        public DateTime PetDOB { get; set; }

    }
}

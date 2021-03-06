﻿using System;
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

        [Required, StringLength(255), Display(Name = "Species Type")]
        public string SpeciesType { get; set; }

        [Required, StringLength(255), Display(Name = "Species Gender")]
        public string SpeciesGender { get; set; }

        //Species has PetID
        [ForeignKey("PetID")]
        public int PetID { get; set; }

        //One species to many pets.
        [InverseProperty("Species")]
        public virtual List<Pet> Pets { get; set; }

        //Pet Owners
        public virtual Owner Owner { get; set; }

        //Pet has OwnerID
        [ForeignKey("OwnerID")]
        public int OwnerID { get; set; }
    }
}

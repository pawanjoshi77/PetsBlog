using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System.Security.Claims;
using System.Threading.Tasks;
using PetBlog.Models;

namespace PetBlog.Data
{
    public class PetsBlogContext: IdentityDbContext<ApplicationUser>
    {
        public PetsBlogContext(DbContextOptions<PetsBlogContext> options)
            : base(options)
        {

        }

        public DbSet<Pet> Pets { get; set; }
        public DbSet<Owner> Owners { get; set; }
        public DbSet<Species> Species { get; set; }
        public object Pet { get; internal set; }

        protected override void OnModelCreating(ModelBuilder modelbuilder)
        {
            //Each Pet has one owner and Owner has many pets
            modelbuilder.Entity<Pet>()
                .HasOne(p => p.Owner)
                .WithMany(o => o.Pets)
                .HasForeignKey(p => p.OwnerID);

            //Each pet has one species and there can be many species of pets
            modelbuilder.Entity<Pet>()
                .HasOne(p => p.Species)
                .WithMany(s => s.Pets)
                .HasForeignKey(s => s.PetID);

            //Each owner has one user and that user has one owner
            modelbuilder.Entity<Owner>()
                .HasOne(o => o.user)
                .WithOne(u => u.owner)
                .HasForeignKey<ApplicationUser>(u => u.OwnerID);

            base.OnModelCreating(modelbuilder);
            modelbuilder.Entity<Pet>().ToTable("Pets");
            modelbuilder.Entity<Owner>().ToTable("Owners");
            modelbuilder.Entity<Species>().ToTable("Species");

        }
    }
}

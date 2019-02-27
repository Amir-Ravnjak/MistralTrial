using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MistralTrialAPI.Data
{
    public class MyContext : DbContext
    {
        public MyContext(DbContextOptions<MyContext> options):base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);


            modelBuilder.Entity<Title>()
                .HasOne(pt => pt.ImgFile)
                .WithMany()
                .HasForeignKey(pt => pt.ImgFileId)
                .OnDelete(DeleteBehavior.Restrict);
        }
        public DbSet<Actors> Actors { get; set; }
        public DbSet<ImgFile> ImgFiles { get; set; }
        public DbSet<Rate> Rates { get; set; }
        public DbSet<Title> Titles { get; set; }
        public DbSet<TitleActors> TitleActors { get; set; }
        public DbSet<User> Users { get; set; }
    }
}

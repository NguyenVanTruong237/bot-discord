using Microsoft.EntityFrameworkCore;
using System;

namespace Infrastructure
{
    public class ApplicationDbContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer
                ("Server=(localdb)\\mssqllocaldb;Database=bot-discord;Trusted_Connection=True;MultipleActiveResultSets=true;");
        }
        public DbSet<Sever> Severs { get; set; }

        public class Sever
        {
            public int Id { get; set; }
            public string Prefix { get; set; }
        }
    }
}

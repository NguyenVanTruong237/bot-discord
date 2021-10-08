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
            public ulong Id { get; set; }
            public string Prefix { get; set; }
            public ulong Welcome { get; set; }
            public string Background { get; set; }
        }
    }
}

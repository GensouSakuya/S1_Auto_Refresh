using Core.Models;
using Microsoft.EntityFrameworkCore;
using System.IO;

namespace Core
{

    public class SQLiteDb : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder options)
            => options.UseSqlite($"Data Source={Directory.GetCurrentDirectory()}\\keepers.db;Version=3;");

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<KeeperModel>().HasKey(p => p.Id);
            modelBuilder.Entity<KeeperModel>().Property(p => p.Key);
            modelBuilder.Entity<KeeperModel>().Property(p => p.InitKey);
        }
    }
}

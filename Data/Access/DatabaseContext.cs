using Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Access
{
    public class DatabaseContext : DbContext
    {
        public DbSet<Container> Container
        {
            get; set;
        }
        public DbSet<Source> Source
        {
            get;
            set;
        }
        public DbSet<Observation> Observation
        {
            get;
            set;
        }
        public DbSet<Channel> Channel
        {
            get; set;
        }
        public DbSet<Series> Series
        {
            get; set;
        }
        public string DbPath
        {
            get;
        }

        public DatabaseContext()
        {
            var folder = Environment.SpecialFolder.LocalApplicationData;
            var path = Environment.GetFolderPath(folder);
            DbPath = Path.Join(path, @"PQDIF\sqlite.db");
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            string currentDirectory = Directory.GetCurrentDirectory();
            string databasePath = Path.Combine(currentDirectory, "sqlite.db");

            optionsBuilder.UseSqlite(
                $"Data Source={this.DbPath}; Foreign Keys=True;");
        }
    }
}

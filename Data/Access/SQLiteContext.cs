using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Data.Access;
public class SQLiteContext : DatabaseContext
{
    private readonly string DbPath;
    public SQLiteContext()
    {
        var folder = Environment.SpecialFolder.LocalApplicationData;
        var path = Environment.GetFolderPath(folder);
        DbPath = Path.Join(path, @"PQDIF\sqlite.db");
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        var currentDirectory = Directory.GetCurrentDirectory();
        var databasePath = Path.Combine(currentDirectory, "sqlite.db");

        optionsBuilder.UseSqlite($"Data Source={this.DbPath}; Foreign Keys=True;");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Series>()
            .Property(c => c.IsSync)
            .HasDefaultValue(false);
    }
}

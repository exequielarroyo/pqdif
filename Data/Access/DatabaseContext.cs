using Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace Data.Access;

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

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Series>()
            .Property(c => c.IsSync)
            .HasDefaultValue(false);
    }
}

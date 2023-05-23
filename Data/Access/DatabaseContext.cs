using Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.NetworkInformation;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace Data.Access;

public class DatabaseContext : DbContext
{
    public DbSet<Container> Containers
    {
        get; set;
    }
    public DbSet<Source> Sources
    {
        get;
        set;
    }
    public DbSet<Observation> Observations
    {
        get;
        set;
    }
    public DbSet<Channel> Channels
    {
        get; set;
    }
    public DbSet<Series> Series
    {
        get; set;
    }
}

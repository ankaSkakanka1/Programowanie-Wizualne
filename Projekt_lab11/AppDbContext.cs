using Microsoft.EntityFrameworkCore;
using BioManager.Models;

namespace BioManager.Data;

public class AppDbContext : DbContext
{
    public DbSet<BiologicalSample> Samples => Set<BiologicalSample>();

    protected override void OnConfiguring(DbContextOptionsBuilder options)
        => options.UseSqlite("Data Source=samples.db"); 
}
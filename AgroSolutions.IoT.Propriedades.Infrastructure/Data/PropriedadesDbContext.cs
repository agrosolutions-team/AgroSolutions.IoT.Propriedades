using AgroSolutions.IoT.Propriedades.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace AgroSolutions.IoT.Propriedades.Infrastructure.Data;

public class PropriedadesDbContext : DbContext
{
    public PropriedadesDbContext(DbContextOptions<PropriedadesDbContext> options) : base(options)
    {
    }

    public DbSet<Propriedade> Propriedades { get; set; }
    public DbSet<Talhao> Talhoes { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(PropriedadesDbContext).Assembly);
    }
}

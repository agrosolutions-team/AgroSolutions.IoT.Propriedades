using AgroSolutions.IoT.Propriedades.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AgroSolutions.IoT.Propriedades.Infrastructure.Data.Configurations;

public class TalhaoConfiguration : IEntityTypeConfiguration<Talhao>
{
    public void Configure(EntityTypeBuilder<Talhao> builder)
    {
        builder.ToTable("Talhoes");

        builder.HasKey(t => t.Id);

        builder.Property(t => t.Id)
            .ValueGeneratedOnAdd()
            .HasDefaultValueSql("gen_random_uuid()");

        builder.Property(t => t.Nome)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(t => t.AreaEmHectares)
            .IsRequired()
            .HasPrecision(18, 2);

        builder.Property(t => t.CulturaPlantada)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(t => t.PropriedadeId)
            .IsRequired();

        builder.HasOne(t => t.Propriedade)
            .WithMany(p => p.Talhoes)
            .HasForeignKey(t => t.PropriedadeId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}

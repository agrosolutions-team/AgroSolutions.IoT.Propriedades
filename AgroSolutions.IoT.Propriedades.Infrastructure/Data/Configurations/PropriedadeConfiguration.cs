using AgroSolutions.IoT.Propriedades.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AgroSolutions.IoT.Propriedades.Infrastructure.Data.Configurations;

public class PropriedadeConfiguration : IEntityTypeConfiguration<Propriedade>
{
    public void Configure(EntityTypeBuilder<Propriedade> builder)
    {
        builder.ToTable("Propriedades");

        builder.HasKey(p => p.Id);

        builder.Property(p => p.Id)
            .ValueGeneratedOnAdd()
            .HasDefaultValueSql("gen_random_uuid()");

        builder.Property(p => p.Nome)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(p => p.Descricao)
            .HasMaxLength(500);

        builder.Property(p => p.ProdutorId)
            .IsRequired();

        builder.HasMany(p => p.Talhoes)
            .WithOne(t => t.Propriedade)
            .HasForeignKey(t => t.PropriedadeId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Metadata
            .FindNavigation(nameof(Propriedade.Talhoes))!
            .SetPropertyAccessMode(PropertyAccessMode.Field);
    }
}

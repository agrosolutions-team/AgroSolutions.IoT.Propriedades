using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using AgroSolutions.IoT.Propriedades.Infrastructure.Data;

#nullable disable

namespace AgroSolutions.IoT.Propriedades.Infrastructure.Migrations
{
    [DbContext(typeof(PropriedadesDbContext))]
    partial class PropriedadesDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.0")
                .HasAnnotation("Npgsql:PostgresExtension:pgcrypto", ",,")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            modelBuilder.Entity("AgroSolutions.IoT.Propriedades.Domain.Entities.Propriedade", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasDefaultValueSql("gen_random_uuid()");

                    b.Property<string>("Descricao")
                        .HasMaxLength(500)
                        .HasColumnType("character varying(500)");

                    b.Property<string>("Nome")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("character varying(200)");

                    b.Property<Guid>("ProdutorId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.ToTable("Propriedades", (string)null);
                });

            modelBuilder.Entity("AgroSolutions.IoT.Propriedades.Domain.Entities.Talhao", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasDefaultValueSql("gen_random_uuid()");

                    b.Property<decimal>("AreaEmHectares")
                        .HasPrecision(18, 2)
                        .HasColumnType("numeric(18,2)");

                    b.Property<string>("CulturaPlantada")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.Property<string>("Nome")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("character varying(200)");

                    b.Property<Guid>("PropriedadeId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("PropriedadeId");

                    b.ToTable("Talhoes", (string)null);
                });

            modelBuilder.Entity("AgroSolutions.IoT.Propriedades.Domain.Entities.Talhao", b =>
                {
                    b.HasOne("AgroSolutions.IoT.Propriedades.Domain.Entities.Propriedade", "Propriedade")
                        .WithMany("Talhoes")
                        .HasForeignKey("PropriedadeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Propriedade");
                });

            modelBuilder.Entity("AgroSolutions.IoT.Propriedades.Domain.Entities.Propriedade", b =>
                {
                    b.Navigation("Talhoes");
                });
#pragma warning restore 612, 618
        }
    }
}

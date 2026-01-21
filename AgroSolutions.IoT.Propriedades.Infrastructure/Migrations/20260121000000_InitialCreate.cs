using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AgroSolutions.IoT.Propriedades.Infrastructure.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("Npgsql:PostgresExtension:pgcrypto", ",,");

            migrationBuilder.CreateTable(
                name: "Propriedades",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    Nome = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Descricao = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    ProdutorId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Propriedades", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Talhoes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    Nome = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    AreaEmHectares = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    CulturaPlantada = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    PropriedadeId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Talhoes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Talhoes_Propriedades_PropriedadeId",
                        column: x => x.PropriedadeId,
                        principalTable: "Propriedades",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Talhoes_PropriedadeId",
                table: "Talhoes",
                column: "PropriedadeId");

            var propriedadeId = Guid.NewGuid();
            var produtorId = Guid.NewGuid();
            var talhaoId = Guid.NewGuid();

            migrationBuilder.Sql($@"
                INSERT INTO ""Propriedades"" (""Id"", ""Nome"", ""Descricao"", ""ProdutorId"")
                SELECT '{propriedadeId}', 'Propriedade Modelo', 'Propriedade criada automaticamente via migration', '{produtorId}'
                WHERE NOT EXISTS (SELECT 1 FROM ""Propriedades"" WHERE ""Id"" = '{propriedadeId}');
            ");

            migrationBuilder.Sql($@"
                INSERT INTO ""Talhoes"" (""Id"", ""Nome"", ""AreaEmHectares"", ""CulturaPlantada"", ""PropriedadeId"")
                SELECT '{talhaoId}', 'Talhão 01', 10, 'Soja', '{propriedadeId}'
                WHERE NOT EXISTS (SELECT 1 FROM ""Talhoes"" WHERE ""Id"" = '{talhaoId}');
            ");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Talhoes");

            migrationBuilder.DropTable(
                name: "Propriedades");
        }
    }
}

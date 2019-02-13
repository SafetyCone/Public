using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ExaminingEntityFramework.Migrations.Migrations
{
    public partial class EntitiesBAndC : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "EntityBs",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EntityBs", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "EntityCs",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EntityCs", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "EntityBToEntityCMappings",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    EntityBID = table.Column<int>(nullable: false),
                    EntityCID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EntityBToEntityCMappings", x => x.ID);
                    table.ForeignKey(
                        name: "FK_EntityBToEntityCMappings_EntityBs_EntityBID",
                        column: x => x.EntityBID,
                        principalTable: "EntityBs",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_EntityBToEntityCMappings_EntityCs_EntityCID",
                        column: x => x.EntityCID,
                        principalTable: "EntityCs",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EntityBToEntityCMappings_EntityBID",
                table: "EntityBToEntityCMappings",
                column: "EntityBID");

            migrationBuilder.CreateIndex(
                name: "IX_EntityBToEntityCMappings_EntityCID",
                table: "EntityBToEntityCMappings",
                column: "EntityCID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EntityBToEntityCMappings");

            migrationBuilder.DropTable(
                name: "EntityBs");

            migrationBuilder.DropTable(
                name: "EntityCs");
        }
    }
}

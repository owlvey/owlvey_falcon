using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Owlvey.Falcon.Data.SQLite.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AppSettingEntity",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<string>(nullable: false),
                    ModifiedOn = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<string>(nullable: false),
                    Key = table.Column<string>(nullable: false),
                    Value = table.Column<string>(nullable: false),
                    IsReadOnly = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppSettingEntity", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CustomerEntity",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<string>(nullable: false),
                    ModifiedOn = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<string>(nullable: false),
                    Name = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomerEntity", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SquadEntity",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<string>(nullable: false),
                    ModifiedOn = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<string>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    Description = table.Column<string>(nullable: false),
                    Avatar = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SquadEntity", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ProductEntity",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<string>(nullable: false),
                    ModifiedOn = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<string>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    Description = table.Column<string>(nullable: false),
                    CustomerId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductEntity", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductEntity_CustomerEntity_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "CustomerEntity",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "UserEntity",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<string>(nullable: false),
                    ModifiedOn = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<string>(nullable: false),
                    Email = table.Column<string>(nullable: false),
                    SquadEntityId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserEntity", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserEntity_SquadEntity_SquadEntityId",
                        column: x => x.SquadEntityId,
                        principalTable: "SquadEntity",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ServiceEntity",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<string>(nullable: false),
                    ModifiedOn = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<string>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    Description = table.Column<string>(nullable: false),
                    SLO = table.Column<float>(nullable: false),
                    Avatar = table.Column<string>(nullable: true),
                    ProductEntityId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ServiceEntity", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ServiceEntity_ProductEntity_ProductEntityId",
                        column: x => x.ProductEntityId,
                        principalTable: "ProductEntity",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "FeatureEntity",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<string>(nullable: false),
                    ModifiedOn = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<string>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    Avatar = table.Column<string>(nullable: true),
                    ProductEntityId = table.Column<int>(nullable: true),
                    ServiceEntityId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FeatureEntity", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FeatureEntity_ProductEntity_ProductEntityId",
                        column: x => x.ProductEntityId,
                        principalTable: "ProductEntity",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FeatureEntity_ServiceEntity_ServiceEntityId",
                        column: x => x.ServiceEntityId,
                        principalTable: "ServiceEntity",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "JournalEntity",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<string>(nullable: false),
                    ModifiedOn = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<string>(nullable: false),
                    GoodDefinition = table.Column<string>(nullable: false),
                    TotalDefinition = table.Column<string>(nullable: false),
                    Avatar = table.Column<string>(nullable: true),
                    FeatureEntityId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JournalEntity", x => x.Id);
                    table.ForeignKey(
                        name: "FK_JournalEntity_FeatureEntity_FeatureEntityId",
                        column: x => x.FeatureEntityId,
                        principalTable: "FeatureEntity",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "JournalItemEntity",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<string>(nullable: false),
                    ModifiedOn = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<string>(nullable: false),
                    JournalId = table.Column<int>(nullable: true),
                    Good = table.Column<int>(nullable: false),
                    Total = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JournalItemEntity", x => x.Id);
                    table.ForeignKey(
                        name: "FK_JournalItemEntity_JournalEntity_JournalId",
                        column: x => x.JournalId,
                        principalTable: "JournalEntity",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FeatureEntity_ProductEntityId",
                table: "FeatureEntity",
                column: "ProductEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_FeatureEntity_ServiceEntityId",
                table: "FeatureEntity",
                column: "ServiceEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_JournalEntity_FeatureEntityId",
                table: "JournalEntity",
                column: "FeatureEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_JournalItemEntity_JournalId",
                table: "JournalItemEntity",
                column: "JournalId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductEntity_CustomerId",
                table: "ProductEntity",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_ServiceEntity_ProductEntityId",
                table: "ServiceEntity",
                column: "ProductEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_UserEntity_SquadEntityId",
                table: "UserEntity",
                column: "SquadEntityId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AppSettingEntity");

            migrationBuilder.DropTable(
                name: "JournalItemEntity");

            migrationBuilder.DropTable(
                name: "UserEntity");

            migrationBuilder.DropTable(
                name: "JournalEntity");

            migrationBuilder.DropTable(
                name: "SquadEntity");

            migrationBuilder.DropTable(
                name: "FeatureEntity");

            migrationBuilder.DropTable(
                name: "ServiceEntity");

            migrationBuilder.DropTable(
                name: "ProductEntity");

            migrationBuilder.DropTable(
                name: "CustomerEntity");
        }
    }
}

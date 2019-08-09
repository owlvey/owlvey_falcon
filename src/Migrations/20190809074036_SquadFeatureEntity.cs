using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Owlvey.Falcon.Migrations
{
    public partial class SquadFeatureEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FeatureEntity_ProductEntity_ProductId",
                table: "FeatureEntity");

            migrationBuilder.DropForeignKey(
                name: "FK_IndicatorEntity_FeatureEntity_FeatureId",
                table: "IndicatorEntity");

            migrationBuilder.DropForeignKey(
                name: "FK_IndicatorEntity_SourceEntity_SourceId",
                table: "IndicatorEntity");

            migrationBuilder.DropForeignKey(
                name: "FK_ServiceEntity_ProductEntity_ProductId",
                table: "ServiceEntity");

            migrationBuilder.DropForeignKey(
                name: "FK_SourceItemEntity_SourceEntity_SourceId",
                table: "SourceItemEntity");

            migrationBuilder.AlterColumn<int>(
                name: "SourceId",
                table: "SourceItemEntity",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "ProductId",
                table: "ServiceEntity",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "SourceId",
                table: "IndicatorEntity",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "FeatureId",
                table: "IndicatorEntity",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "ProductId",
                table: "FeatureEntity",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "SquadFeatureEntity",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<string>(nullable: false),
                    ModifiedOn = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<string>(nullable: false),
                    FeatureId = table.Column<int>(nullable: false),
                    SquadId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SquadFeatureEntity", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SquadFeatureEntity_FeatureEntity_FeatureId",
                        column: x => x.FeatureId,
                        principalTable: "FeatureEntity",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SquadFeatureEntity_SquadEntity_SquadId",
                        column: x => x.SquadId,
                        principalTable: "SquadEntity",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SquadFeatureEntity_FeatureId",
                table: "SquadFeatureEntity",
                column: "FeatureId");

            migrationBuilder.CreateIndex(
                name: "IX_SquadFeatureEntity_SquadId",
                table: "SquadFeatureEntity",
                column: "SquadId");

            migrationBuilder.AddForeignKey(
                name: "FK_FeatureEntity_ProductEntity_ProductId",
                table: "FeatureEntity",
                column: "ProductId",
                principalTable: "ProductEntity",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_IndicatorEntity_FeatureEntity_FeatureId",
                table: "IndicatorEntity",
                column: "FeatureId",
                principalTable: "FeatureEntity",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_IndicatorEntity_SourceEntity_SourceId",
                table: "IndicatorEntity",
                column: "SourceId",
                principalTable: "SourceEntity",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ServiceEntity_ProductEntity_ProductId",
                table: "ServiceEntity",
                column: "ProductId",
                principalTable: "ProductEntity",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SourceItemEntity_SourceEntity_SourceId",
                table: "SourceItemEntity",
                column: "SourceId",
                principalTable: "SourceEntity",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FeatureEntity_ProductEntity_ProductId",
                table: "FeatureEntity");

            migrationBuilder.DropForeignKey(
                name: "FK_IndicatorEntity_FeatureEntity_FeatureId",
                table: "IndicatorEntity");

            migrationBuilder.DropForeignKey(
                name: "FK_IndicatorEntity_SourceEntity_SourceId",
                table: "IndicatorEntity");

            migrationBuilder.DropForeignKey(
                name: "FK_ServiceEntity_ProductEntity_ProductId",
                table: "ServiceEntity");

            migrationBuilder.DropForeignKey(
                name: "FK_SourceItemEntity_SourceEntity_SourceId",
                table: "SourceItemEntity");

            migrationBuilder.DropTable(
                name: "SquadFeatureEntity");

            migrationBuilder.AlterColumn<int>(
                name: "SourceId",
                table: "SourceItemEntity",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<int>(
                name: "ProductId",
                table: "ServiceEntity",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<int>(
                name: "SourceId",
                table: "IndicatorEntity",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<int>(
                name: "FeatureId",
                table: "IndicatorEntity",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<int>(
                name: "ProductId",
                table: "FeatureEntity",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddForeignKey(
                name: "FK_FeatureEntity_ProductEntity_ProductId",
                table: "FeatureEntity",
                column: "ProductId",
                principalTable: "ProductEntity",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_IndicatorEntity_FeatureEntity_FeatureId",
                table: "IndicatorEntity",
                column: "FeatureId",
                principalTable: "FeatureEntity",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_IndicatorEntity_SourceEntity_SourceId",
                table: "IndicatorEntity",
                column: "SourceId",
                principalTable: "SourceEntity",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ServiceEntity_ProductEntity_ProductId",
                table: "ServiceEntity",
                column: "ProductId",
                principalTable: "ProductEntity",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_SourceItemEntity_SourceEntity_SourceId",
                table: "SourceItemEntity",
                column: "SourceId",
                principalTable: "SourceEntity",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}

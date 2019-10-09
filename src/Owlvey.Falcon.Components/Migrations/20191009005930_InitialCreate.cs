using Microsoft.EntityFrameworkCore.Migrations;

namespace Owlvey.Falcon.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_IncidentMapEntity_FeatureEntity_FeatureId",
                table: "IncidentMapEntity");

            migrationBuilder.DropForeignKey(
                name: "FK_IndicatorEntity_FeatureEntity_FeatureId",
                table: "IndicatorEntity");

            migrationBuilder.DropForeignKey(
                name: "FK_ServiceMapEntity_FeatureEntity_FeatureId",
                table: "ServiceMapEntity");

            migrationBuilder.DropForeignKey(
                name: "FK_SquadFeatureEntity_FeatureEntity_FeatureId",
                table: "SquadFeatureEntity");

            migrationBuilder.AddForeignKey(
                name: "FK_IncidentMapEntity_FeatureEntity_FeatureId",
                table: "IncidentMapEntity",
                column: "FeatureId",
                principalTable: "FeatureEntity",
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
                name: "FK_ServiceMapEntity_FeatureEntity_FeatureId",
                table: "ServiceMapEntity",
                column: "FeatureId",
                principalTable: "FeatureEntity",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_SquadFeatureEntity_FeatureEntity_FeatureId",
                table: "SquadFeatureEntity",
                column: "FeatureId",
                principalTable: "FeatureEntity",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_IncidentMapEntity_FeatureEntity_FeatureId",
                table: "IncidentMapEntity");

            migrationBuilder.DropForeignKey(
                name: "FK_IndicatorEntity_FeatureEntity_FeatureId",
                table: "IndicatorEntity");

            migrationBuilder.DropForeignKey(
                name: "FK_ServiceMapEntity_FeatureEntity_FeatureId",
                table: "ServiceMapEntity");

            migrationBuilder.DropForeignKey(
                name: "FK_SquadFeatureEntity_FeatureEntity_FeatureId",
                table: "SquadFeatureEntity");

            migrationBuilder.AddForeignKey(
                name: "FK_IncidentMapEntity_FeatureEntity_FeatureId",
                table: "IncidentMapEntity",
                column: "FeatureId",
                principalTable: "FeatureEntity",
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
                name: "FK_ServiceMapEntity_FeatureEntity_FeatureId",
                table: "ServiceMapEntity",
                column: "FeatureId",
                principalTable: "FeatureEntity",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SquadFeatureEntity_FeatureEntity_FeatureId",
                table: "SquadFeatureEntity",
                column: "FeatureId",
                principalTable: "FeatureEntity",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

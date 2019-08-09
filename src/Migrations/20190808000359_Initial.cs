﻿using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Owlvey.Falcon.Migrations
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
                    Name = table.Column<string>(nullable: false),
                    Avatar = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomerEntity", x => x.Id);
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
                    Description = table.Column<string>(nullable: true),
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
                    Description = table.Column<string>(nullable: true),
                    Avatar = table.Column<string>(nullable: true),
                    CustomerId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SquadEntity", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SquadEntity_CustomerEntity_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "CustomerEntity",
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
                    Description = table.Column<string>(nullable: true),
                    SLO = table.Column<float>(nullable: false),
                    Avatar = table.Column<string>(nullable: true),
                    ProductId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ServiceEntity", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ServiceEntity_ProductEntity_ProductId",
                        column: x => x.ProductId,
                        principalTable: "ProductEntity",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SourceEntity",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<string>(nullable: false),
                    ModifiedOn = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<string>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    Tags = table.Column<string>(nullable: true),
                    GoodDefinition = table.Column<string>(nullable: false),
                    TotalDefinition = table.Column<string>(nullable: false),
                    Avatar = table.Column<string>(nullable: true),
                    ProductId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SourceEntity", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SourceEntity_ProductEntity_ProductId",
                        column: x => x.ProductId,
                        principalTable: "ProductEntity",
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
                    Description = table.Column<string>(nullable: false),
                    Avatar = table.Column<string>(nullable: true),
                    ProductId = table.Column<int>(nullable: true),
                    ServiceEntityId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FeatureEntity", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FeatureEntity_ProductEntity_ProductId",
                        column: x => x.ProductId,
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
                name: "SourceItemEntity",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<string>(nullable: false),
                    ModifiedOn = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<string>(nullable: false),
                    SourceId = table.Column<int>(nullable: true),
                    Good = table.Column<int>(nullable: false),
                    Total = table.Column<int>(nullable: false),
                    Start = table.Column<DateTime>(nullable: false),
                    End = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SourceItemEntity", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SourceItemEntity_SourceEntity_SourceId",
                        column: x => x.SourceId,
                        principalTable: "SourceEntity",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "MemberEntity",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<string>(nullable: false),
                    ModifiedOn = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<string>(nullable: false),
                    SquadId = table.Column<int>(nullable: true),
                    UserId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MemberEntity", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MemberEntity_SquadEntity_SquadId",
                        column: x => x.SquadId,
                        principalTable: "SquadEntity",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MemberEntity_UserEntity_UserId",
                        column: x => x.UserId,
                        principalTable: "UserEntity",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "IndicatorEntity",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<string>(nullable: false),
                    ModifiedOn = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<string>(nullable: false),
                    SourceId = table.Column<int>(nullable: true),
                    FeatureId = table.Column<int>(nullable: true),
                    Avatar = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IndicatorEntity", x => x.Id);
                    table.ForeignKey(
                        name: "FK_IndicatorEntity_FeatureEntity_FeatureId",
                        column: x => x.FeatureId,
                        principalTable: "FeatureEntity",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_IndicatorEntity_SourceEntity_SourceId",
                        column: x => x.SourceId,
                        principalTable: "SourceEntity",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ServiceMapEntity",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<string>(nullable: false),
                    ModifiedOn = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<string>(nullable: false),
                    ServiceId = table.Column<int>(nullable: true),
                    FeatureId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ServiceMapEntity", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ServiceMapEntity_FeatureEntity_FeatureId",
                        column: x => x.FeatureId,
                        principalTable: "FeatureEntity",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ServiceMapEntity_ServiceEntity_ServiceId",
                        column: x => x.ServiceId,
                        principalTable: "ServiceEntity",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CustomerEntity_Name",
                table: "CustomerEntity",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_FeatureEntity_ProductId",
                table: "FeatureEntity",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_FeatureEntity_ServiceEntityId",
                table: "FeatureEntity",
                column: "ServiceEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_IndicatorEntity_FeatureId",
                table: "IndicatorEntity",
                column: "FeatureId");

            migrationBuilder.CreateIndex(
                name: "IX_IndicatorEntity_SourceId",
                table: "IndicatorEntity",
                column: "SourceId");

            migrationBuilder.CreateIndex(
                name: "IX_MemberEntity_SquadId",
                table: "MemberEntity",
                column: "SquadId");

            migrationBuilder.CreateIndex(
                name: "IX_MemberEntity_UserId",
                table: "MemberEntity",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductEntity_CustomerId",
                table: "ProductEntity",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_ServiceEntity_ProductId",
                table: "ServiceEntity",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_ServiceMapEntity_FeatureId",
                table: "ServiceMapEntity",
                column: "FeatureId");

            migrationBuilder.CreateIndex(
                name: "IX_ServiceMapEntity_ServiceId",
                table: "ServiceMapEntity",
                column: "ServiceId");

            migrationBuilder.CreateIndex(
                name: "IX_SourceEntity_ProductId",
                table: "SourceEntity",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_SourceItemEntity_SourceId",
                table: "SourceItemEntity",
                column: "SourceId");

            migrationBuilder.CreateIndex(
                name: "IX_SquadEntity_CustomerId",
                table: "SquadEntity",
                column: "CustomerId");

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
                name: "IndicatorEntity");

            migrationBuilder.DropTable(
                name: "MemberEntity");

            migrationBuilder.DropTable(
                name: "ServiceMapEntity");

            migrationBuilder.DropTable(
                name: "SourceItemEntity");

            migrationBuilder.DropTable(
                name: "UserEntity");

            migrationBuilder.DropTable(
                name: "FeatureEntity");

            migrationBuilder.DropTable(
                name: "SourceEntity");

            migrationBuilder.DropTable(
                name: "SquadEntity");

            migrationBuilder.DropTable(
                name: "ServiceEntity");

            migrationBuilder.DropTable(
                name: "ProductEntity");

            migrationBuilder.DropTable(
                name: "CustomerEntity");
        }
    }
}
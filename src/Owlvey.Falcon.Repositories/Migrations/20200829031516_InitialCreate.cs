using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Owlvey.Falcon.Repositories.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AppSettingEntity",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
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
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<string>(nullable: false),
                    ModifiedOn = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<string>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    Avatar = table.Column<string>(nullable: false),
                    Leaders = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomerEntity", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ReliabilityThreatEntity",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<string>(nullable: false),
                    ModifiedOn = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<string>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    Avatar = table.Column<string>(nullable: true),
                    Reference = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    Tags = table.Column<string>(nullable: true),
                    ETTD = table.Column<decimal>(nullable: false),
                    ETTE = table.Column<decimal>(nullable: false),
                    ETTF = table.Column<decimal>(nullable: false),
                    UserImpact = table.Column<decimal>(nullable: false),
                    ETTFail = table.Column<decimal>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReliabilityThreatEntity", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SecurityThreatEntity",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<string>(nullable: false),
                    ModifiedOn = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<string>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    Avatar = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    Reference = table.Column<string>(nullable: true),
                    Tags = table.Column<string>(nullable: true),
                    AgentSkillLevel = table.Column<int>(nullable: false),
                    Motive = table.Column<int>(nullable: false),
                    Opportunity = table.Column<int>(nullable: false),
                    Size = table.Column<int>(nullable: false),
                    EasyDiscovery = table.Column<int>(nullable: false),
                    EasyExploit = table.Column<int>(nullable: false),
                    Awareness = table.Column<int>(nullable: false),
                    IntrusionDetection = table.Column<int>(nullable: false),
                    LossConfidentiality = table.Column<int>(nullable: false),
                    LossIntegrity = table.Column<int>(nullable: false),
                    LossAvailability = table.Column<int>(nullable: false),
                    LossAccountability = table.Column<int>(nullable: false),
                    FinancialDamage = table.Column<int>(nullable: false),
                    ReputationDamage = table.Column<int>(nullable: false),
                    NonCompliance = table.Column<int>(nullable: false),
                    PrivacyViolation = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SecurityThreatEntity", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserEntity",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<string>(nullable: false),
                    ModifiedOn = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<string>(nullable: false),
                    Email = table.Column<string>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    Avatar = table.Column<string>(nullable: false),
                    SlackMember = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserEntity", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ProductEntity",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<string>(nullable: false),
                    ModifiedOn = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<string>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    Avatar = table.Column<string>(nullable: false),
                    Description = table.Column<string>(nullable: false),
                    Leaders = table.Column<string>(nullable: true),
                    CustomerId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductEntity", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductEntity_CustomerEntity_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "CustomerEntity",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SquadEntity",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<string>(nullable: false),
                    ModifiedOn = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<string>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    Description = table.Column<string>(nullable: true),
                    Avatar = table.Column<string>(nullable: true),
                    Leaders = table.Column<string>(nullable: true),
                    CustomerId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SquadEntity", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SquadEntity_CustomerEntity_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "CustomerEntity",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AnchorEntity",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<string>(nullable: false),
                    ModifiedOn = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<string>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    Target = table.Column<DateTime>(nullable: false),
                    ProductId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AnchorEntity", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AnchorEntity_ProductEntity_ProductId",
                        column: x => x.ProductId,
                        principalTable: "ProductEntity",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FeatureEntity",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<string>(nullable: false),
                    ModifiedOn = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<string>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    Description = table.Column<string>(nullable: false),
                    Avatar = table.Column<string>(nullable: false),
                    ProductId = table.Column<int>(nullable: false),
                    JourneyMapId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FeatureEntity", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FeatureEntity_ProductEntity_ProductId",
                        column: x => x.ProductId,
                        principalTable: "ProductEntity",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "IncidentEntity",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<string>(nullable: false),
                    ModifiedOn = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<string>(nullable: false),
                    Key = table.Column<string>(nullable: false),
                    Title = table.Column<string>(nullable: false),
                    Url = table.Column<string>(nullable: false),
                    Tags = table.Column<string>(nullable: true),
                    TTD = table.Column<int>(nullable: false),
                    TTE = table.Column<int>(nullable: false),
                    TTF = table.Column<int>(nullable: false),
                    End = table.Column<DateTime>(nullable: false),
                    Affected = table.Column<int>(nullable: false),
                    ProductId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IncidentEntity", x => x.Id);
                    table.ForeignKey(
                        name: "FK_IncidentEntity_ProductEntity_ProductId",
                        column: x => x.ProductId,
                        principalTable: "ProductEntity",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "JourneyEntity",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<string>(nullable: false),
                    ModifiedOn = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<string>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    Description = table.Column<string>(nullable: true),
                    AvailabilitySlo = table.Column<decimal>(type: "decimal(5,3)", nullable: false),
                    ExperienceSlo = table.Column<decimal>(type: "decimal(5,3)", nullable: false),
                    LatencySlo = table.Column<decimal>(type: "decimal(12,3)", nullable: false),
                    AvailabilitySla = table.Column<decimal>(nullable: false),
                    LatencySla = table.Column<decimal>(nullable: false),
                    Avatar = table.Column<string>(nullable: false),
                    Leaders = table.Column<string>(nullable: true),
                    Group = table.Column<string>(nullable: false),
                    ProductId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JourneyEntity", x => x.Id);
                    table.ForeignKey(
                        name: "FK_JourneyEntity_ProductEntity_ProductId",
                        column: x => x.ProductId,
                        principalTable: "ProductEntity",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SourceEntity",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<string>(nullable: false),
                    ModifiedOn = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<string>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    Tags = table.Column<string>(nullable: true),
                    GoodDefinitionAvailability = table.Column<string>(nullable: false),
                    TotalDefinitionAvailability = table.Column<string>(nullable: false),
                    GoodDefinitionLatency = table.Column<string>(nullable: false),
                    TotalDefinitionLatency = table.Column<string>(nullable: false),
                    GoodDefinitionExperience = table.Column<string>(nullable: false),
                    TotalDefinitionExperience = table.Column<string>(nullable: false),
                    Avatar = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    Percentile = table.Column<decimal>(nullable: false),
                    ProductId = table.Column<int>(nullable: false),
                    Latency = table.Column<decimal>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SourceEntity", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SourceEntity_ProductEntity_ProductId",
                        column: x => x.ProductId,
                        principalTable: "ProductEntity",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MemberEntity",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<string>(nullable: false),
                    ModifiedOn = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<string>(nullable: false),
                    SquadId = table.Column<int>(nullable: false),
                    UserId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MemberEntity", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MemberEntity_SquadEntity_SquadId",
                        column: x => x.SquadId,
                        principalTable: "SquadEntity",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MemberEntity_UserEntity_UserId",
                        column: x => x.UserId,
                        principalTable: "UserEntity",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SquadFeatureEntity",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
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
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "IncidentMapEntity",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<string>(nullable: false),
                    ModifiedOn = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<string>(nullable: false),
                    FeatureId = table.Column<int>(nullable: false),
                    IncidentId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IncidentMapEntity", x => x.Id);
                    table.ForeignKey(
                        name: "FK_IncidentMapEntity_FeatureEntity_FeatureId",
                        column: x => x.FeatureId,
                        principalTable: "FeatureEntity",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_IncidentMapEntity_IncidentEntity_IncidentId",
                        column: x => x.IncidentId,
                        principalTable: "IncidentEntity",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "JourneyMapEntity",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<string>(nullable: false),
                    ModifiedOn = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<string>(nullable: false),
                    JourneyId = table.Column<int>(nullable: false),
                    FeatureId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JourneyMapEntity", x => x.Id);
                    table.ForeignKey(
                        name: "FK_JourneyMapEntity_FeatureEntity_FeatureId",
                        column: x => x.FeatureId,
                        principalTable: "FeatureEntity",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_JourneyMapEntity_JourneyEntity_JourneyId",
                        column: x => x.JourneyId,
                        principalTable: "JourneyEntity",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "IndicatorEntity",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<string>(nullable: false),
                    ModifiedOn = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<string>(nullable: false),
                    SourceId = table.Column<int>(nullable: false),
                    FeatureId = table.Column<int>(nullable: false),
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
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_IndicatorEntity_SourceEntity_SourceId",
                        column: x => x.SourceId,
                        principalTable: "SourceEntity",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ReliabilityRiskEntity",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<string>(nullable: false),
                    ModifiedOn = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<string>(nullable: false),
                    SourceId = table.Column<int>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    Avatar = table.Column<string>(nullable: true),
                    Reference = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    Tags = table.Column<string>(nullable: true),
                    ETTD = table.Column<decimal>(nullable: false),
                    ETTE = table.Column<decimal>(nullable: false),
                    ETTF = table.Column<decimal>(nullable: false),
                    UserImpact = table.Column<decimal>(nullable: false),
                    ETTFail = table.Column<decimal>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReliabilityRiskEntity", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ReliabilityRiskEntity_SourceEntity_SourceId",
                        column: x => x.SourceId,
                        principalTable: "SourceEntity",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SecurityRiskEntity",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<string>(nullable: false),
                    ModifiedOn = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<string>(nullable: false),
                    SourceId = table.Column<int>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    Avatar = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    Reference = table.Column<string>(nullable: true),
                    Tags = table.Column<string>(nullable: true),
                    AgentSkillLevel = table.Column<int>(nullable: false),
                    Motive = table.Column<int>(nullable: false),
                    Opportunity = table.Column<int>(nullable: false),
                    Size = table.Column<int>(nullable: false),
                    EasyDiscovery = table.Column<int>(nullable: false),
                    EasyExploit = table.Column<int>(nullable: false),
                    Awareness = table.Column<int>(nullable: false),
                    IntrusionDetection = table.Column<int>(nullable: false),
                    LossConfidentiality = table.Column<int>(nullable: false),
                    LossIntegrity = table.Column<int>(nullable: false),
                    LossAvailability = table.Column<int>(nullable: false),
                    LossAccountability = table.Column<int>(nullable: false),
                    FinancialDamage = table.Column<int>(nullable: false),
                    ReputationDamage = table.Column<int>(nullable: false),
                    NonCompliance = table.Column<int>(nullable: false),
                    PrivacyViolation = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SecurityRiskEntity", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SecurityRiskEntity_SourceEntity_SourceId",
                        column: x => x.SourceId,
                        principalTable: "SourceEntity",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SourceItemEntity",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<string>(nullable: false),
                    ModifiedOn = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<string>(nullable: false),
                    SourceId = table.Column<int>(nullable: false),
                    Good = table.Column<int>(nullable: true),
                    Group = table.Column<int>(nullable: false),
                    Total = table.Column<int>(nullable: true),
                    Target = table.Column<DateTime>(nullable: false),
                    Measure = table.Column<decimal>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SourceItemEntity", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SourceItemEntity_SourceEntity_SourceId",
                        column: x => x.SourceId,
                        principalTable: "SourceEntity",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ClueEntity",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<string>(nullable: false),
                    ModifiedOn = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<string>(nullable: false),
                    SourceItemId = table.Column<int>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    Value = table.Column<decimal>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClueEntity", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ClueEntity_SourceItemEntity_SourceItemId",
                        column: x => x.SourceItemId,
                        principalTable: "SourceItemEntity",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AnchorEntity_ProductId",
                table: "AnchorEntity",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_ClueEntity_SourceItemId",
                table: "ClueEntity",
                column: "SourceItemId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerEntity_Name",
                table: "CustomerEntity",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_FeatureEntity_ProductId_Name",
                table: "FeatureEntity",
                columns: new[] { "ProductId", "Name" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_IncidentEntity_ProductId",
                table: "IncidentEntity",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_IncidentMapEntity_FeatureId",
                table: "IncidentMapEntity",
                column: "FeatureId");

            migrationBuilder.CreateIndex(
                name: "IX_IncidentMapEntity_IncidentId",
                table: "IncidentMapEntity",
                column: "IncidentId");

            migrationBuilder.CreateIndex(
                name: "IX_IndicatorEntity_SourceId",
                table: "IndicatorEntity",
                column: "SourceId");

            migrationBuilder.CreateIndex(
                name: "IX_IndicatorEntity_FeatureId_SourceId",
                table: "IndicatorEntity",
                columns: new[] { "FeatureId", "SourceId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_JourneyEntity_ProductId_Name",
                table: "JourneyEntity",
                columns: new[] { "ProductId", "Name" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_JourneyMapEntity_FeatureId",
                table: "JourneyMapEntity",
                column: "FeatureId");

            migrationBuilder.CreateIndex(
                name: "IX_JourneyMapEntity_JourneyId_FeatureId",
                table: "JourneyMapEntity",
                columns: new[] { "JourneyId", "FeatureId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_MemberEntity_SquadId",
                table: "MemberEntity",
                column: "SquadId");

            migrationBuilder.CreateIndex(
                name: "IX_MemberEntity_UserId",
                table: "MemberEntity",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductEntity_CustomerId_Name",
                table: "ProductEntity",
                columns: new[] { "CustomerId", "Name" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ReliabilityRiskEntity_SourceId",
                table: "ReliabilityRiskEntity",
                column: "SourceId");

            migrationBuilder.CreateIndex(
                name: "IX_SecurityRiskEntity_SourceId",
                table: "SecurityRiskEntity",
                column: "SourceId");

            migrationBuilder.CreateIndex(
                name: "IX_SourceEntity_ProductId_Name",
                table: "SourceEntity",
                columns: new[] { "ProductId", "Name" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SourceItemEntity_SourceId",
                table: "SourceItemEntity",
                column: "SourceId");

            migrationBuilder.CreateIndex(
                name: "IX_SourceItemEntity_Target",
                table: "SourceItemEntity",
                column: "Target");

            migrationBuilder.CreateIndex(
                name: "IX_SquadEntity_CustomerId_Name",
                table: "SquadEntity",
                columns: new[] { "CustomerId", "Name" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SquadFeatureEntity_FeatureId",
                table: "SquadFeatureEntity",
                column: "FeatureId");

            migrationBuilder.CreateIndex(
                name: "IX_SquadFeatureEntity_SquadId",
                table: "SquadFeatureEntity",
                column: "SquadId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AnchorEntity");

            migrationBuilder.DropTable(
                name: "AppSettingEntity");

            migrationBuilder.DropTable(
                name: "ClueEntity");

            migrationBuilder.DropTable(
                name: "IncidentMapEntity");

            migrationBuilder.DropTable(
                name: "IndicatorEntity");

            migrationBuilder.DropTable(
                name: "JourneyMapEntity");

            migrationBuilder.DropTable(
                name: "MemberEntity");

            migrationBuilder.DropTable(
                name: "ReliabilityRiskEntity");

            migrationBuilder.DropTable(
                name: "ReliabilityThreatEntity");

            migrationBuilder.DropTable(
                name: "SecurityRiskEntity");

            migrationBuilder.DropTable(
                name: "SecurityThreatEntity");

            migrationBuilder.DropTable(
                name: "SquadFeatureEntity");

            migrationBuilder.DropTable(
                name: "SourceItemEntity");

            migrationBuilder.DropTable(
                name: "IncidentEntity");

            migrationBuilder.DropTable(
                name: "JourneyEntity");

            migrationBuilder.DropTable(
                name: "UserEntity");

            migrationBuilder.DropTable(
                name: "FeatureEntity");

            migrationBuilder.DropTable(
                name: "SquadEntity");

            migrationBuilder.DropTable(
                name: "SourceEntity");

            migrationBuilder.DropTable(
                name: "ProductEntity");

            migrationBuilder.DropTable(
                name: "CustomerEntity");
        }
    }
}

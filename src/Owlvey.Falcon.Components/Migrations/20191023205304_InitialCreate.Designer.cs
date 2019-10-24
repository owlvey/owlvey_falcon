﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Owlvey.Falcon.Repositories;

namespace Owlvey.Falcon.Migrations
{
    [DbContext(typeof(FalconDbContext))]
    [Migration("20191023205304_InitialCreate")]
    partial class InitialCreate
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.2.6-servicing-10079");

            modelBuilder.Entity("Owlvey.Falcon.Core.Entities.AnchorEntity", b =>
                {
                    b.Property<int?>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("CreatedBy")
                        .IsRequired();

                    b.Property<DateTime?>("CreatedOn")
                        .IsRequired();

                    b.Property<bool>("Deleted");

                    b.Property<string>("ModifiedBy")
                        .IsRequired();

                    b.Property<DateTime?>("ModifiedOn")
                        .IsRequired();

                    b.Property<string>("Name")
                        .IsRequired();

                    b.Property<int>("ProductId");

                    b.Property<DateTime>("Target");

                    b.HasKey("Id");

                    b.HasIndex("ProductId");

                    b.ToTable("AnchorEntity");
                });

            modelBuilder.Entity("Owlvey.Falcon.Core.Entities.AppSettingEntity", b =>
                {
                    b.Property<int?>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("CreatedBy")
                        .IsRequired();

                    b.Property<DateTime?>("CreatedOn")
                        .IsRequired();

                    b.Property<bool>("Deleted");

                    b.Property<bool>("IsReadOnly");

                    b.Property<string>("Key")
                        .IsRequired();

                    b.Property<string>("ModifiedBy")
                        .IsRequired();

                    b.Property<DateTime?>("ModifiedOn")
                        .IsRequired();

                    b.Property<string>("Value")
                        .IsRequired();

                    b.HasKey("Id");

                    b.ToTable("AppSettingEntity");
                });

            modelBuilder.Entity("Owlvey.Falcon.Core.Entities.CustomerEntity", b =>
                {
                    b.Property<int?>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Avatar")
                        .IsRequired();

                    b.Property<string>("CreatedBy")
                        .IsRequired();

                    b.Property<DateTime?>("CreatedOn")
                        .IsRequired();

                    b.Property<bool>("Deleted");

                    b.Property<string>("ModifiedBy")
                        .IsRequired();

                    b.Property<DateTime?>("ModifiedOn")
                        .IsRequired();

                    b.Property<string>("Name")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("Name")
                        .IsUnique();

                    b.ToTable("CustomerEntity");
                });

            modelBuilder.Entity("Owlvey.Falcon.Core.Entities.FeatureEntity", b =>
                {
                    b.Property<int?>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Avatar")
                        .IsRequired();

                    b.Property<string>("CreatedBy")
                        .IsRequired();

                    b.Property<DateTime?>("CreatedOn")
                        .IsRequired();

                    b.Property<bool>("Deleted");

                    b.Property<string>("Description")
                        .IsRequired();

                    b.Property<string>("ModifiedBy")
                        .IsRequired();

                    b.Property<DateTime?>("ModifiedOn")
                        .IsRequired();

                    b.Property<string>("Name")
                        .IsRequired();

                    b.Property<int>("ProductId");

                    b.Property<int>("ServiceMapId");

                    b.HasKey("Id");

                    b.HasIndex("ProductId");

                    b.ToTable("FeatureEntity");
                });

            modelBuilder.Entity("Owlvey.Falcon.Core.Entities.IncidentEntity", b =>
                {
                    b.Property<int?>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("Affected");

                    b.Property<string>("CreatedBy")
                        .IsRequired();

                    b.Property<DateTime?>("CreatedOn")
                        .IsRequired();

                    b.Property<bool>("Deleted");

                    b.Property<DateTime>("End");

                    b.Property<string>("Key")
                        .IsRequired();

                    b.Property<string>("ModifiedBy")
                        .IsRequired();

                    b.Property<DateTime?>("ModifiedOn")
                        .IsRequired();

                    b.Property<int>("ProductId");

                    b.Property<int>("TTD");

                    b.Property<int>("TTE");

                    b.Property<int>("TTF");

                    b.Property<string>("Tags");

                    b.Property<string>("Title")
                        .IsRequired();

                    b.Property<string>("Url")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("ProductId");

                    b.ToTable("IncidentEntity");
                });

            modelBuilder.Entity("Owlvey.Falcon.Core.Entities.IncidentMapEntity", b =>
                {
                    b.Property<int?>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("CreatedBy")
                        .IsRequired();

                    b.Property<DateTime?>("CreatedOn")
                        .IsRequired();

                    b.Property<bool>("Deleted");

                    b.Property<int>("FeatureId");

                    b.Property<int>("IncidentId");

                    b.Property<string>("ModifiedBy")
                        .IsRequired();

                    b.Property<DateTime?>("ModifiedOn")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("FeatureId");

                    b.HasIndex("IncidentId");

                    b.ToTable("IncidentMapEntity");
                });

            modelBuilder.Entity("Owlvey.Falcon.Core.Entities.IndicatorEntity", b =>
                {
                    b.Property<int?>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Avatar");

                    b.Property<string>("CreatedBy")
                        .IsRequired();

                    b.Property<DateTime?>("CreatedOn")
                        .IsRequired();

                    b.Property<bool>("Deleted");

                    b.Property<string>("Description");

                    b.Property<int>("FeatureId");

                    b.Property<string>("ModifiedBy")
                        .IsRequired();

                    b.Property<DateTime?>("ModifiedOn")
                        .IsRequired();

                    b.Property<int>("SourceId");

                    b.HasKey("Id");

                    b.HasIndex("FeatureId");

                    b.HasIndex("SourceId");

                    b.ToTable("IndicatorEntity");
                });

            modelBuilder.Entity("Owlvey.Falcon.Core.Entities.MemberEntity", b =>
                {
                    b.Property<int?>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("CreatedBy")
                        .IsRequired();

                    b.Property<DateTime?>("CreatedOn")
                        .IsRequired();

                    b.Property<bool>("Deleted");

                    b.Property<string>("ModifiedBy")
                        .IsRequired();

                    b.Property<DateTime?>("ModifiedOn")
                        .IsRequired();

                    b.Property<int>("SquadId");

                    b.Property<int>("UserId");

                    b.HasKey("Id");

                    b.HasIndex("SquadId");

                    b.HasIndex("UserId");

                    b.ToTable("MemberEntity");
                });

            modelBuilder.Entity("Owlvey.Falcon.Core.Entities.ProductEntity", b =>
                {
                    b.Property<int?>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Avatar")
                        .IsRequired();

                    b.Property<string>("CreatedBy")
                        .IsRequired();

                    b.Property<DateTime?>("CreatedOn")
                        .IsRequired();

                    b.Property<int>("CustomerId");

                    b.Property<bool>("Deleted");

                    b.Property<string>("Description")
                        .IsRequired();

                    b.Property<string>("ModifiedBy")
                        .IsRequired();

                    b.Property<DateTime?>("ModifiedOn")
                        .IsRequired();

                    b.Property<string>("Name")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("CustomerId");

                    b.ToTable("ProductEntity");
                });

            modelBuilder.Entity("Owlvey.Falcon.Core.Entities.ServiceEntity", b =>
                {
                    b.Property<int?>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Avatar")
                        .IsRequired();

                    b.Property<string>("CreatedBy")
                        .IsRequired();

                    b.Property<DateTime?>("CreatedOn")
                        .IsRequired();

                    b.Property<bool>("Deleted");

                    b.Property<string>("Description");

                    b.Property<string>("ModifiedBy")
                        .IsRequired();

                    b.Property<DateTime?>("ModifiedOn")
                        .IsRequired();

                    b.Property<string>("Name")
                        .IsRequired();

                    b.Property<string>("Owner")
                        .IsRequired();

                    b.Property<int>("ProductId");

                    b.Property<decimal>("Slo");

                    b.HasKey("Id");

                    b.HasIndex("ProductId");

                    b.ToTable("ServiceEntity");
                });

            modelBuilder.Entity("Owlvey.Falcon.Core.Entities.ServiceMapEntity", b =>
                {
                    b.Property<int?>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("CreatedBy")
                        .IsRequired();

                    b.Property<DateTime?>("CreatedOn")
                        .IsRequired();

                    b.Property<bool>("Deleted");

                    b.Property<int>("FeatureId");

                    b.Property<string>("ModifiedBy")
                        .IsRequired();

                    b.Property<DateTime?>("ModifiedOn")
                        .IsRequired();

                    b.Property<int>("ServiceId");

                    b.HasKey("Id");

                    b.HasIndex("FeatureId");

                    b.HasIndex("ServiceId");

                    b.ToTable("ServiceMapEntity");
                });

            modelBuilder.Entity("Owlvey.Falcon.Core.Entities.SourceEntity", b =>
                {
                    b.Property<int?>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Avatar");

                    b.Property<string>("CreatedBy")
                        .IsRequired();

                    b.Property<DateTime?>("CreatedOn")
                        .IsRequired();

                    b.Property<bool>("Deleted");

                    b.Property<string>("Description");

                    b.Property<string>("GoodDefinition")
                        .IsRequired();

                    b.Property<int>("Kind");

                    b.Property<string>("ModifiedBy")
                        .IsRequired();

                    b.Property<DateTime?>("ModifiedOn")
                        .IsRequired();

                    b.Property<string>("Name")
                        .IsRequired();

                    b.Property<int>("ProductId");

                    b.Property<string>("Tags");

                    b.Property<string>("TotalDefinition")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("ProductId");

                    b.ToTable("SourceEntity");
                });

            modelBuilder.Entity("Owlvey.Falcon.Core.Entities.SourceItemEntity", b =>
                {
                    b.Property<int?>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("CreatedBy")
                        .IsRequired();

                    b.Property<DateTime?>("CreatedOn")
                        .IsRequired();

                    b.Property<bool>("Deleted");

                    b.Property<DateTime>("End");

                    b.Property<int>("Good");

                    b.Property<string>("ModifiedBy")
                        .IsRequired();

                    b.Property<DateTime?>("ModifiedOn")
                        .IsRequired();

                    b.Property<int>("SourceId");

                    b.Property<DateTime>("Start");

                    b.Property<int>("Total");

                    b.HasKey("Id");

                    b.HasIndex("End");

                    b.HasIndex("SourceId");

                    b.HasIndex("Start");

                    b.ToTable("SourceItemEntity");
                });

            modelBuilder.Entity("Owlvey.Falcon.Core.Entities.SquadEntity", b =>
                {
                    b.Property<int?>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Avatar");

                    b.Property<string>("CreatedBy")
                        .IsRequired();

                    b.Property<DateTime?>("CreatedOn")
                        .IsRequired();

                    b.Property<int>("CustomerId");

                    b.Property<bool>("Deleted");

                    b.Property<string>("Description");

                    b.Property<string>("ModifiedBy")
                        .IsRequired();

                    b.Property<DateTime?>("ModifiedOn")
                        .IsRequired();

                    b.Property<string>("Name")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("CustomerId");

                    b.ToTable("SquadEntity");
                });

            modelBuilder.Entity("Owlvey.Falcon.Core.Entities.SquadFeatureEntity", b =>
                {
                    b.Property<int?>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("CreatedBy")
                        .IsRequired();

                    b.Property<DateTime?>("CreatedOn")
                        .IsRequired();

                    b.Property<bool>("Deleted");

                    b.Property<int>("FeatureId");

                    b.Property<string>("ModifiedBy")
                        .IsRequired();

                    b.Property<DateTime?>("ModifiedOn")
                        .IsRequired();

                    b.Property<int>("SquadId");

                    b.HasKey("Id");

                    b.HasIndex("FeatureId");

                    b.HasIndex("SquadId");

                    b.ToTable("SquadFeatureEntity");
                });

            modelBuilder.Entity("Owlvey.Falcon.Core.Entities.UserEntity", b =>
                {
                    b.Property<int?>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Avatar")
                        .IsRequired();

                    b.Property<string>("CreatedBy")
                        .IsRequired();

                    b.Property<DateTime?>("CreatedOn")
                        .IsRequired();

                    b.Property<bool>("Deleted");

                    b.Property<string>("Email")
                        .IsRequired();

                    b.Property<string>("ModifiedBy")
                        .IsRequired();

                    b.Property<DateTime?>("ModifiedOn")
                        .IsRequired();

                    b.Property<string>("Name")
                        .IsRequired();

                    b.HasKey("Id");

                    b.ToTable("UserEntity");
                });

            modelBuilder.Entity("Owlvey.Falcon.Core.Entities.AnchorEntity", b =>
                {
                    b.HasOne("Owlvey.Falcon.Core.Entities.ProductEntity", "Product")
                        .WithMany("Anchors")
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Owlvey.Falcon.Core.Entities.FeatureEntity", b =>
                {
                    b.HasOne("Owlvey.Falcon.Core.Entities.ProductEntity", "Product")
                        .WithMany("Features")
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Owlvey.Falcon.Core.Entities.IncidentEntity", b =>
                {
                    b.HasOne("Owlvey.Falcon.Core.Entities.ProductEntity", "Product")
                        .WithMany("Incidents")
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Owlvey.Falcon.Core.Entities.IncidentMapEntity", b =>
                {
                    b.HasOne("Owlvey.Falcon.Core.Entities.FeatureEntity", "Feature")
                        .WithMany("IncidentMap")
                        .HasForeignKey("FeatureId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("Owlvey.Falcon.Core.Entities.IncidentEntity", "Incident")
                        .WithMany("FeatureMaps")
                        .HasForeignKey("IncidentId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Owlvey.Falcon.Core.Entities.IndicatorEntity", b =>
                {
                    b.HasOne("Owlvey.Falcon.Core.Entities.FeatureEntity", "Feature")
                        .WithMany("Indicators")
                        .HasForeignKey("FeatureId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Owlvey.Falcon.Core.Entities.SourceEntity", "Source")
                        .WithMany("Indicators")
                        .HasForeignKey("SourceId")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("Owlvey.Falcon.Core.Entities.MemberEntity", b =>
                {
                    b.HasOne("Owlvey.Falcon.Core.Entities.SquadEntity", "Squad")
                        .WithMany("Members")
                        .HasForeignKey("SquadId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Owlvey.Falcon.Core.Entities.UserEntity", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Owlvey.Falcon.Core.Entities.ProductEntity", b =>
                {
                    b.HasOne("Owlvey.Falcon.Core.Entities.CustomerEntity", "Customer")
                        .WithMany("Products")
                        .HasForeignKey("CustomerId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Owlvey.Falcon.Core.Entities.ServiceEntity", b =>
                {
                    b.HasOne("Owlvey.Falcon.Core.Entities.ProductEntity", "Product")
                        .WithMany("Services")
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Owlvey.Falcon.Core.Entities.ServiceMapEntity", b =>
                {
                    b.HasOne("Owlvey.Falcon.Core.Entities.FeatureEntity", "Feature")
                        .WithMany("ServiceMaps")
                        .HasForeignKey("FeatureId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("Owlvey.Falcon.Core.Entities.ServiceEntity", "Service")
                        .WithMany("FeatureMap")
                        .HasForeignKey("ServiceId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Owlvey.Falcon.Core.Entities.SourceEntity", b =>
                {
                    b.HasOne("Owlvey.Falcon.Core.Entities.ProductEntity", "Product")
                        .WithMany("Sources")
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Owlvey.Falcon.Core.Entities.SourceItemEntity", b =>
                {
                    b.HasOne("Owlvey.Falcon.Core.Entities.SourceEntity", "Source")
                        .WithMany("SourceItems")
                        .HasForeignKey("SourceId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Owlvey.Falcon.Core.Entities.SquadEntity", b =>
                {
                    b.HasOne("Owlvey.Falcon.Core.Entities.CustomerEntity", "Customer")
                        .WithMany("Squads")
                        .HasForeignKey("CustomerId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Owlvey.Falcon.Core.Entities.SquadFeatureEntity", b =>
                {
                    b.HasOne("Owlvey.Falcon.Core.Entities.FeatureEntity", "Feature")
                        .WithMany("Squads")
                        .HasForeignKey("FeatureId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("Owlvey.Falcon.Core.Entities.SquadEntity", "Squad")
                        .WithMany("FeatureMaps")
                        .HasForeignKey("SquadId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
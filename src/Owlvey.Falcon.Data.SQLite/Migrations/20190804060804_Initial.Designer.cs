﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Owlvey.Falcon.Data.SQLite.Context;

namespace Owlvey.Falcon.Data.SQLite.Migrations
{
    [DbContext(typeof(FalconDbContext))]
    [Migration("20190804060804_Initial")]
    partial class Initial
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.2.6-servicing-10079");

            modelBuilder.Entity("Owlvey.Falcon.Core.Entities.AppSettingEntity", b =>
                {
                    b.Property<int?>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("CreatedBy")
                        .IsRequired();

                    b.Property<DateTime>("CreatedOn");

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

                    b.Property<string>("CreatedBy")
                        .IsRequired();

                    b.Property<DateTime>("CreatedOn");

                    b.Property<string>("ModifiedBy")
                        .IsRequired();

                    b.Property<DateTime?>("ModifiedOn")
                        .IsRequired();

                    b.Property<string>("Name")
                        .IsRequired();

                    b.HasKey("Id");

                    b.ToTable("CustomerEntity");
                });

            modelBuilder.Entity("Owlvey.Falcon.Core.Entities.FeatureEntity", b =>
                {
                    b.Property<int?>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Avatar");

                    b.Property<string>("CreatedBy")
                        .IsRequired();

                    b.Property<DateTime>("CreatedOn");

                    b.Property<string>("ModifiedBy")
                        .IsRequired();

                    b.Property<DateTime?>("ModifiedOn")
                        .IsRequired();

                    b.Property<string>("Name")
                        .IsRequired();

                    b.Property<int?>("ProductEntityId");

                    b.Property<int?>("ServiceEntityId");

                    b.HasKey("Id");

                    b.HasIndex("ProductEntityId");

                    b.HasIndex("ServiceEntityId");

                    b.ToTable("FeatureEntity");
                });

            modelBuilder.Entity("Owlvey.Falcon.Core.Entities.JournalEntity", b =>
                {
                    b.Property<int?>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Avatar");

                    b.Property<string>("CreatedBy")
                        .IsRequired();

                    b.Property<DateTime>("CreatedOn");

                    b.Property<int?>("FeatureEntityId");

                    b.Property<string>("GoodDefinition")
                        .IsRequired();

                    b.Property<string>("ModifiedBy")
                        .IsRequired();

                    b.Property<DateTime?>("ModifiedOn")
                        .IsRequired();

                    b.Property<string>("TotalDefinition")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("FeatureEntityId");

                    b.ToTable("JournalEntity");
                });

            modelBuilder.Entity("Owlvey.Falcon.Core.Entities.JournalItemEntity", b =>
                {
                    b.Property<int?>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("CreatedBy")
                        .IsRequired();

                    b.Property<DateTime>("CreatedOn");

                    b.Property<int>("Good");

                    b.Property<int?>("JournalId");

                    b.Property<string>("ModifiedBy")
                        .IsRequired();

                    b.Property<DateTime?>("ModifiedOn")
                        .IsRequired();

                    b.Property<int>("Total");

                    b.HasKey("Id");

                    b.HasIndex("JournalId");

                    b.ToTable("JournalItemEntity");
                });

            modelBuilder.Entity("Owlvey.Falcon.Core.Entities.ProductEntity", b =>
                {
                    b.Property<int?>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("CreatedBy")
                        .IsRequired();

                    b.Property<DateTime>("CreatedOn");

                    b.Property<int?>("CustomerId");

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

                    b.Property<string>("Avatar");

                    b.Property<string>("CreatedBy")
                        .IsRequired();

                    b.Property<DateTime>("CreatedOn");

                    b.Property<string>("Description")
                        .IsRequired();

                    b.Property<string>("ModifiedBy")
                        .IsRequired();

                    b.Property<DateTime?>("ModifiedOn")
                        .IsRequired();

                    b.Property<string>("Name")
                        .IsRequired();

                    b.Property<int?>("ProductEntityId");

                    b.Property<float>("SLO");

                    b.HasKey("Id");

                    b.HasIndex("ProductEntityId");

                    b.ToTable("ServiceEntity");
                });

            modelBuilder.Entity("Owlvey.Falcon.Core.Entities.SquadEntity", b =>
                {
                    b.Property<int?>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Avatar");

                    b.Property<string>("CreatedBy")
                        .IsRequired();

                    b.Property<DateTime>("CreatedOn");

                    b.Property<string>("Description")
                        .IsRequired();

                    b.Property<string>("ModifiedBy")
                        .IsRequired();

                    b.Property<DateTime?>("ModifiedOn")
                        .IsRequired();

                    b.Property<string>("Name")
                        .IsRequired();

                    b.HasKey("Id");

                    b.ToTable("SquadEntity");
                });

            modelBuilder.Entity("Owlvey.Falcon.Core.Entities.UserEntity", b =>
                {
                    b.Property<int?>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("CreatedBy")
                        .IsRequired();

                    b.Property<DateTime>("CreatedOn");

                    b.Property<string>("Email")
                        .IsRequired();

                    b.Property<string>("ModifiedBy")
                        .IsRequired();

                    b.Property<DateTime?>("ModifiedOn")
                        .IsRequired();

                    b.Property<int?>("SquadEntityId");

                    b.HasKey("Id");

                    b.HasIndex("SquadEntityId");

                    b.ToTable("UserEntity");
                });

            modelBuilder.Entity("Owlvey.Falcon.Core.Entities.FeatureEntity", b =>
                {
                    b.HasOne("Owlvey.Falcon.Core.Entities.ProductEntity")
                        .WithMany("Features")
                        .HasForeignKey("ProductEntityId");

                    b.HasOne("Owlvey.Falcon.Core.Entities.ServiceEntity")
                        .WithMany("Features")
                        .HasForeignKey("ServiceEntityId");
                });

            modelBuilder.Entity("Owlvey.Falcon.Core.Entities.JournalEntity", b =>
                {
                    b.HasOne("Owlvey.Falcon.Core.Entities.FeatureEntity")
                        .WithMany("JournalEntities")
                        .HasForeignKey("FeatureEntityId");
                });

            modelBuilder.Entity("Owlvey.Falcon.Core.Entities.JournalItemEntity", b =>
                {
                    b.HasOne("Owlvey.Falcon.Core.Entities.JournalEntity", "Journal")
                        .WithMany("JournalItems")
                        .HasForeignKey("JournalId");
                });

            modelBuilder.Entity("Owlvey.Falcon.Core.Entities.ProductEntity", b =>
                {
                    b.HasOne("Owlvey.Falcon.Core.Entities.CustomerEntity", "Customer")
                        .WithMany()
                        .HasForeignKey("CustomerId");
                });

            modelBuilder.Entity("Owlvey.Falcon.Core.Entities.ServiceEntity", b =>
                {
                    b.HasOne("Owlvey.Falcon.Core.Entities.ProductEntity")
                        .WithMany("Services")
                        .HasForeignKey("ProductEntityId");
                });

            modelBuilder.Entity("Owlvey.Falcon.Core.Entities.UserEntity", b =>
                {
                    b.HasOne("Owlvey.Falcon.Core.Entities.SquadEntity")
                        .WithMany("Users")
                        .HasForeignKey("SquadEntityId");
                });
#pragma warning restore 612, 618
        }
    }
}

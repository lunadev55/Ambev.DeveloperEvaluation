﻿// <auto-generated />
using System;
using Ambev.DeveloperEvaluation.ORM;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Ambev.DeveloperEvaluation.ORM.Migrations
{
    [DbContext(typeof(DefaultContext))]
    [Migration("20250528011802_AddSalesEntities")]
    partial class AddSalesEntities
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.10")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Ambev.DeveloperEvaluation.Domain.Entities.Sale", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateTime>("Date")
                        .HasColumnType("timestamp with time zone");

                    b.Property<bool>("IsCancelled")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("boolean")
                        .HasDefaultValue(false)
                        .HasColumnName("IsCancelled");

                    b.Property<string>("SaleNumber")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Sales", (string)null);
                });

            modelBuilder.Entity("Ambev.DeveloperEvaluation.Domain.Entities.User", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasDefaultValueSql("gen_random_uuid()");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.Property<string>("Phone")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("character varying(20)");

                    b.Property<string>("Role")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("character varying(20)");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("character varying(20)");

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)");

                    b.HasKey("Id");

                    b.ToTable("Users", (string)null);
                });

            modelBuilder.Entity("Ambev.DeveloperEvaluation.Domain.Entities.Sale", b =>
                {
                    b.OwnsOne("Ambev.DeveloperEvaluation.Domain.Entities.BranchId", "BranchId", b1 =>
                        {
                            b1.Property<Guid>("SaleId")
                                .HasColumnType("uuid");

                            b1.Property<Guid>("Value")
                                .HasColumnType("uuid")
                                .HasColumnName("BranchId");

                            b1.HasKey("SaleId");

                            b1.ToTable("Sales");

                            b1.WithOwner()
                                .HasForeignKey("SaleId");
                        });

                    b.OwnsOne("Ambev.DeveloperEvaluation.Domain.Entities.CustomerId", "CustomerId", b1 =>
                        {
                            b1.Property<Guid>("SaleId")
                                .HasColumnType("uuid");

                            b1.Property<Guid>("Value")
                                .HasColumnType("uuid")
                                .HasColumnName("CustomerId");

                            b1.HasKey("SaleId");

                            b1.ToTable("Sales");

                            b1.WithOwner()
                                .HasForeignKey("SaleId");
                        });

                    b.OwnsMany("Ambev.DeveloperEvaluation.Domain.Entities.SaleItem", "Items", b1 =>
                        {
                            b1.Property<Guid>("Id")
                                .ValueGeneratedOnAdd()
                                .HasColumnType("uuid");

                            b1.Property<decimal>("DiscountRate")
                                .HasColumnType("decimal(5,2)");

                            b1.Property<bool>("IsCancelled")
                                .ValueGeneratedOnAdd()
                                .HasColumnType("boolean")
                                .HasDefaultValue(false)
                                .HasColumnName("IsCancelled");

                            b1.Property<Guid>("ProductId")
                                .HasColumnType("uuid");

                            b1.Property<int>("Quantity")
                                .HasColumnType("integer");

                            b1.Property<Guid>("SaleId")
                                .HasColumnType("uuid");

                            b1.Property<decimal>("UnitPrice")
                                .HasColumnType("decimal(18,2)");

                            b1.HasKey("Id");

                            b1.HasIndex("SaleId");

                            b1.ToTable("SaleItems", (string)null);

                            b1.WithOwner()
                                .HasForeignKey("SaleId");
                        });

                    b.Navigation("BranchId")
                        .IsRequired();

                    b.Navigation("CustomerId")
                        .IsRequired();

                    b.Navigation("Items");
                });
#pragma warning restore 612, 618
        }
    }
}

﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using MyWorkManagerSN.Model;

#nullable disable

namespace MyWorkManagerSN.Model.Migrations
{
    [DbContext(typeof(MyEntitiesContext))]
    [Migration("20220309140110_showimage")]
    partial class showimage
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.2")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("MyWorkManagerSN.Model.Category", b =>
                {
                    b.Property<string>("ID")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Code")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Label")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ID");

                    b.ToTable("Category");
                });

            modelBuilder.Entity("MyWorkManagerSN.Model.Customer", b =>
                {
                    b.Property<string>("ID")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Email")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Mobile")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Surname")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ID");

                    b.ToTable("Customer");
                });

            modelBuilder.Entity("MyWorkManagerSN.Model.Field", b =>
                {
                    b.Property<string>("ID")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("CustomerID")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Label")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ProductID")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Type")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Value")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ID");

                    b.HasIndex("CustomerID");

                    b.HasIndex("ProductID");

                    b.ToTable("Field");
                });

            modelBuilder.Entity("MyWorkManagerSN.Model.Order", b =>
                {
                    b.Property<string>("ID")
                        .HasColumnType("nvarchar(450)");

                    b.Property<double>("AmountPaid")
                        .HasColumnType("float");

                    b.Property<double>("AmountTotal")
                        .HasColumnType("float");

                    b.Property<string>("CustomerId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<DateTime>("DateCreation")
                        .HasColumnType("datetime2");

                    b.Property<double>("Discount")
                        .HasColumnType("float");

                    b.Property<bool>("IsOrderSubuscription")
                        .HasColumnType("bit");

                    b.Property<int>("NumOrder")
                        .HasColumnType("int");

                    b.Property<string>("PaymentModeId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ID");

                    b.HasIndex("CustomerId");

                    b.HasIndex("PaymentModeId");

                    b.ToTable("Order");
                });

            modelBuilder.Entity("MyWorkManagerSN.Model.OrderLine", b =>
                {
                    b.Property<string>("ID")
                        .HasColumnType("nvarchar(450)");

                    b.Property<double>("AmountTotalHT")
                        .HasColumnType("float");

                    b.Property<double>("AmountTotalTTc")
                        .HasColumnType("float");

                    b.Property<double>("Discount")
                        .HasColumnType("float");

                    b.Property<string>("OrderID")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ProductId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("Quantity")
                        .HasColumnType("int");

                    b.Property<int>("TVA")
                        .HasColumnType("int");

                    b.Property<double>("UnitPrice")
                        .HasColumnType("float");

                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ID");

                    b.HasIndex("OrderID");

                    b.HasIndex("ProductId");

                    b.ToTable("OrderLines");
                });

            modelBuilder.Entity("MyWorkManagerSN.Model.PaymentMode", b =>
                {
                    b.Property<string>("ID")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Code")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Label")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ID");

                    b.ToTable("PaymentModes");
                });

            modelBuilder.Entity("MyWorkManagerSN.Model.Product", b =>
                {
                    b.Property<string>("ID")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("CategoryId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Code")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Label")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<double>("PriceHT")
                        .HasColumnType("float");

                    b.Property<double>("PriceTtc")
                        .HasColumnType("float");

                    b.Property<int>("Stock")
                        .HasColumnType("int");

                    b.Property<int>("TVA")
                        .HasColumnType("int");

                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ID");

                    b.HasIndex("CategoryId");

                    b.ToTable("Product");
                });

            modelBuilder.Entity("MyWorkManagerSN.Model.SubscriptionWithAmount", b =>
                {
                    b.Property<string>("ID")
                        .HasColumnType("nvarchar(450)");

                    b.Property<double>("Amount")
                        .HasColumnType("float");

                    b.Property<string>("CustomerId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ID");

                    b.HasIndex("CustomerId");

                    b.ToTable("SubscriptionWithAmounts");
                });

            modelBuilder.Entity("MyWorkManagerSN.Model.User", b =>
                {
                    b.Property<string>("ID")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("CompanyName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Devise")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Mobile")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ProfileImagePath")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("ShowImageOnInvoice")
                        .HasColumnType("bit");

                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Username")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ID");

                    b.ToTable("User");
                });

            modelBuilder.Entity("MyWorkManagerSN.Model.Customer", b =>
                {
                    b.OwnsOne("MyWorkManagerSN.Model.Address", "Address", b1 =>
                        {
                            b1.Property<string>("CustomerID")
                                .HasColumnType("nvarchar(450)");

                            b1.Property<string>("CodePostal")
                                .HasColumnType("nvarchar(max)");

                            b1.Property<string>("Complement")
                                .HasColumnType("nvarchar(max)");

                            b1.Property<string>("PaysCode")
                                .HasColumnType("nvarchar(max)");

                            b1.Property<string>("Rue")
                                .HasColumnType("nvarchar(max)");

                            b1.Property<string>("Ville")
                                .HasColumnType("nvarchar(max)");

                            b1.HasKey("CustomerID");

                            b1.ToTable("Customer");

                            b1.WithOwner()
                                .HasForeignKey("CustomerID");
                        });

                    b.Navigation("Address");
                });

            modelBuilder.Entity("MyWorkManagerSN.Model.Field", b =>
                {
                    b.HasOne("MyWorkManagerSN.Model.Customer", null)
                        .WithMany("MoreFields")
                        .HasForeignKey("CustomerID");

                    b.HasOne("MyWorkManagerSN.Model.Product", null)
                        .WithMany("MoreFields")
                        .HasForeignKey("ProductID");
                });

            modelBuilder.Entity("MyWorkManagerSN.Model.Order", b =>
                {
                    b.HasOne("MyWorkManagerSN.Model.Customer", "Customer")
                        .WithMany()
                        .HasForeignKey("CustomerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("MyWorkManagerSN.Model.PaymentMode", "PaymentMode")
                        .WithMany()
                        .HasForeignKey("PaymentModeId");

                    b.Navigation("Customer");

                    b.Navigation("PaymentMode");
                });

            modelBuilder.Entity("MyWorkManagerSN.Model.OrderLine", b =>
                {
                    b.HasOne("MyWorkManagerSN.Model.Order", null)
                        .WithMany("Lines")
                        .HasForeignKey("OrderID");

                    b.HasOne("MyWorkManagerSN.Model.Product", "Product")
                        .WithMany()
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Product");
                });

            modelBuilder.Entity("MyWorkManagerSN.Model.Product", b =>
                {
                    b.HasOne("MyWorkManagerSN.Model.Category", "Category")
                        .WithMany("Products")
                        .HasForeignKey("CategoryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Category");
                });

            modelBuilder.Entity("MyWorkManagerSN.Model.SubscriptionWithAmount", b =>
                {
                    b.HasOne("MyWorkManagerSN.Model.Customer", "Customer")
                        .WithMany()
                        .HasForeignKey("CustomerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Customer");
                });

            modelBuilder.Entity("MyWorkManagerSN.Model.User", b =>
                {
                    b.OwnsOne("MyWorkManagerSN.Model.Address", "Address", b1 =>
                        {
                            b1.Property<string>("UserID")
                                .HasColumnType("nvarchar(450)");

                            b1.Property<string>("CodePostal")
                                .HasColumnType("nvarchar(max)");

                            b1.Property<string>("Complement")
                                .HasColumnType("nvarchar(max)");

                            b1.Property<string>("PaysCode")
                                .HasColumnType("nvarchar(max)");

                            b1.Property<string>("Rue")
                                .HasColumnType("nvarchar(max)");

                            b1.Property<string>("Ville")
                                .HasColumnType("nvarchar(max)");

                            b1.HasKey("UserID");

                            b1.ToTable("User");

                            b1.WithOwner()
                                .HasForeignKey("UserID");
                        });

                    b.Navigation("Address");
                });

            modelBuilder.Entity("MyWorkManagerSN.Model.Category", b =>
                {
                    b.Navigation("Products");
                });

            modelBuilder.Entity("MyWorkManagerSN.Model.Customer", b =>
                {
                    b.Navigation("MoreFields");
                });

            modelBuilder.Entity("MyWorkManagerSN.Model.Order", b =>
                {
                    b.Navigation("Lines");
                });

            modelBuilder.Entity("MyWorkManagerSN.Model.Product", b =>
                {
                    b.Navigation("MoreFields");
                });
#pragma warning restore 612, 618
        }
    }
}

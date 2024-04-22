﻿// <auto-generated />
using System;
using Infrastructure.Repositories.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Infrastructure.Repositories.Migrations
{
    [DbContext(typeof(CamAIContext))]
    partial class CamAIContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Core.Domain.Entities.Account", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("AccountStatus")
                        .HasColumnType("int");

                    b.Property<string>("AddressLine")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateOnly?>("Birthday")
                        .HasColumnType("date");

                    b.Property<Guid?>("BrandId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("Gender")
                        .HasColumnType("int");

                    b.Property<DateTime>("ModifiedDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Phone")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<int>("Role")
                        .HasColumnType("int");

                    b.Property<byte[]>("Timestamp")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("rowversion");

                    b.Property<int?>("WardId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("BrandId");

                    b.HasIndex("WardId");

                    b.ToTable("Accounts");
                });

            modelBuilder.Entity("Core.Domain.Entities.AccountNotification", b =>
                {
                    b.Property<Guid>("AccountId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("NotificationId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.HasKey("AccountId", "NotificationId");

                    b.HasIndex("NotificationId");

                    b.ToTable("AccountNotifications");
                });

            modelBuilder.Entity("Core.Domain.Entities.Brand", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("BannerId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("BrandManagerId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("BrandStatus")
                        .HasColumnType("int");

                    b.Property<string>("BrandWebsite")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CompanyAddress")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.Property<string>("CompanyName")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<int?>("CompanyWardId")
                        .HasColumnType("int");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Email")
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid?>("LogoId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("ModifiedDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("Phone")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<byte[]>("Timestamp")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("rowversion");

                    b.HasKey("Id");

                    b.HasIndex("BannerId");

                    b.HasIndex("BrandManagerId")
                        .IsUnique()
                        .HasFilter("[BrandManagerId] IS NOT NULL");

                    b.HasIndex("CompanyWardId");

                    b.HasIndex("LogoId");

                    b.ToTable("Brands");
                });

            modelBuilder.Entity("Core.Domain.Entities.Camera", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Host")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("ModifiedDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<string>("Path")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Port")
                        .HasColumnType("int");

                    b.Property<string>("Protocol")
                        .IsRequired()
                        .HasMaxLength(10)
                        .HasColumnType("nvarchar(10)");

                    b.Property<Guid>("ShopId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.Property<byte[]>("Timestamp")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("rowversion");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<bool>("WillRunAI")
                        .HasColumnType("bit");

                    b.Property<int>("Zone")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("ShopId");

                    b.ToTable("Cameras");
                });

            modelBuilder.Entity("Core.Domain.Entities.District", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<int>("ProvinceId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("ProvinceId");

                    b.ToTable("Districts");
                });

            modelBuilder.Entity("Core.Domain.Entities.EdgeBox", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("EdgeBoxLocation")
                        .HasColumnType("int");

                    b.Property<Guid>("EdgeBoxModelId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("EdgeBoxStatus")
                        .HasColumnType("int");

                    b.Property<string>("MacAddress")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("ModifiedDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Name")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("SerialNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<byte[]>("Timestamp")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("rowversion");

                    b.Property<string>("Version")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("EdgeBoxModelId");

                    b.ToTable("EdgeBoxes");
                });

            modelBuilder.Entity("Core.Domain.Entities.EdgeBoxActivity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid?>("EdgeBoxId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("EdgeBoxInstallId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("ModifiedById")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("ModifiedTime")
                        .HasColumnType("datetime2");

                    b.Property<int>("Type")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("EdgeBoxId");

                    b.HasIndex("EdgeBoxInstallId");

                    b.HasIndex("ModifiedById");

                    b.ToTable("EdgeBoxActivities");
                });

            modelBuilder.Entity("Core.Domain.Entities.EdgeBoxInstall", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("ActivationCode")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("ActivationStatus")
                        .HasColumnType("int");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime2");

                    b.Property<Guid>("EdgeBoxId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("EdgeBoxInstallStatus")
                        .HasColumnType("int");

                    b.Property<string>("IpAddress")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("LastSeen")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("ModifiedDate")
                        .HasColumnType("datetime2");

                    b.Property<bool>("NotificationSent")
                        .HasColumnType("bit");

                    b.Property<string>("OperatingSystem")
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("ShopId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<byte[]>("Timestamp")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("rowversion");

                    b.Property<DateTime?>("UninstalledTime")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("EdgeBoxId");

                    b.HasIndex("ShopId");

                    b.ToTable("EdgeBoxInstalls");
                });

            modelBuilder.Entity("Core.Domain.Entities.EdgeBoxModel", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("CPU")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Manufacturer")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ModelCode")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("ModifiedDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("OS")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("RAM")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Storage")
                        .HasColumnType("nvarchar(max)");

                    b.Property<byte[]>("Timestamp")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("rowversion");

                    b.HasKey("Id");

                    b.ToTable("EdgeBoxModels");
                });

            modelBuilder.Entity("Core.Domain.Entities.Employee", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("AddressLine")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateOnly?>("Birthday")
                        .HasColumnType("date");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Email")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("EmployeeStatus")
                        .HasColumnType("int");

                    b.Property<int>("Gender")
                        .HasColumnType("int");

                    b.Property<string>("Image")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("ModifiedDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("Phone")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<Guid?>("ShopId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<byte[]>("Timestamp")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("rowversion");

                    b.Property<int?>("WardId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("ShopId");

                    b.HasIndex("WardId");

                    b.ToTable("Employees");
                });

            modelBuilder.Entity("Core.Domain.Entities.Evidence", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("CameraId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("EvidenceType")
                        .HasColumnType("int");

                    b.Property<Guid?>("ImageId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("IncidentId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("ModifiedDate")
                        .HasColumnType("datetime2");

                    b.Property<byte[]>("Timestamp")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("rowversion");

                    b.HasKey("Id");

                    b.HasIndex("CameraId");

                    b.HasIndex("ImageId");

                    b.HasIndex("IncidentId");

                    b.ToTable("Evidences");
                });

            modelBuilder.Entity("Core.Domain.Entities.Image", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("ContentType")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("HostingUri")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhysicalPath")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Images");
                });

            modelBuilder.Entity("Core.Domain.Entities.Incident", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("AiId")
                        .HasColumnType("int");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime2");

                    b.Property<Guid>("EdgeBoxId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("EmployeeId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime?>("EndTime")
                        .HasColumnType("datetime2");

                    b.Property<int>("IncidentType")
                        .HasColumnType("int");

                    b.Property<DateTime>("ModifiedDate")
                        .HasColumnType("datetime2");

                    b.Property<Guid>("ShopId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("StartTime")
                        .HasColumnType("datetime2");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.Property<byte[]>("Timestamp")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("rowversion");

                    b.HasKey("Id");

                    b.HasIndex("EdgeBoxId");

                    b.HasIndex("EmployeeId");

                    b.HasIndex("ShopId");

                    b.ToTable("Incidents");
                });

            modelBuilder.Entity("Core.Domain.Entities.Notification", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("AccountId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Content")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("ModifiedDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("Priority")
                        .HasColumnType("int");

                    b.Property<Guid?>("RelatedEntityId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<byte[]>("Timestamp")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("rowversion");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.Property<int>("Type")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("AccountId");

                    b.ToTable("Notifications");
                });

            modelBuilder.Entity("Core.Domain.Entities.Province", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("Id");

                    b.ToTable("Provinces");
                });

            modelBuilder.Entity("Core.Domain.Entities.Shop", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("AddressLine")
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("BrandId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<TimeOnly>("CloseTime")
                        .HasColumnType("time");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("ModifiedDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<TimeOnly>("OpenTime")
                        .HasColumnType("time");

                    b.Property<string>("Phone")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<Guid?>("ShopManagerId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("ShopStatus")
                        .HasColumnType("int");

                    b.Property<byte[]>("Timestamp")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("rowversion");

                    b.Property<int>("WardId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("BrandId");

                    b.HasIndex("ShopManagerId")
                        .IsUnique()
                        .HasFilter("[ShopManagerId] IS NOT NULL");

                    b.HasIndex("WardId");

                    b.ToTable("Shops");
                });

            modelBuilder.Entity("Core.Domain.Entities.Ward", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("DistrictId")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("Id");

                    b.HasIndex("DistrictId");

                    b.ToTable("Wards");
                });

            modelBuilder.Entity("Core.Domain.Entities.Account", b =>
                {
                    b.HasOne("Core.Domain.Entities.Brand", "Brand")
                        .WithMany("Accounts")
                        .HasForeignKey("BrandId");

                    b.HasOne("Core.Domain.Entities.Ward", "Ward")
                        .WithMany()
                        .HasForeignKey("WardId");

                    b.Navigation("Brand");

                    b.Navigation("Ward");
                });

            modelBuilder.Entity("Core.Domain.Entities.AccountNotification", b =>
                {
                    b.HasOne("Core.Domain.Entities.Account", "Account")
                        .WithMany("ReceivedNotifications")
                        .HasForeignKey("AccountId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Core.Domain.Entities.Notification", "Notification")
                        .WithMany("SentTo")
                        .HasForeignKey("NotificationId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Account");

                    b.Navigation("Notification");
                });

            modelBuilder.Entity("Core.Domain.Entities.Brand", b =>
                {
                    b.HasOne("Core.Domain.Entities.Image", "Banner")
                        .WithMany()
                        .HasForeignKey("BannerId");

                    b.HasOne("Core.Domain.Entities.Account", "BrandManager")
                        .WithOne("ManagingBrand")
                        .HasForeignKey("Core.Domain.Entities.Brand", "BrandManagerId");

                    b.HasOne("Core.Domain.Entities.Ward", "CompanyWard")
                        .WithMany()
                        .HasForeignKey("CompanyWardId");

                    b.HasOne("Core.Domain.Entities.Image", "Logo")
                        .WithMany()
                        .HasForeignKey("LogoId");

                    b.Navigation("Banner");

                    b.Navigation("BrandManager");

                    b.Navigation("CompanyWard");

                    b.Navigation("Logo");
                });

            modelBuilder.Entity("Core.Domain.Entities.Camera", b =>
                {
                    b.HasOne("Core.Domain.Entities.Shop", "Shop")
                        .WithMany("Cameras")
                        .HasForeignKey("ShopId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Shop");
                });

            modelBuilder.Entity("Core.Domain.Entities.District", b =>
                {
                    b.HasOne("Core.Domain.Entities.Province", "Province")
                        .WithMany("Districts")
                        .HasForeignKey("ProvinceId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Province");
                });

            modelBuilder.Entity("Core.Domain.Entities.EdgeBox", b =>
                {
                    b.HasOne("Core.Domain.Entities.EdgeBoxModel", "EdgeBoxModel")
                        .WithMany("EdgeBoxes")
                        .HasForeignKey("EdgeBoxModelId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("EdgeBoxModel");
                });

            modelBuilder.Entity("Core.Domain.Entities.EdgeBoxActivity", b =>
                {
                    b.HasOne("Core.Domain.Entities.EdgeBox", "EdgeBox")
                        .WithMany()
                        .HasForeignKey("EdgeBoxId");

                    b.HasOne("Core.Domain.Entities.EdgeBoxInstall", "EdgeBoxInstall")
                        .WithMany()
                        .HasForeignKey("EdgeBoxInstallId");

                    b.HasOne("Core.Domain.Entities.Account", "ModifiedBy")
                        .WithMany()
                        .HasForeignKey("ModifiedById");

                    b.Navigation("EdgeBox");

                    b.Navigation("EdgeBoxInstall");

                    b.Navigation("ModifiedBy");
                });

            modelBuilder.Entity("Core.Domain.Entities.EdgeBoxInstall", b =>
                {
                    b.HasOne("Core.Domain.Entities.EdgeBox", "EdgeBox")
                        .WithMany("Installs")
                        .HasForeignKey("EdgeBoxId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Core.Domain.Entities.Shop", "Shop")
                        .WithMany("EdgeBoxInstalls")
                        .HasForeignKey("ShopId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("EdgeBox");

                    b.Navigation("Shop");
                });

            modelBuilder.Entity("Core.Domain.Entities.Employee", b =>
                {
                    b.HasOne("Core.Domain.Entities.Shop", "Shop")
                        .WithMany("Employees")
                        .HasForeignKey("ShopId");

                    b.HasOne("Core.Domain.Entities.Ward", "Ward")
                        .WithMany()
                        .HasForeignKey("WardId");

                    b.Navigation("Shop");

                    b.Navigation("Ward");
                });

            modelBuilder.Entity("Core.Domain.Entities.Evidence", b =>
                {
                    b.HasOne("Core.Domain.Entities.Camera", "Camera")
                        .WithMany()
                        .HasForeignKey("CameraId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Core.Domain.Entities.Image", "Image")
                        .WithMany()
                        .HasForeignKey("ImageId");

                    b.HasOne("Core.Domain.Entities.Incident", "Incident")
                        .WithMany("Evidences")
                        .HasForeignKey("IncidentId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Camera");

                    b.Navigation("Image");

                    b.Navigation("Incident");
                });

            modelBuilder.Entity("Core.Domain.Entities.Incident", b =>
                {
                    b.HasOne("Core.Domain.Entities.EdgeBox", "EdgeBox")
                        .WithMany()
                        .HasForeignKey("EdgeBoxId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Core.Domain.Entities.Employee", "Employee")
                        .WithMany("Incidents")
                        .HasForeignKey("EmployeeId");

                    b.HasOne("Core.Domain.Entities.Shop", "Shop")
                        .WithMany()
                        .HasForeignKey("ShopId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("EdgeBox");

                    b.Navigation("Employee");

                    b.Navigation("Shop");
                });

            modelBuilder.Entity("Core.Domain.Entities.Notification", b =>
                {
                    b.HasOne("Core.Domain.Entities.Account", null)
                        .WithMany("SentNotifications")
                        .HasForeignKey("AccountId");
                });

            modelBuilder.Entity("Core.Domain.Entities.Shop", b =>
                {
                    b.HasOne("Core.Domain.Entities.Brand", "Brand")
                        .WithMany("Shops")
                        .HasForeignKey("BrandId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Core.Domain.Entities.Account", "ShopManager")
                        .WithOne("ManagingShop")
                        .HasForeignKey("Core.Domain.Entities.Shop", "ShopManagerId");

                    b.HasOne("Core.Domain.Entities.Ward", "Ward")
                        .WithMany()
                        .HasForeignKey("WardId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Brand");

                    b.Navigation("ShopManager");

                    b.Navigation("Ward");
                });

            modelBuilder.Entity("Core.Domain.Entities.Ward", b =>
                {
                    b.HasOne("Core.Domain.Entities.District", "District")
                        .WithMany("Wards")
                        .HasForeignKey("DistrictId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("District");
                });

            modelBuilder.Entity("Core.Domain.Entities.Account", b =>
                {
                    b.Navigation("ManagingBrand");

                    b.Navigation("ManagingShop");

                    b.Navigation("ReceivedNotifications");

                    b.Navigation("SentNotifications");
                });

            modelBuilder.Entity("Core.Domain.Entities.Brand", b =>
                {
                    b.Navigation("Accounts");

                    b.Navigation("Shops");
                });

            modelBuilder.Entity("Core.Domain.Entities.District", b =>
                {
                    b.Navigation("Wards");
                });

            modelBuilder.Entity("Core.Domain.Entities.EdgeBox", b =>
                {
                    b.Navigation("Installs");
                });

            modelBuilder.Entity("Core.Domain.Entities.EdgeBoxModel", b =>
                {
                    b.Navigation("EdgeBoxes");
                });

            modelBuilder.Entity("Core.Domain.Entities.Employee", b =>
                {
                    b.Navigation("Incidents");
                });

            modelBuilder.Entity("Core.Domain.Entities.Incident", b =>
                {
                    b.Navigation("Evidences");
                });

            modelBuilder.Entity("Core.Domain.Entities.Notification", b =>
                {
                    b.Navigation("SentTo");
                });

            modelBuilder.Entity("Core.Domain.Entities.Province", b =>
                {
                    b.Navigation("Districts");
                });

            modelBuilder.Entity("Core.Domain.Entities.Shop", b =>
                {
                    b.Navigation("Cameras");

                    b.Navigation("EdgeBoxInstalls");

                    b.Navigation("Employees");
                });
#pragma warning restore 612, 618
        }
    }
}

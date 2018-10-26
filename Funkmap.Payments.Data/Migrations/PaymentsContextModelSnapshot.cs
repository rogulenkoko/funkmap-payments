﻿// <auto-generated />
using System;
using Funkmap.Payments.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Funkmap.Payments.Data.Migrations
{
    [DbContext(typeof(PaymentsContext))]
    partial class PaymentsContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.1.4-rtm-31024")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Funkmap.Payments.Data.Entities.PaymentEntity", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Currency")
                        .IsRequired();

                    b.Property<DateTime>("DateUtc");

                    b.Property<string>("ExternalId")
                        .IsRequired();

                    b.Property<int>("PaymentStatus");

                    b.Property<decimal>("Total");

                    b.HasKey("Id");

                    b.ToTable("Payments");
                });

            modelBuilder.Entity("Funkmap.Payments.Data.Entities.PayPalPlanEntity", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<long>("ProductLocaleId");

                    b.HasKey("Id");

                    b.HasIndex("ProductLocaleId")
                        .IsUnique();

                    b.ToTable("PayPalPlans");
                });

            modelBuilder.Entity("Funkmap.Payments.Data.Entities.ProductEntity", b =>
                {
                    b.Property<string>("Name")
                        .ValueGeneratedOnAdd();

                    b.Property<bool>("HasTrial");

                    b.Property<int>("PaymentType");

                    b.Property<int>("Period");

                    b.HasKey("Name");

                    b.ToTable("Products");

                    b.HasData(
                        new { Name = "pro_account", HasTrial = false, PaymentType = 1, Period = 1 },
                        new { Name = "priority_profile", HasTrial = false, PaymentType = 1, Period = 1 }
                    );
                });

            modelBuilder.Entity("Funkmap.Payments.Data.Entities.ProductLocaleEntity", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Currency");

                    b.Property<string>("Description");

                    b.Property<string>("Language");

                    b.Property<string>("Name");

                    b.Property<string>("ProductName")
                        .IsRequired();

                    b.Property<decimal>("Total");

                    b.HasKey("Id");

                    b.HasIndex("ProductName");

                    b.ToTable("ProductLocales");

                    b.HasData(
                        new { Id = 1L, Currency = "RUB", Description = "Предоставляет возможность создания более 1 профиля. Применятеся к пользователю ресурса (не к его профилям).", Language = "ru-RU", Name = "Pro-аккаунт", ProductName = "pro_account", Total = 300m },
                        new { Id = 2L, Currency = "USD", Description = "Provides the ability to create more than 1 profile. Apply to the resource user (not to his profiles).", Language = "en-US", Name = "Pro-account", ProductName = "pro_account", Total = 5m },
                        new { Id = 3L, Currency = "RUB", Description = "Позволяет выделять один из ваших профилей на карте Bandmap и выдавать одним из первых в поисковых запросах.", Language = "ru-RU", Name = "Приоритетный профиль", ProductName = "priority_profile", Total = 300m },
                        new { Id = 4L, Currency = "USD", Description = "Allows you to highlight one of your profiles on the Bandmap map and issue one of the first in search queries.", Language = "en-US", Name = "Priority profile", ProductName = "priority_profile", Total = 5m }
                    );
                });

            modelBuilder.Entity("Funkmap.Payments.Data.Entities.PayPalPlanEntity", b =>
                {
                    b.HasOne("Funkmap.Payments.Data.Entities.ProductLocaleEntity")
                        .WithOne()
                        .HasForeignKey("Funkmap.Payments.Data.Entities.PayPalPlanEntity", "ProductLocaleId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Funkmap.Payments.Data.Entities.ProductLocaleEntity", b =>
                {
                    b.HasOne("Funkmap.Payments.Data.Entities.ProductEntity")
                        .WithMany("ProductLocales")
                        .HasForeignKey("ProductName")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
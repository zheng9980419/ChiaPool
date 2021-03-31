﻿// <auto-generated />
using ChiaMiningManager.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace ChiaMiningManager.Migrations
{
    [DbContext(typeof(ConfigurationContext))]
    partial class ConfigurationContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "5.0.4");

            modelBuilder.Entity("ChiaMiningManager.Models.PlotInfo", b =>
                {
                    b.Property<string>("PublicKey")
                        .HasColumnType("TEXT");

                    b.Property<string>("FileName")
                        .HasColumnType("TEXT");

                    b.Property<int>("Minutes")
                        .HasColumnType("INTEGER");

                    b.HasKey("PublicKey");

                    b.ToTable("PlotInfos");
                });
#pragma warning restore 612, 618
        }
    }
}

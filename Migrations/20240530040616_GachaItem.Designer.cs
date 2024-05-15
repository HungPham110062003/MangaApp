﻿// <auto-generated />
using System;
using MangaApp.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace MangaApp.Migrations
{
    [DbContext(typeof(MangaAppDbcontext))]
    [Migration("20240530040616_GachaItem")]
    partial class GachaItem
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.0-preview.3.24172.4")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("MangaApp.Model.Domain.User", b =>
                {
                    b.Property<Guid>("UserId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Avatar")
                        .HasColumnType("text");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<byte[]>("HashPassword")
                        .HasColumnType("bytea");

                    b.Property<long>("Point")
                        .HasColumnType("bigint");

                    b.Property<byte[]>("SaltPassword")
                        .HasColumnType("bytea");

                    b.Property<string>("UserEmail")
                        .HasColumnType("text");

                    b.Property<string>("UserName")
                        .HasColumnType("text");

                    b.Property<string>("faceAuthenticationImage")
                        .HasColumnType("text");

                    b.HasKey("UserId");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("MangaApp.Model.Domain.UserManga", b =>
                {
                    b.Property<Guid>("MangaId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("MangaImage")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("MangaName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Slug")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid");

                    b.HasKey("MangaId");

                    b.HasIndex("UserId");

                    b.ToTable("UserMangas");
                });

            modelBuilder.Entity("MangaApp.Model.Domain.UserManga", b =>
                {
                    b.HasOne("MangaApp.Model.Domain.User", "User")
                        .WithMany("UserMangas")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("MangaApp.Model.Domain.User", b =>
                {
                    b.Navigation("UserMangas");
                });
#pragma warning restore 612, 618
        }
    }
}

﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using TvTrackServer;

#nullable disable

namespace TvTrackServer.Migrations
{
    [DbContext(typeof(TvTrackServerDbContext))]
    [Migration("20230508140550_OptionalTokenId")]
    partial class OptionalTokenId
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.4")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("TVTrack.Models.API.Database.Tokens", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("FCMDeviceToken")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("GoogleCalendarRefreshToken")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("GoogleCalendarToken")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("Tokens");
                });

            modelBuilder.Entity("TVTrack.Models.Database.EpisodeActivity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("ShowActivityId")
                        .HasColumnType("int");

                    b.Property<int>("TvMazeId")
                        .HasColumnType("int");

                    b.Property<bool>("Watched")
                        .HasColumnType("bit");

                    b.HasKey("Id");

                    b.HasIndex("ShowActivityId");

                    b.ToTable("EpisodeActivities");
                });

            modelBuilder.Entity("TVTrack.Models.Database.ShowActivity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<bool>("Calendar")
                        .HasColumnType("bit");

                    b.Property<DateTime>("NextNotifyDate")
                        .HasColumnType("datetime2");

                    b.Property<bool>("Notifications")
                        .HasColumnType("bit");

                    b.Property<int>("TvMazeId")
                        .HasColumnType("int");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.Property<bool>("UserRated")
                        .HasColumnType("bit");

                    b.Property<int>("UserRating")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("ShowActivities");
                });

            modelBuilder.Entity("TVTrack.Models.Database.ShowList", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<bool>("Default")
                        .HasColumnType("bit");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("ShowLists");
                });

            modelBuilder.Entity("TVTrack.Models.Database.ShowListItem", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("ShowListId")
                        .HasColumnType("int");

                    b.Property<int>("TvMazeId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("ShowListId");

                    b.ToTable("ShowListItems");
                });

            modelBuilder.Entity("TVTrack.Models.Database.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int?>("TokensId")
                        .HasColumnType("int");

                    b.Property<string>("Username")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("TokensId")
                        .IsUnique()
                        .HasFilter("[TokensId] IS NOT NULL");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("TVTrack.Models.Database.UserRatedShow", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("TvMazeId")
                        .HasColumnType("int");

                    b.Property<float>("UserRating")
                        .HasColumnType("real");

                    b.Property<int>("UserRatingCount")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("UserRatedShows");
                });

            modelBuilder.Entity("TVTrack.Models.Database.EpisodeActivity", b =>
                {
                    b.HasOne("TVTrack.Models.Database.ShowActivity", "ShowActivity")
                        .WithMany("EpisodeActivities")
                        .HasForeignKey("ShowActivityId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("ShowActivity");
                });

            modelBuilder.Entity("TVTrack.Models.Database.ShowActivity", b =>
                {
                    b.HasOne("TVTrack.Models.Database.User", "User")
                        .WithMany("ShowActivities")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("TVTrack.Models.Database.ShowList", b =>
                {
                    b.HasOne("TVTrack.Models.Database.User", "User")
                        .WithMany("ShowLists")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("TVTrack.Models.Database.ShowListItem", b =>
                {
                    b.HasOne("TVTrack.Models.Database.ShowList", "ShowList")
                        .WithMany("Shows")
                        .HasForeignKey("ShowListId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("ShowList");
                });

            modelBuilder.Entity("TVTrack.Models.Database.User", b =>
                {
                    b.HasOne("TVTrack.Models.API.Database.Tokens", "Tokens")
                        .WithOne("User")
                        .HasForeignKey("TVTrack.Models.Database.User", "TokensId");

                    b.Navigation("Tokens");
                });

            modelBuilder.Entity("TVTrack.Models.API.Database.Tokens", b =>
                {
                    b.Navigation("User")
                        .IsRequired();
                });

            modelBuilder.Entity("TVTrack.Models.Database.ShowActivity", b =>
                {
                    b.Navigation("EpisodeActivities");
                });

            modelBuilder.Entity("TVTrack.Models.Database.ShowList", b =>
                {
                    b.Navigation("Shows");
                });

            modelBuilder.Entity("TVTrack.Models.Database.User", b =>
                {
                    b.Navigation("ShowActivities");

                    b.Navigation("ShowLists");
                });
#pragma warning restore 612, 618
        }
    }
}

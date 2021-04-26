﻿// <auto-generated />
using System;
using BGG.Bot.Data.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace BGG.Bot.Data.Migrations
{
    [DbContext(typeof(CollectionContext))]
    [Migration("20210426044710_change_fk_to_bggid")]
    partial class change_fk_to_bggid
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "5.0.5");

            modelBuilder.Entity("BGG.Bot.Data.Models.CollectionItem", b =>
                {
                    b.Property<int>("CollectionItemId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("BggId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .HasColumnType("TEXT");

                    b.HasKey("CollectionItemId");

                    b.ToTable("CollectionItems");
                });

            modelBuilder.Entity("BGG.Bot.Data.Models.User", b =>
                {
                    b.Property<int>("UserId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("BggUsername")
                        .HasColumnType("TEXT");

                    b.Property<ulong>("DiscordId")
                        .HasColumnType("INTEGER");

                    b.HasKey("UserId");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("BGG.Bot.Data.Models.UserCollectionItem", b =>
                {
                    b.Property<int>("UserId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("BggId")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("ForTrade")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("LastModified")
                        .HasColumnType("TEXT");

                    b.Property<bool>("Owned")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("PreOrdered")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("PreviouslyOwned")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("Want")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("WantToBuy")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("WantToPlay")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("WishList")
                        .HasColumnType("INTEGER");

                    b.HasKey("UserId", "BggId");

                    b.HasIndex("BggId");

                    b.ToTable("UserCollectionItem");
                });

            modelBuilder.Entity("BGG.Bot.Data.Models.UserCollectionItem", b =>
                {
                    b.HasOne("BGG.Bot.Data.Models.CollectionItem", "CollectionItem")
                        .WithMany("UserCollectionItems")
                        .HasForeignKey("BggId")
                        .HasPrincipalKey("BggId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("BGG.Bot.Data.Models.User", "User")
                        .WithMany("UserCollectionItems")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("CollectionItem");

                    b.Navigation("User");
                });

            modelBuilder.Entity("BGG.Bot.Data.Models.CollectionItem", b =>
                {
                    b.Navigation("UserCollectionItems");
                });

            modelBuilder.Entity("BGG.Bot.Data.Models.User", b =>
                {
                    b.Navigation("UserCollectionItems");
                });
#pragma warning restore 612, 618
        }
    }
}

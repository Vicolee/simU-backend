﻿// <auto-generated />
using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using SimU_GameService.Infrastructure.Persistence;

#nullable disable

namespace SimU_GameService.Infrastructure.Migrations
{
    [DbContext(typeof(SimUDbContext))]
    partial class SimUDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.13")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("SimU_GameService.Domain.Models.Agent", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uuid");

                    b.Property<DateTime>("CreatedTime")
                        .HasColumnType("timestamp with time zone");

                    b.Property<Guid>("CreatorId")
                        .HasColumnType("uuid");

                    b.Property<string>("Description")
                        .HasColumnType("text");

                    b.Property<DateTime>("HatchTime")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("SpriteHeadshotURL")
                        .HasColumnType("text");

                    b.Property<string>("SpriteURL")
                        .HasColumnType("text");

                    b.Property<string>("Summary")
                        .HasColumnType("text");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Agents");
                });

            modelBuilder.Entity("SimU_GameService.Domain.Models.Chat", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uuid");

                    b.Property<string>("Content")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<Guid>("ConversationId")
                        .HasColumnType("uuid");

                    b.Property<DateTime>("CreatedTime")
                        .HasColumnType("timestamp with time zone");

                    b.Property<bool>("IsGroupChat")
                        .HasColumnType("boolean");

                    b.Property<Guid>("RecipientId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("SenderId")
                        .HasColumnType("uuid");

                    b.Property<bool>("WasSenderOnline")
                        .HasColumnType("boolean");

                    b.HasKey("Id");

                    b.ToTable("Chats");
                });

            modelBuilder.Entity("SimU_GameService.Domain.Models.Conversation", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateTime>("CreatedTime")
                        .HasColumnType("timestamp with time zone");

                    b.Property<bool>("IsConversationOver")
                        .HasColumnType("boolean");

                    b.Property<bool>("IsGroupChat")
                        .HasColumnType("boolean");

                    b.Property<DateTime>("LastMessageTime")
                        .HasColumnType("timestamp with time zone");

                    b.Property<List<Guid>>("Participants")
                        .IsRequired()
                        .HasColumnType("uuid[]");

                    b.HasKey("Id");

                    b.ToTable("Conversations");
                });

            modelBuilder.Entity("SimU_GameService.Domain.Models.Group", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uuid");

                    b.Property<DateTime>("CreatedTime")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("MemberIds")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.Property<Guid>("OwnerId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.ToTable("Groups");
                });

            modelBuilder.Entity("SimU_GameService.Domain.Models.Question", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Content")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("QuestionType")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.ToTable("Questions");
                });

            modelBuilder.Entity("SimU_GameService.Domain.Models.User", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uuid");

                    b.Property<Guid>("ActiveWorldId")
                        .HasColumnType("uuid");

                    b.Property<DateTime>("CreatedTime")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Description")
                        .HasColumnType("text");

                    b.Property<string>("Email")
                        .HasColumnType("text");

                    b.Property<string>("IdentityId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<bool>("IsOnline")
                        .HasColumnType("boolean");

                    b.Property<string>("SpriteHeadshotURL")
                        .HasColumnType("text");

                    b.Property<string>("SpriteURL")
                        .HasColumnType("text");

                    b.Property<string>("Summary")
                        .HasColumnType("text");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("WorldsCreated")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("WorldsJoined")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("SimU_GameService.Domain.Models.World", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateTime>("CreatedTime")
                        .HasColumnType("timestamp with time zone");

                    b.Property<Guid>("CreatorId")
                        .HasColumnType("uuid");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("ThumbnailURL")
                        .HasColumnType("text");

                    b.Property<List<Guid>>("WorldAgents")
                        .IsRequired()
                        .HasColumnType("uuid[]");

                    b.Property<string>("WorldCode")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<List<Guid>>("WorldUsers")
                        .IsRequired()
                        .HasColumnType("uuid[]");

                    b.HasKey("Id");

                    b.ToTable("Worlds");
                });

            modelBuilder.Entity("SimU_GameService.Domain.Models.Agent", b =>
                {
                    b.OwnsOne("SimU_GameService.Domain.Models.Location", "Location", b1 =>
                        {
                            b1.Property<Guid>("AgentId")
                                .HasColumnType("uuid");

                            b1.Property<int>("X_coord")
                                .HasColumnType("integer")
                                .HasColumnName("X_coord");

                            b1.Property<int>("Y_coord")
                                .HasColumnType("integer")
                                .HasColumnName("Y_coord");

                            b1.HasKey("AgentId");

                            b1.ToTable("Agents");

                            b1.WithOwner()
                                .HasForeignKey("AgentId");
                        });

                    b.OwnsMany("SimU_GameService.Domain.Models.Response", "QuestionResponses", b1 =>
                        {
                            b1.Property<Guid>("Id")
                                .HasColumnType("uuid");

                            b1.Property<int>("Id1")
                                .ValueGeneratedOnAdd()
                                .HasColumnType("integer");

                            NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b1.Property<int>("Id1"));

                            b1.Property<string>("Content")
                                .IsRequired()
                                .HasColumnType("text");

                            b1.Property<Guid>("QuestionId")
                                .HasColumnType("uuid");

                            b1.Property<Guid>("ResponderId")
                                .HasColumnType("uuid");

                            b1.Property<Guid>("TargetId")
                                .HasColumnType("uuid");

                            b1.HasKey("Id", "Id1");

                            b1.ToTable("Agents_QuestionResponses");

                            b1.WithOwner()
                                .HasForeignKey("Id");
                        });

                    b.Navigation("Location");

                    b.Navigation("QuestionResponses");
                });

            modelBuilder.Entity("SimU_GameService.Domain.Models.User", b =>
                {
                    b.OwnsMany("SimU_GameService.Domain.Models.Friend", "Friends", b1 =>
                        {
                            b1.Property<Guid>("Id")
                                .HasColumnType("uuid");

                            b1.Property<Guid>("FriendId")
                                .HasColumnType("uuid");

                            b1.Property<DateTime>("CreatedTime")
                                .HasColumnType("timestamp with time zone");

                            b1.HasKey("Id", "FriendId");

                            b1.ToTable("Friend");

                            b1.WithOwner()
                                .HasForeignKey("Id");
                        });

                    b.OwnsMany("SimU_GameService.Domain.Models.Response", "QuestionResponses", b1 =>
                        {
                            b1.Property<Guid>("Id")
                                .HasColumnType("uuid");

                            b1.Property<int>("Id1")
                                .ValueGeneratedOnAdd()
                                .HasColumnType("integer");

                            NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b1.Property<int>("Id1"));

                            b1.Property<string>("Content")
                                .IsRequired()
                                .HasColumnType("text");

                            b1.Property<Guid>("QuestionId")
                                .HasColumnType("uuid");

                            b1.Property<Guid>("ResponderId")
                                .HasColumnType("uuid");

                            b1.Property<Guid>("TargetId")
                                .HasColumnType("uuid");

                            b1.HasKey("Id", "Id1");

                            b1.ToTable("Users_QuestionResponses");

                            b1.WithOwner()
                                .HasForeignKey("Id");
                        });

                    b.OwnsOne("SimU_GameService.Domain.Models.Location", "Location", b1 =>
                        {
                            b1.Property<Guid>("UserId")
                                .HasColumnType("uuid");

                            b1.Property<int>("X_coord")
                                .HasColumnType("integer")
                                .HasColumnName("X_coord");

                            b1.Property<int>("Y_coord")
                                .HasColumnType("integer")
                                .HasColumnName("Y_coord");

                            b1.HasKey("UserId");

                            b1.ToTable("Users");

                            b1.WithOwner()
                                .HasForeignKey("UserId");
                        });

                    b.Navigation("Friends");

                    b.Navigation("Location");

                    b.Navigation("QuestionResponses");
                });
#pragma warning restore 612, 618
        }
    }
}

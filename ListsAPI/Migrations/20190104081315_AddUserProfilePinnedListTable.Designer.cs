﻿// <auto-generated />
using System;
using ListsAPI.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace ListsAPI.Migrations
{
    [DbContext(typeof(ListContext))]
    [Migration("20190104081315_AddUserProfilePinnedListTable")]
    partial class AddUserProfilePinnedListTable
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.1.4-rtm-31024")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("ListsAPI.Features.Lists.Tables.List", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("BackgroundImageFileName")
                        .IsRequired();

                    b.Property<string>("BackgroundImageFilePath")
                        .IsRequired();

                    b.Property<DateTime>("DateCreated");

                    b.Property<DateTime>("DateUpdated");

                    b.Property<string>("Description")
                        .IsRequired();

                    b.Property<string>("Name")
                        .IsRequired();

                    b.Property<int?>("Position");

                    b.Property<int>("State");

                    b.Property<int>("UserId");

                    b.HasKey("Id");

                    b.ToTable("Lists");
                });

            modelBuilder.Entity("ListsAPI.Features.Lists.Tables.UserProfilePinnedList", b =>
                {
                    b.Property<int>("UserProfilePinnedListId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("ListId");

                    b.Property<int>("UserProfileId");

                    b.HasKey("UserProfilePinnedListId");

                    b.ToTable("UserProfilePinnedLists");
                });

            modelBuilder.Entity("ListsAPI.Features.Profile.Tables.UserProfile", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("DateCreated");

                    b.Property<DateTime>("DateUpdated");

                    b.Property<string>("EmailAddress")
                        .IsRequired();

                    b.Property<string>("FamilyName")
                        .IsRequired();

                    b.Property<string>("GivenName")
                        .IsRequired();

                    b.Property<string>("Password")
                        .IsRequired();

                    b.Property<string>("ProfilePictureFileName")
                        .IsRequired();

                    b.Property<string>("ProfilePicturePath")
                        .IsRequired();

                    b.HasKey("Id");

                    b.ToTable("UserProfiles");
                });

            modelBuilder.Entity("ListsAPI.Features.Profile.Tables.UserProfileToken", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("DateCreated");

                    b.Property<DateTime?>("DateUsed");

                    b.Property<DateTime>("ExpirationTime");

                    b.Property<string>("Token")
                        .IsRequired();

                    b.Property<int>("TokenType");

                    b.Property<int>("UserProfileId");

                    b.HasKey("Id");

                    b.ToTable("UserProfileTokens");
                });

            modelBuilder.Entity("ListsAPI.Features.TodoItems.Tables.TodoItem", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("DateCreated");

                    b.Property<DateTime>("DateUpdated");

                    b.Property<int>("ListId");

                    b.Property<int?>("Position");

                    b.Property<int>("State");

                    b.Property<string>("Title")
                        .IsRequired();

                    b.Property<int>("UserProfileId");

                    b.HasKey("Id");

                    b.ToTable("TodoItems");
                });
#pragma warning restore 612, 618
        }
    }
}

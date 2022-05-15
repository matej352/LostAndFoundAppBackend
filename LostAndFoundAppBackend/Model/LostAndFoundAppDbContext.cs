﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace EF.Model
{
    public partial class LostandfoundappdbContext : DbContext
    {
        public LostandfoundappdbContext()
        {
        }

        public LostandfoundappdbContext(DbContextOptions<LostandfoundappdbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Account> Account { get; set; }
        public virtual DbSet<Advertisement> Advertisement { get; set; }
        public virtual DbSet<Category> Category { get; set; }
        public virtual DbSet<Image> Image { get; set; }
        public virtual DbSet<Item> Item { get; set; }
        public virtual DbSet<Message> Message { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "Croatian_CI_AS");

            modelBuilder.Entity<Account>(entity =>
            {
                entity.Property(e => e.AccountId).HasColumnName("accountId");

                entity.Property(e => e.Active).HasColumnName("active");

                entity.Property(e => e.ConnectionId)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("connectionId");

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("email");

                entity.Property(e => e.FirstName)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("firstName");

                entity.Property(e => e.LastName)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("lastName");

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasColumnName("password");

                entity.Property(e => e.PasswordHashSalt)
                    .IsRequired()
                    .HasColumnName("passwordHashSalt");

                entity.Property(e => e.PhoneNumber).HasColumnName("phoneNumber");

                entity.Property(e => e.Role).HasColumnName("role");

                entity.Property(e => e.Username)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("username");
            });

            modelBuilder.Entity<Advertisement>(entity =>
            {
                entity.Property(e => e.AccountId).HasColumnName("accountId");

                entity.Property(e => e.ExpirationDate)
                    .HasColumnType("date")
                    .HasColumnName("expirationDate");

                entity.Property(e => e.Found).HasColumnName("found");

                entity.Property(e => e.Lost).HasColumnName("lost");

                entity.Property(e => e.PublishDate)
                    .HasColumnType("date")
                    .HasColumnName("publishDate");

                entity.Property(e => e.Status).HasColumnName("status");

                entity.HasOne(d => d.Account)
                    .WithMany(p => p.Advertisement)
                    .HasForeignKey(d => d.AccountId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Advertise__accou__267ABA7A");
            });

            modelBuilder.Entity<Category>(entity =>
            {
                entity.Property(e => e.CategoryId).HasColumnName("categoryId");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("name");
            });

            modelBuilder.Entity<Image>(entity =>
            {
                entity.Property(e => e.ImageId).HasColumnName("imageId");

                entity.Property(e => e.Data)
                    .IsRequired()
                    .HasColumnName("data");

                entity.Property(e => e.ItemId).HasColumnName("itemId");

                entity.Property(e => e.Title)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("title");

                entity.HasOne(d => d.Item)
                    .WithMany(p => p.Image)
                    .HasForeignKey(d => d.ItemId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Image__itemId__31EC6D26");
            });

            modelBuilder.Entity<Item>(entity =>
            {
                entity.Property(e => e.ItemId).HasColumnName("itemId");

                entity.Property(e => e.CategoryId).HasColumnName("categoryId");

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasMaxLength(1000)
                    .IsUnicode(false)
                    .HasColumnName("description");

                entity.Property(e => e.FindingDate)
                    .HasColumnType("datetime")
                    .HasColumnName("findingDate");

                entity.Property(e => e.LossDate)
                    .HasColumnType("datetime")
                    .HasColumnName("lossDate");

                entity.Property(e => e.Title)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("title");

                entity.HasOne(d => d.Advertisement)
                    .WithMany(p => p.Item)
                    .HasForeignKey(d => d.AdvertisementId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Item__Advertisem__2E1BDC42");

                entity.HasOne(d => d.Category)
                    .WithMany(p => p.Item)
                    .HasForeignKey(d => d.CategoryId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Item__categoryId__2F10007B");
            });

            modelBuilder.Entity<Message>(entity =>
            {
                entity.Property(e => e.MessageId).HasColumnName("messageId");

                entity.Property(e => e.AccountId).HasColumnName("accountId");

                entity.Property(e => e.Content)
                    .IsRequired()
                    .HasMaxLength(1000)
                    .IsUnicode(false)
                    .HasColumnName("content");

                entity.Property(e => e.IsRead).HasColumnName("isRead");

                entity.Property(e => e.ReadDateTime).HasColumnType("datetime");

                entity.Property(e => e.RecieverId).HasColumnName("recieverId");

                entity.Property(e => e.SendDateTime)
                    .HasColumnType("datetime")
                    .HasColumnName("sendDateTime");

                entity.HasOne(d => d.Account)
                    .WithMany(p => p.Message)
                    .HasForeignKey(d => d.AccountId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Message__account__2B3F6F97");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
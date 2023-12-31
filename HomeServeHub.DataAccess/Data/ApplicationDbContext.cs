﻿using HomeServeHub.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeServeHub.DataAccess.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<TbAppointment> TbAppointments { get; set; }
        public DbSet<TbPaymentDetail> TbPaymentDetails { get; set; }
        public DbSet<TbService> TbServices { get; set; }
        public DbSet<TbServiceProvider> TbServiceProviders { get; set; }
        public DbSet<TbUser> TbUsers { get; set; }
        public DbSet<TbUserType> TbUserTypes { get; set; }
        public DbSet<TbReview> TbReviews { get; set; }
        public DbSet<TbNotification> TbNotifications { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Relationship between TbUser and TbServiceProvider
            modelBuilder.Entity<TbUser>()
                .HasMany(u => u.ServiceProvider)
                .WithOne(sp => sp.User)
                .HasForeignKey(sp => sp.UserID)
                .IsRequired();

            // Relationship between TbUser and TbAppointment
            modelBuilder.Entity<TbUser>()
                .HasMany(u => u.Appointment)
                .WithOne(a => a.User)
                .HasForeignKey(a => a.UserID)
                .IsRequired();

            // Relationship between TbUser and TbPaymentDetail
            modelBuilder.Entity<TbUser>()
                .HasMany(u => u.PaymentDetail)
                .WithOne(pd => pd.User)
                .HasForeignKey(pd => pd.UserID)
                .IsRequired();

            // Relationship between TbUser and TbUserType
            modelBuilder.Entity<TbUser>()
                .HasMany(u => u.UserType)
                .WithOne(sp => sp.User)
                .HasForeignKey(sp => sp.UserID)
                .IsRequired();

            // Relationship between TbUser and TbUserType
            modelBuilder.Entity<TbUser>()
                .HasMany(u => u.Review)
                .WithOne(sp => sp.User)
                .HasForeignKey(sp => sp.UserID)
                .IsRequired();

            // Relationship between TbUser and TbNotification
            modelBuilder.Entity<TbUser>()
                .HasMany(u => u.Notification)
                .WithOne(sp => sp.User)
                .HasForeignKey(sp => sp.UserID)
                .IsRequired();

            // Relationship between TbServiceProvider and TbUser
            modelBuilder.Entity<TbServiceProvider>()
                .HasOne(sp => sp.User)
                .WithMany(u => u.ServiceProvider)
                .HasForeignKey(sp => sp.UserID)
                .IsRequired()
                .OnDelete(DeleteBehavior.NoAction); // Set to NO ACTION to avoid cascade paths

            modelBuilder.Entity<TbServiceProvider>()
                .HasMany(u => u.Notification)
                .WithOne(pd => pd.ServiceProvider)
                .HasForeignKey(pd => pd.UserID)
                .IsRequired();

            // Relationship between TbAppointment and TbServiceProvider
            modelBuilder.Entity<TbAppointment>()
                .HasOne(a => a.ServiceProvider)
                .WithMany(sp => sp.Appointment)
                .HasForeignKey(a => a.ServiceProviderID)
                .IsRequired()
                .OnDelete(DeleteBehavior.NoAction); // Set to NO ACTION to avoid cascade paths

            // Relationship between TbAppointment and TbService
            modelBuilder.Entity<TbAppointment>()
                .HasOne(a => a.Service)
                .WithMany(s => s.Appointment)
                .HasForeignKey(a => a.ServiceID)
                .IsRequired()
                .OnDelete(DeleteBehavior.NoAction); // Set to NO ACTION to avoid cascade paths

            // Relationship between TbAppointment and TbPaymentDetail
            modelBuilder.Entity<TbAppointment>()
                .HasMany(a => a.PaymentDetail)
                .WithOne(pd => pd.Appointment)
                .HasForeignKey(pd => pd.AppointmentID)
                .IsRequired()
                .OnDelete(DeleteBehavior.NoAction); // Set to NO ACTION to avoid cascade paths

            // Relationship between TbReview and TbServiceProvider
            modelBuilder.Entity<TbReview>()
            .HasOne(r => r.ServiceProvider)
            .WithMany()
            .HasForeignKey(r => r.ServiceProviderID);

        }


    }
}


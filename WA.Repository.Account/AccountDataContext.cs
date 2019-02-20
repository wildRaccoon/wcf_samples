﻿using Microsoft.EntityFrameworkCore;
using WA.Repository.Account.Data;

namespace WA.Repository.Account
{
    public class AccountDataContext : DbContext
    {
        public DbSet<UserData> Users { get; set; }
        public DbSet<UserStatusData> UserStatuses { get; set; }
        public DbSet<SessionData> Sessions { get; set; }


        public AccountDataContext(DbContextOptions opt) : base(opt)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //relations
            modelBuilder.Entity<UserData>()
                .HasOne(u => u.Status)
                .WithOne(us => us.User)
                .HasForeignKey<UserStatusData>(us => us.UserId)
                .IsRequired(true)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<UserData>()
                .HasOne(u => u.Session)
                .WithOne(s => s.User)
                .HasForeignKey<SessionData>(s => s.UserId)
                .IsRequired(true)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<SessionData>()
                .HasOne(s => s.User)
                .WithOne(u => u.Session);

            modelBuilder.Entity<UserStatusData>()
                .HasOne(us => us.User)
                .WithOne(u => u.Status);


            //primary keys
            modelBuilder.Entity<UserData>()
                .HasKey(u => u.Id);

            modelBuilder.Entity<UserStatusData>()
                .HasKey(us => us.UserId);

            //only one session per user, even different location
            modelBuilder.Entity<SessionData>()
                .HasKey(us => us.UserId);

            //autogenerated
            modelBuilder.Entity<UserData>()
                .Property(u => u.Id)
                .ValueGeneratedOnAdd();
        }
    }
}

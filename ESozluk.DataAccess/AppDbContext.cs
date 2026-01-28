using ESozluk.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ESozluk.DataAccess
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder) //OnModelCreating, Entity Framework Core'da veritabanı tablolarının nasıl oluşacağını ve birbirleriyle nasıl ilişki kuracağını en ince detayına kadar yapılandırdığın metottur.
        {
            // User -> Entry ilişkisinde Cascade Delete'i kapatıyoruz
            modelBuilder.Entity<Entry>()
                .HasOne(e => e.User)
                .WithMany(u => u.Entries)
                .HasForeignKey(e => e.UserId)
                .OnDelete(DeleteBehavior.Restrict); // kullanıcı silinirse entryi silme

            modelBuilder.Entity<Topic>()
                .HasOne(t => t.User)// bir e
                .WithMany(u => u.Topics)// çok
                .HasForeignKey(t => t.UserId)
                .OnDelete(DeleteBehavior.Restrict);// kullancıı silinirse topiği silme

            modelBuilder.Entity<Like>()
                .HasOne(l => l.User)
                .WithMany(u => u.Likes)
                .HasForeignKey(l => l.UserId)
                .OnDelete(DeleteBehavior.Restrict);// kullanıcı silinirse like i silme
            //.OnDeleteBehaviorCascade

            modelBuilder.Entity<UserRole>()
                .HasKey(ur => new { ur.UserId, ur.RoleId });

            // Kullanıcı ile UserRole ilişkisi
            modelBuilder.Entity<UserRole>()
                .HasOne(ur => ur.User)
                .WithMany(u => u.UserRoles)
                .HasForeignKey(ur => ur.UserId);

            // Rol ile UserRole ilişkisi
            modelBuilder.Entity<UserRole>()
                .HasOne(ur => ur.Role)
                .WithMany(r => r.UserRoles)
                .HasForeignKey(ur => ur.RoleId);

            modelBuilder.Entity<EntryComplaint>()
                .HasOne(ur => ur.User)
                .WithMany(u => u.EntryComplaints)
                .HasForeignKey(ur => ur.UserId);

            modelBuilder.Entity<EntryComplaint>()
               .HasOne(ur => ur.Entry)
               .WithMany(u => u.EntryComplaints)
               .HasForeignKey(ur => ur.EntryId);

            base.OnModelCreating(modelBuilder);
        }
        public DbSet<EntryComplaint> EntryComplaints { get; set; }
        public DbSet<Topic> Topics{ get; set; }
        public DbSet<Like> Likes{get; set;}
        public DbSet<Entry> Entries{get; set;}
        public DbSet<Category> Categories{get; set;}
        public DbSet<User> Users{get; set;}
        public DbSet<Role> Roles { get; set;}
        public DbSet<UserRole> UserRoles { get; set;}

        
    }
}

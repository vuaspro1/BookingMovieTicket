using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using System.Data;
using System.Net.Sockets;
using System.Reflection.Emit;

namespace OrderTicketFilm.Models
{
    public class MyDbContext : DbContext
    {
        public MyDbContext()
        {

        }
        public MyDbContext(DbContextOptions<MyDbContext> options) : base(options) { }

        public virtual DbSet<Ticket> Tickets { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Film> Films { get; set; }
        public virtual DbSet<UserRole> UserRoles { get; set; }
        public virtual DbSet<Role> Roles { get; set; }
        public virtual DbSet<Room> Rooms { get; set; }
        public virtual DbSet<Seat> Seats { get; set; }
        public virtual DbSet<TypeOfFilm> TypeOfFilms { get; set; }
        public virtual DbSet<Customer> Customers { get; set; }
        public virtual DbSet<ShowTime> ShowTimes { get; set; }
        public virtual DbSet<RefreshToken> RefreshTokens { get; set; }
        public virtual DbSet<Bill> Bills { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {


            modelBuilder.Entity<UserRole>()
                .HasKey(pc => new { pc.RoleId, pc.UserId });
            modelBuilder.Entity<UserRole>()
                .HasOne(p => p.Role)
                .WithMany(pc => pc.UserRoles)
                .HasForeignKey(p => p.RoleId);
            modelBuilder.Entity<UserRole>()
                .HasOne(p => p.User)
                .WithMany(pc => pc.UserRoles)
            .HasForeignKey(p => p.UserId);

            //modelBuilder.Entity<ShowTime>()
            //    .HasMany(c => c.Tickets)
            //    .WithOne(e => e.ShowTime);

            ////Entity Ticket
            //modelBuilder.Entity<Ticket>()
            //   .HasOne(c => c.Customer)
            //   .WithMany(t => t.Tickets)
            //   .HasForeignKey(d => d.CustomerId)
            //   .OnDelete(DeleteBehavior.ClientSetNull);

            //modelBuilder.Entity<Ticket>()
            //   .HasOne(c => c.User)
            //   .WithMany(t => t.Tickets)
            //   .HasForeignKey(d => d.UserId)
            //   .OnDelete(DeleteBehavior.ClientSetNull);

            //modelBuilder.Entity<Ticket>()
            //   .HasOne(c => c.ShowTime)
            //   .WithMany(t => t.Tickets)
            //   .HasForeignKey(d => d.ShowTimeId)
            //   .OnDelete(DeleteBehavior.ClientSetNull);

            //modelBuilder.Entity<Ticket>()
            //   .HasOne(c => c.Seat)
            //   .WithMany(t => t.Tickets)
            //   .HasForeignKey(d => d.SeatId)
            //   .OnDelete(DeleteBehavior.ClientSetNull);


        }
    }
}

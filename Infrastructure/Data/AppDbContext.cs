using ClinicManagement.API.Domain.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data
{
    public class AppDbContext : IdentityDbContext<User>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);


            modelBuilder.Entity<RoomSpecialty>().HasKey(x => new { x.SpecialtyId, x.RoomId });

            modelBuilder.Entity<RoomSpecialty>()
                .HasOne(r => r.Room)
                .WithMany(rs => rs.RoomSpecialties)
                .HasForeignKey(r => r.RoomId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<RoomSpecialty>()
                .HasOne(s => s.Specialty)
                .WithMany(rs => rs.RoomSpecialties)
                .HasForeignKey(rs => rs.SpecialtyId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Appointment>()
                .HasOne(p => p.Patient)
                .WithMany(ap => ap.Appointments)
                .HasForeignKey(ap => ap.PatientId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Appointment>()
                .HasOne(d => d.Doctor)
                .WithMany(ap => ap.Appointments)
                .HasForeignKey(ap => ap.DoctorId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Appointment>()
                .HasOne(r => r.Room)
                .WithMany(ap => ap.Appointments)
                .HasForeignKey(ap => ap.RoomId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Doctor>()
                .HasOne(s => s.Specialty)
                .WithMany(d => d.Doctors)
                .HasForeignKey(s => s.SpecialtyId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Doctor>(entity =>
            {
                entity.HasKey(d => d.Id);
                entity.Property(d => d.Id)
                .HasDefaultValueSql("NEWID()");

                entity.HasOne(d => d.User)
                .WithOne()
                .HasForeignKey<Doctor>(d => d.UserId)
                .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<Patient>(entity =>
            {
                entity.HasKey(p => p.Id);
                entity.Property(p => p.Id)
                .HasDefaultValueSql("NEWID()");

                entity.HasOne(p => p.User)
                .WithOne()
                .HasForeignKey<Patient>(p => p.UserId)
                .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<Doctor>
                (
                  p =>
                  {
                      p.Property(n => n.Name).HasColumnType("NVARCHAR").HasMaxLength(100);
                      p.Property(ph => ph.PhoneNumber).HasColumnType("VARCHAR").HasMaxLength(12);
                  }
                );
            modelBuilder.Entity<Patient>
                (
                  p =>
                  {
                      p.Property(n => n.Name).HasColumnType("NVARCHAR").HasMaxLength(100);
                      p.Property(ph => ph.PhoneNumber).HasColumnType("VARCHAR").HasMaxLength(12);
                      p.Property(dob => dob.DateOfBirth).HasColumnType("DATE");
                  }
                );
            modelBuilder.Entity<Room>()
                .Property(rm => rm.RoomNumber).HasColumnType("VARCHAR").HasMaxLength(10);

        }

        public DbSet<Doctor> Doctors { get; set; }
        public DbSet<Patient> Patients { get; set; }
        public DbSet<Appointment> Appointments  { get; set; }
        public DbSet<Specialty> Specialties  { get; set; }
        public DbSet<Room>  Rooms { get; set; }
        public DbSet<RoomSpecialty> RoomSpecialties { get; set; }
        public DbSet<DoctorCertificate> DoctorCertificates { get; set; }
    }
}

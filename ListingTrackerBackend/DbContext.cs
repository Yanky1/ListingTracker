using ListingTracker.DbEntities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace ListingTracker
{
    public class qDbContext : DbContext
    {
        public qDbContext(DbContextOptions<qDbContext> options)
           : base(options)
        {
        }
        public DbSet<Person> People { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<FileUpload> FileUploads { get; set; }
        public DbSet<ActivityLog> ActivityLogs { get; set; }
        public DbSet<AcceptedSourceTracking> SourceTrackings { get; set; }
        public DbSet<AcceptedPerson> AcceptedPeople { get; set; }
        public DbSet<PersonWithFile> PersonWithFile { get; set; }
        public DbSet<AcceptedPersonWithMatchingRecord> AcceptedPersonWithMatchingRecord { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //optionsBuilder.UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=ListingTrackerw;User=testadmin;Password=djDJK%^36^&JE78;Trusted_Connection=True;TrustServerCertificate=true;");
            optionsBuilder.UseSqlServer(@"Data Source=.;Initial Catalog=ListingTrackerw;Integrated Security=True;TrustServerCertificate=true");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PersonWithFile>()
                .HasKey(sc => new { sc.PersonId, sc.FileUploadId }); // composite key

            modelBuilder.Entity<PersonWithFile>()
                .HasOne(sc => sc.Person)
                .WithMany(s => s.PersonWithFileUploads)
                .HasForeignKey(sc => sc.PersonId);

            modelBuilder.Entity<PersonWithFile>()
                .HasOne(sc => sc.FileUpload)
                .WithMany(c => c.FileUploadWithPeople)
                .HasForeignKey(sc => sc.FileUploadId);

            modelBuilder.Entity<AcceptedPersonWithMatchingRecord>()
                .HasKey(sc => new { sc.AcceptedPersonId, sc.PersonId }); // composite key

            modelBuilder.Entity<AcceptedPersonWithMatchingRecord>()
                .HasOne(sc => sc.AcceptedPerson)
                .WithMany(s => s.AcceptedPersonWithMatchingRecords)
                .HasForeignKey(sc => sc.AcceptedPersonId);

            modelBuilder.Entity<AcceptedPersonWithMatchingRecord>()
                .HasOne(sc => sc.Person)
                .WithMany(c => c.AcceptedPersonWithMatchingRecords)
                .HasForeignKey(sc => sc.PersonId);
        }
    }
}

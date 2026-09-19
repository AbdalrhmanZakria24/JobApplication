using JobApplication.Domain.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;


namespace JobApplication.Infrastructure.Persistence
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<Job> Jobs { get; set; }
        public DbSet<Candidate> Candidates { get; set; }
        public DbSet<JobCandidateApplication> JobCandidateApplications { get; set; }
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) 
            : base(options) { }

        protected override void OnModelCreating(ModelBuilder builder) {
            base.OnModelCreating(builder);
            builder.Entity<Job>().HasKey(x => x.Id);
            builder.Entity<Job>().Property(x => x.Title).IsRequired().HasMaxLength(200); 
            
            builder.Entity<Job>().Property(x => x.Description).IsRequired();
            builder.Entity<Job>().Property(x => x.RecruiterId).IsRequired();
        }
    }
}

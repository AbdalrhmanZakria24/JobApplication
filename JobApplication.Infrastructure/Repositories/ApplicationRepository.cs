using JobApplication.Application.Interfaces;
using JobApplication.Domain.Entities;
using JobApplication.Infrastructure.Persistence;

namespace JobApplication.Infrastructure.Repositories
{
    public class ApplicationRepository : IApplicationRepository
    {
        private readonly ApplicationDbContext _context;

        public ApplicationRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task InsertAsync(JobCandidateApplication job)
        {
            await _context.JobCandidateApplications.AddAsync(job);
        }
        public void Update(JobCandidateApplication job)
        {
            _context.JobCandidateApplications.Update(job);
        }
        public IQueryable<JobCandidateApplication> Get()
        {
            var jobs = _context.JobCandidateApplications.AsQueryable();
            return jobs;
        }
        public JobCandidateApplication GetOne(int id)
        {
            var jobs = _context.JobCandidateApplications.AsQueryable();

            jobs=jobs.Where(x => x.Id == id);
            return jobs.FirstOrDefault()!;
        }
        public void Remove(JobCandidateApplication job)
        {
            _context.JobCandidateApplications.Remove(job);
        }
        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}

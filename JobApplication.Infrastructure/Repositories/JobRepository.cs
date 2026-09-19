using JobApplication.Application.Interfaces;
using JobApplication.Domain.Entities;
using JobApplication.Infrastructure.Persistence;

namespace JobApplication.Infrastructure.Repositories
{
    public class JobRepository : IJobRepository
    {
        private readonly ApplicationDbContext _context;

        public JobRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Job?> GetOneAsync(int id)
        {
            return await _context.Jobs.FindAsync(id);
        }

        public async Task UpdateAsync(Job job)
        {
            _context.Jobs.Update(job);

            await _context.SaveChangesAsync();
        }

        public async Task InsertAsync(Job job)
        {
            await _context.Jobs.AddAsync(job);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}

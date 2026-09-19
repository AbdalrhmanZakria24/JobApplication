using JobApplication.Application.Interfaces;
using JobApplication.Domain.Entities;

namespace JobApplication.Application.Services
{
    public class CloseJob
    {
        private readonly IJobRepository _jobRepository;

        public CloseJob(IJobRepository jobRepository)
        {
            _jobRepository = jobRepository;
        }

        public async Task CloseAsync(int jobId, int recruiterId)
        {
            var job = await _jobRepository.GetOneAsync(jobId);

            if (job is null)
            {
                throw new KeyNotFoundException(
                    $"Job with id {jobId} was not found.");
            }

            if (job.RecruiterId != recruiterId)
            {
                throw new UnauthorizedAccessException(
                    "You are not authorized to close this job.");
            }

            if (!job.IsActive)
            {
                throw new InvalidOperationException(
                    "This job is already closed.");
            }

            job.IsActive = false;
            job.ClosedAt = DateTime.UtcNow;
            job.ClosedBy = recruiterId;

            await _jobRepository.UpdateAsync(job);
        }
    }
}

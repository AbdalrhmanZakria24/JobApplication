using JobApplication.Application.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace JobApplication.Application.CQRS.Command.CLoseJob
{
    public class CloseJobCommandJandler : IRequestHandler<CloseJobCOmmand, int>
    {
        private readonly IJobRepository _jobRepository;

        public CloseJobCommandJandler(IJobRepository jobRepository)
        {
            _jobRepository = jobRepository;
        }
        public async Task<int> Handle(CloseJobCOmmand request, CancellationToken cancellationToken)
        {
            var job = await _jobRepository.GetOneAsync(request.jobId);

            if (job is null)
            {
                throw new KeyNotFoundException(
                    $"Job with id {request.jobId} was not found.");
            }

            if (job.RecruiterId != request.recruiterId)
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
            job.ClosedBy = request.recruiterId;

            await _jobRepository.UpdateAsync(job);

            return  job.RecruiterId;
        }
    }
}


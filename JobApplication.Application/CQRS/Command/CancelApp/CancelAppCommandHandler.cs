using JobApplication.Application.Interfaces;
using JobApplication.Domain.Entities;
using JobApplication.Domain.Enums;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace JobApplication.Application.CQRS.Command.CancelApp
{
    public class CancelAppCommandHandler : IRequestHandler<CancelAppCommand, int>
    {
        private readonly IApplicationRepository _applicationRepository;

        public CancelAppCommandHandler(IApplicationRepository applicationRepository)
        {
            _applicationRepository = applicationRepository;
        }
        public async Task<int> Handle(CancelAppCommand request, CancellationToken cancellationToken)
        {
            var application = _applicationRepository.GetOne(request.Id);

            if (application is null)
                throw new Exception("Application not found.");

            if (application.CandidateId != request.candidateId)
                throw new UnauthorizedAccessException(
                    "You cannot cancel this application.");

            if (application.JobApplicationStatus != JobApplicationStatus.Applied &&
               application.JobApplicationStatus != JobApplicationStatus.UnderReview)
            {
                throw new InvalidOperationException(
                    "Application cannot be cancelled in its current status.");
            }

            application.JobApplicationStatus = JobApplicationStatus.Cancelled;

            application.StatusUpdatedAt = DateTime.UtcNow;

            _applicationRepository.Update(application);

            await _applicationRepository.SaveChangesAsync();

            return application.Id;
        }
    }
}

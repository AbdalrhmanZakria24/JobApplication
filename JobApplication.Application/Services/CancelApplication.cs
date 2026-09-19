using JobApplication.Application.Interfaces;
using JobApplication.Domain.Entities;
using JobApplication.Domain.Enums;

namespace JobApplication.Application.Services
{
    public class CancelApplication :ICancelApplication
    {
        private readonly IApplicationRepository _applicationRepository;

        public CancelApplication(IApplicationRepository applicationRepository) 
        {
            _applicationRepository = applicationRepository;
        }

        public async Task Cancel(int Id, int candidateId)
        {
            var application =  _applicationRepository.GetOne(Id);

            if (application is null)
                throw new Exception("Application not found.");

            if (application.CandidateId != candidateId)
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

        }
    }
}

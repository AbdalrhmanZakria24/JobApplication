using JobApplication.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace JobApplication.Application.Interfaces
{
    public interface IApplicationRepository
    {
        Task InsertAsync(JobCandidateApplication job);
        void Update(JobCandidateApplication job);
        IQueryable<JobCandidateApplication> Get();
        public JobCandidateApplication GetOne(int id);
        void Remove(JobCandidateApplication job);
        Task SaveChangesAsync();
    }
}

using JobApplication.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace JobApplication.Application.Interfaces
{
    public interface IJobRepository
    {

        public  Task<Job?> GetOneAsync(int id);

        public  Task UpdateAsync(Job job);
        public Task InsertAsync(Job job);
        public Task SaveChangesAsync();
    }
}

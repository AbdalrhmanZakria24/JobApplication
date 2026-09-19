using System;
using System.Collections.Generic;
using System.Text;

namespace JobApplication.Application.Interfaces
{
    public interface ICancelApplication
    {
        public Task Cancel(int Id, int candidateId);
    }
}

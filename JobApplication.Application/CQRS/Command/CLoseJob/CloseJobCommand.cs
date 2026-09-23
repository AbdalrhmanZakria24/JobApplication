using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace JobApplication.Application.CQRS.Command.CLoseJob
{
    public record CloseJobCOmmand(int jobId, int recruiterId) : IRequest<int>;
}

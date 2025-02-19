using ACA.Domain.Common;
using ACA.Domain.Entities.Workers;
using ACA.Domain.Entities.Processes;

namespace ACA.Domain.Relations
{
    public class Process_Supervisor : Entity
        {
            public Guid SupervisorId { get; set; }

            public Supervisor Supervisor { get; set; }

            public Guid ProcessId { get; set; }

            public Process Process { get; set; }
        }
}


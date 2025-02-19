using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ACA.Application.Abstract;

namespace ACA.Application.Commads.Delete.DeleteSupervisor
{
    public record DeleteSupervisorCommand(Guid Id) : ICommand;
}

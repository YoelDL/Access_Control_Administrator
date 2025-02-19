using System;
using System.Collections.Generic;
using System.Linq;
using ACA.Contracts.Workers;
using ACA.DataAccess.Contexts;
using ACA.DataAccess.Repositories.Common;
using ACA.Domain.Entities.Workers; 
using Microsoft.EntityFrameworkCore;

public class SupervisorRepository : RepositoryBase<Supervisor>, ISupervisorRepository
{
    public SupervisorRepository(AplicationContext context) : base(context) { }

    public void AddSupervisor(Supervisor supervisor) => AddAsync(supervisor).Wait();

    public Supervisor? GetSupervisorById(Guid id) => GetByIdAsync(id).Result;

    public IEnumerable<T> GetAllSupervisors<T>() where T : Supervisor
        => GetAllAsync().Result as IEnumerable<T>;

    public void UpdateSupervisor(Supervisor supervisor) => UpdateAsync(supervisor).Wait();

    public void DeleteSupervisor(Supervisor supervisor) => DeleteAsync(supervisor.Id).Wait();

    public T GetSupervisorById<T>(Guid id)
    {
        throw new NotImplementedException();
    }
}


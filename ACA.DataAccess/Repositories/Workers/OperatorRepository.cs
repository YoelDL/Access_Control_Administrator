using System;
using System.Collections.Generic;
using System.Linq;
using ACA.Contracts.Workers;
using ACA.DataAccess.Contexts;
using ACA.DataAccess.Repositories.Common;
using ACA.Domain.Entities.Workers; 
using Microsoft.EntityFrameworkCore;

public class OperatorRepository : RepositoryBase<Operator>, IOperatorRepository
{
    public OperatorRepository(AplicationContext context) : base(context) { }

    public void AddOperator(Operator operador) => AddAsync(operador).Wait();

    public Operator? GetOperatorById(Guid id) => GetByIdAsync(id).Result;

    public T? GetOperatorById<T>(Guid id) where T : Operator
    {
        return GetAllAsync().Result.OfType<T>().FirstOrDefault(o => o.Id == id);
    }

    public IEnumerable<T> GetAllOperators<T>() where T : Operator
        => GetAllAsync().Result as IEnumerable<T>;

    public void UpdateOperator(Operator operador) => UpdateAsync(operador).Wait();

    public void DeleteOperator(Operator operador) => DeleteAsync(operador.Id).Wait();
}


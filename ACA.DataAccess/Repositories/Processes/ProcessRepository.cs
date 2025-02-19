using ACA.Domain.Entities.Processes;
using ACA.DataAccess.Contexts;
using ACA.Application.Contracts.Processes;

namespace ACA.DataAccess.Repositories
{
    public class ProcessRepository : RepositoryBase<Process>, IProcessRepository
    {
        public ProcessRepository(AplicationContext context) : base(context) { }

        public void AddProcess(Process process)
            => AddAsync(process).Wait();

        public T? GetProcessById<T>(Guid id) where T : Process
            => GetByIdAsync(id).Result as T;

        public IEnumerable<T> GetAllProcesses<T>() where T : Process
            => GetAllAsync().Result as IEnumerable<T>;

        public void UpdateProcess(Process process)
            => UpdateAsync(process).Wait();

        public void DeleteProcess(Process process)
            => DeleteAsync(process.Id).Wait();

    }
}





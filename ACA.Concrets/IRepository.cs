namespace ACA.Application.Contracts
{
    public interface IRepository<T> where T : class
    {
        Task<T> GetByIdAsync(Guid id);
        Task<IEnumerable<T>> GetAllAsync();
        Task<T> AddAsync(T entity);
        Task UpdateAsync(T entity);
        Task DeleteAsync(T entity);

        //Opcional:  Añadir métodos para paginación, filtrado, etc.
        IQueryable<T> GetQueryable(); // Para consultas más complejas
    }
}

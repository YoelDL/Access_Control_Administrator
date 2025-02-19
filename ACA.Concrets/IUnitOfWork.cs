namespace ACA.Contracts
{
    /// <summary>
    /// Define las propiedades y funcionalidades de un elemento de
    /// acceso a datos que utiliza el patrón Unit of Work.
    /// </summary>
    public interface IUnitOfWork
    {
        void SaveChanges();
        Task SaveChangesAsync();
    }
}

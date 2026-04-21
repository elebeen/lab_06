namespace lab_06.Repositories;

public interface IUnitOfWork
{
    IRepository<TEntity> Repository<TEntity>() where TEntity : class;
    Task<int> SaveChanges();
}
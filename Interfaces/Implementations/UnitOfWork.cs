using System.Collections;
using lab_06.Models;

namespace lab_06.Interfaces.Implementations;

public class UnitOfWork : IUnitOfWork
{
    private Hashtable _repositories;
    private readonly context _context;
    
    public UnitOfWork(context context)
    {
        _repositories = new Hashtable();
        _context = context;
    }

    public Task<int> SaveChanges()
    {
        return _context.SaveChangesAsync();
    }

    public IRepository<TEntity> Repository<TEntity>() where TEntity : class
    {
        var type = typeof(TEntity).Name;
        
        if (_repositories.ContainsKey(type))
        {
            return (IRepository<TEntity>)_repositories[type];
        }

        var repoType = typeof(Repository<>);
        var repoInstance = Activator.CreateInstance(repoType.MakeGenericType(typeof(TEntity)), _context);

        if (repoInstance != null)
        {
            _repositories.Add(type, repoInstance);
            return (IRepository<TEntity>)repoInstance;
        }
        
        throw new Exception($"Repository {type} can not be created.");
    }
}
using lab_06.Models;
using Microsoft.EntityFrameworkCore;

namespace lab_06.Repositories.Implementations;

public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
{
    private readonly Context _context;

    public Repository(Context context)
    {
        _context = context;
    }

    public IEnumerable<TEntity> FindAll()
    {
        return _context.Set<TEntity>().ToList();
    }

    public TEntity FindById(int id)
    {
        return _context.Set<TEntity>().Find(id);
    }

    public TEntity FindByName(string name)
    {
        return _context.Set<TEntity>().FirstOrDefault(e => EF.Property<string>(e, "Name") == name);
    }

    public void Add(TEntity entity)
    {
        _context.Set<TEntity>().Add(entity);
    }

    public void Update(TEntity entity)
    {
        _context.Set<TEntity>().Update(entity);
    }

    public void Delete(TEntity entity)
    {
        _context.Set<TEntity>().Remove(entity);
    }
}
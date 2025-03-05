using EduFlow.DAL.Data;
using EduFlow.DAL.Interfaces;
using EduFlow.Domain.Entities.Base;
using Microsoft.EntityFrameworkCore;

namespace EduFlow.DAL.Repositories;

public class Repository<TEntity> : IRepository<TEntity> where TEntity : BaseEntity
{
    private DbSet<TEntity> _dbSet;
    private AppDbContext _context;
    public Repository(AppDbContext context)
    {
        this._dbSet = _context.Set<TEntity>();
        this._context = context;
    }
    public async Task<long> AddAsync(TEntity entity)
    {
        await _dbSet.AddAsync(entity);
        await _context.SaveChangesAsync();

        return entity.Id;
    }

    public async Task<bool> AddConfirmAsync(TEntity entity)
    {
        await _dbSet.AddAsync(entity);
        var res = await _context.SaveChangesAsync();

        return res > 0;
    }

    public async Task<bool> AddRangeAsync(IEnumerable<TEntity> entities)
    {
        await _dbSet.AddRangeAsync(entities);
        var res = await _context.SaveChangesAsync();

        return res > 0;
    }

    public async Task<bool> DeleteAsync(TEntity entity)
    {
        _dbSet.Remove(entity);
        var res = await _context.SaveChangesAsync();

        return res > 0;
    }

    public IQueryable<TEntity> GetAllAsync()
        => _dbSet.AsQueryable();

    public async Task<TEntity> GetAsync(long id)
    {
        TEntity? entity = await _dbSet.FirstOrDefaultAsync(x => x.Id == id);
        if (entity is null)
            return null;

        return entity;
    }

    public async Task<bool> UpdateAsync(TEntity entity)
    {
        _dbSet.Update(entity);
        var res = await _context.SaveChangesAsync();

        return res > 0;
    }

    public async Task<bool> UpdateRangeAsync(IEnumerable<TEntity> entities)
    {
        _dbSet.UpdateRange(entities);
        var res = await _context.SaveChangesAsync();

        return res > 0;
    }
}

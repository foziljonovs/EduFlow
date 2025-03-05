using EduFlow.Domain.Entities.Base;

namespace EduFlow.DAL.Interfaces;
public interface IRepository<TEntity> where TEntity : BaseEntity
{
    Task<bool> AddConfirmAsync(TEntity entity);
    Task<long> AddAsync(TEntity entity);
    Task<bool> AddRangeAsync(IEnumerable<TEntity> entities);
    Task<bool> DeleteAsync(TEntity entity);
    Task<bool> UpdateAsync(TEntity entity);
    Task<bool> UpdateRangeAsync(IEnumerable<TEntity> entities);
    Task<TEntity?> GetAsync(long id);
    IQueryable<TEntity> GetAllAsync();
}

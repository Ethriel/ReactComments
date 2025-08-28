using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace ReactComments.Services.Abstraction
{
    public interface IEntityService<T> where T : class
    {
        T Create(T entity);
        Task<T> CreateAsync(T entity);
        IQueryable<T> Read();
        Task<IQueryable<T>> ReadAsync();
        T? Update(T oldEntity, T newEntity);
        Task<T?> UpdateAsync(T oldEntity, T newEntity);
        bool Delete(object id);
        Task<bool> DeleteAsync(object id);
    }
}

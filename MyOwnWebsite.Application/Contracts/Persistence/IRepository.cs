using System.Linq.Expressions;
using MyOwnWebsite.Domain;

namespace MyOwnWebsite.Application.Contracts.Persistence;

public interface IRepository<T> where T : Entity
{
    Task AddAsync(T entity);
    Task<IReadOnlyList<T>?> GetAllAsync();
    Task<T?> GetAsync(Guid id);
    Task<T?> GetAsync(Expression<Func<T, bool>> criteria);
    Task RemoveAsync(T entity);
    Task Update(T entity);



}
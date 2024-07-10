using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using MyOwnWebsite.Application.Contracts.Persistence;
using MyOwnWebsite.Application.Dtos;
using MyOwnWebsite.Domain;


namespace MyOwnWebsite.Persistence.Repositories;

public class Repository<T>(ApplicationDbContext context)
: IRepository<T> where T : Entity
{
    public async Task AddAsync(T entity)
    {
        await context.Set<T>().AddAsync(entity);
    }

    public Task Update(T entity)
    {
        context.Set<T>().Update(entity);
        return Task.CompletedTask;
    }

    public async Task<IReadOnlyList<T>?> GetAllAsync()
    {
        return await context.Set<T>().ToListAsync();
    }

    public async Task<T?> GetAsync(Guid id)
    {
        return await context.Set<T>().FindAsync(id);
    }

    public Task RemoveAsync(T entity)
    {
        context.Set<T>().Remove(entity);
        return Task.CompletedTask;
    }

    protected async Task<PaginationResponseDto<TEntity>> Paged<TEntity>(IQueryable<TEntity> query, int pageNumber, int pageSize)
    {
        var count = await query.CountAsync();

        var pagedResult = await query
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return new(pagedResult, count);
    }

    public async Task<T?> GetAsync(Expression<Func<T, bool>> criteria)
    {
        return await context.Set<T>().FirstOrDefaultAsync(criteria);
    }
}

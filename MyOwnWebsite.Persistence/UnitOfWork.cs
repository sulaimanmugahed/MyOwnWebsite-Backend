

using MyOwnWebsite.Application.Contracts.Persistence;

namespace MyOwnWebsite.Persistence;


public class UnitOfWork(ApplicationDbContext context) : IUnitOfWork
{
    public async Task<bool> SaveChangesAsync()
    {
        return await context.SaveChangesAsync() > 0;
    }

    public bool SaveChanges()
    {
        return context.SaveChanges() > 0;
    }
}
namespace MyOwnWebsite.Application.Contracts.Persistence;


public interface IUnitOfWork
{
    Task<bool> SaveChangesAsync();
    bool SaveChanges();
}
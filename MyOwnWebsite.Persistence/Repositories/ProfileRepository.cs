using MyOwnWebsite.Application.Contracts.Persistence;
using MyOwnWebsite.Domain.Profiles;

namespace MyOwnWebsite.Persistence.Repositories;

public class ProfileRepository(ApplicationDbContext context) : Repository<Profile>(context), IProfileRepository
{

}

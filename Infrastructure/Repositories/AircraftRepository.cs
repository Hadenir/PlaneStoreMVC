using Microsoft.EntityFrameworkCore;
using PlaneStore.Domain.Entities;
using PlaneStore.Domain.Repositories;
using PlaneStore.Infrastructure.Data;

namespace PlaneStore.Infrastructure.Repositories
{
    internal class AircraftRepository : IAircraftRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public AircraftRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IQueryable<Aircraft> Aircraft => _dbContext.Aircraft;
    }
}

using Microsoft.EntityFrameworkCore;
using PlaneStore.Domain.Entities;
using PlaneStore.Domain.Repositories;
using PlaneStore.Infrastructure.Data;

namespace PlaneStore.Infrastructure.Repositories
{
    public class ManufacturerRepository : IManufacturerRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public ManufacturerRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IQueryable<Manufacturer> Manufacturers => _dbContext.Manufacturers.Include(m => m.ProducedAircraft);
    }
}

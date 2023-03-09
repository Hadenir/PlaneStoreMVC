using Microsoft.EntityFrameworkCore;
using PlaneStore.Domain.Entities;
using PlaneStore.Domain.Repositories;
using PlaneStore.Infrastructure.Data;

namespace PlaneStore.Infrastructure.Repositories
{
    internal class ManufacturerRepository : GenericRepository<Manufacturer>, IManufacturerRepository
    {
        public ManufacturerRepository(ApplicationDbContext context) : base(context)
        { }

        public override Manufacturer? GetById(Guid? id)
            => dbSet.Include(m => m.ProducedAircraft).FirstOrDefault(m => m.Id == id);
    }
}

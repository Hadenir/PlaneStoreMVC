using Microsoft.EntityFrameworkCore;
using PlaneStore.Domain.Entities;
using PlaneStore.Domain.Repositories;
using PlaneStore.Infrastructure.Data;

namespace PlaneStore.Infrastructure.Repositories
{
    internal class AircraftRepository : GenericRepository<Aircraft>, IAircraftRepository
    {
        public AircraftRepository(ApplicationDbContext context) : base(context)
        {}

        public override Aircraft? GetById(Guid? id)
            => dbSet.Include(a => a.Manufacturer).FirstOrDefault(a => a.Id == id);
    }
}

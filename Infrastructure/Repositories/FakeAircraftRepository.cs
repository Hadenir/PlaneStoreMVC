using PlaneStore.Domain.Entities;
using PlaneStore.Domain.Repositories;

namespace PlaneStore.Infrastructure.Repositories
{
    internal class FakeAircraftRepository : IAircraftRepository
    {
        public IQueryable<Aircraft> Aircraft => new List<Aircraft>
        {
            new Aircraft { Name = "Airbus A320neo", Price = 110_600_000 },
            new Aircraft { Name = "Boeing 737-900", Price = 94_600_000 },
            new Aircraft { Name = "Antonov An-225", Price = 500_000_000 },
        }.AsQueryable();
    }
}

using PlaneStore.Domain.Entities;

namespace PlaneStore.Domain.Repositories
{
    public interface IAircraftRepository
    {
        IQueryable<Aircraft> Aircraft { get; }
    }
}

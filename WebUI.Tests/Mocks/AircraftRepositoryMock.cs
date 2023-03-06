using PlaneStore.Domain.Entities;
using PlaneStore.Domain.Repositories;

namespace PlaneStore.WebUI.Tests.Mocks
{
    internal class AircraftRepositoryMock : GenericRepositoryMock<Aircraft>, IAircraftRepository
    {
        public AircraftRepositoryMock(IEnumerable<Aircraft> entities, Func<Aircraft, Guid> primaryKey) : base(entities, primaryKey)
        {}
    }
}

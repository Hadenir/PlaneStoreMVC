using PlaneStore.Domain.Entities;
using PlaneStore.Domain.Repositories;

namespace PlaneStore.WebUI.Tests.Mocks
{
    internal class ManufacturerRepositoryMock : GenericRepositoryMock<Manufacturer>, IManufacturerRepository
    {
        public ManufacturerRepositoryMock(IEnumerable<Manufacturer> entities)
            : base(entities, m => m.Id)
        { }
    }
}

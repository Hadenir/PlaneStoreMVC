using PlaneStore.Domain.Entities;
using PlaneStore.Domain.Repositories;

namespace PlaneStore.WebUI.Tests.Mocks
{
    internal class ManufacturerRepositoryMock : GenericRepositoryMock<Manufacturer>, IManufacturerRepository
    {
        public ManufacturerRepositoryMock(IEnumerable<Manufacturer> entities, Func<Manufacturer, Guid> primaryKey) : base(entities, primaryKey)
        { }
    }
}

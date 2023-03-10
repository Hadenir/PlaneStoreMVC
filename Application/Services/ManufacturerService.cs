using PlaneStore.Application.Utilities;
using PlaneStore.Domain.Repositories;

namespace PlaneStore.Application.Services
{
    public interface IManufacturerService
    {
        void RemoveManufacturer(Guid manufacturerId);
    }

    internal class ManufacturerService : IManufacturerService
    {
        private readonly IManufacturerRepository _manufacturerRepository;

        public ManufacturerService(IManufacturerRepository manufacturerRepository)
        {
            _manufacturerRepository = manufacturerRepository;
        }

        public void RemoveManufacturer(Guid manufacturerId)
        {
            var manufacturer = _manufacturerRepository.GetById(manufacturerId);

            if (manufacturer is null)
            {
                throw new OperationException("Cannot remove nonexisting manufacturer");
            }
            if (manufacturer.ProducedAircraft.Any())
            {
                throw new OperationException("Cannot remove manufacturer relating to existing aircraft");
            }

            _manufacturerRepository.Remove(manufacturer);
            _manufacturerRepository.Commit();
        }
    }
}

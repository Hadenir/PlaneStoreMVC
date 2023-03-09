using PlaneStore.Domain.Repositories;

namespace PlaneStore.Application.Services
{
    public interface IManufacturerService
    {
        bool RemoveManufacturer(Guid manufacturerId);
    }

    internal class ManufacturerService : IManufacturerService
    {
        private readonly IManufacturerRepository _manufacturerRepository;

        public ManufacturerService(IManufacturerRepository manufacturerRepository)
        {
            _manufacturerRepository = manufacturerRepository;
        }

        public bool RemoveManufacturer(Guid manufacturerId)
        {
            var manufacturer = _manufacturerRepository.GetById(manufacturerId);
            if (manufacturer is null || manufacturer.ProducedAircraft.Any())
            {
                return false;
            }

            _manufacturerRepository.Remove(manufacturer);
            _manufacturerRepository.Commit();

            return true;
        }
    }
}

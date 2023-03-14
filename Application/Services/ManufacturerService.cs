using Microsoft.EntityFrameworkCore;
using PlaneStore.Application.Utilities;
using PlaneStore.Domain.DataAccess;
using PlaneStore.Domain.Entities;

namespace PlaneStore.Application.Services
{
    public interface IManufacturerService
    {
        IQueryable<Manufacturer> GetManufacturers();
        Manufacturer? GetManufacturerById(Guid? id);
        Guid AddManufacturer(Manufacturer manufacturer);
        void UpdateManufacturer(Manufacturer manufacturer);
        void RemoveManufacturerById(Guid id);
    }

    internal class ManufacturerService : IManufacturerService
    {
        private readonly IRepository<Manufacturer> _manufacturerRepository;

        public ManufacturerService(IRepository<Manufacturer> manufacturerRepository)
        {
            _manufacturerRepository = manufacturerRepository;
        }

        public IQueryable<Manufacturer> GetManufacturers()
            => _manufacturerRepository.GetAll()
                .Include(m => m.ProducedAircraft);

        public Manufacturer? GetManufacturerById(Guid? id)
            => GetManufacturers().FirstOrDefault(m => m.Id == id);

        public Guid AddManufacturer(Manufacturer manufacturer)
        {
            if (!manufacturer.Name.Any())
            {
                throw new ServiceException("Cannot create manufacturer with empty name");
            }

            _manufacturerRepository.Add(manufacturer);
            _manufacturerRepository.Commit();

            return manufacturer.Id;
        }

        public void UpdateManufacturer(Manufacturer manufacturer)
        {
            if (manufacturer.Id == Guid.Empty)
            {
                throw new ServiceException("Cannot update manufacturer without specified id");
            }

            _manufacturerRepository.Update(manufacturer);
            _manufacturerRepository.Commit();
        }

        public void RemoveManufacturerById(Guid id)
        {
            var manufacturer = GetManufacturerById(id) ?? throw new ServiceException("Cannot remove nonexisting manufacturer");

            if (manufacturer.ProducedAircraft.Any())
            {
                throw new ServiceException("Cannot remove manufacturer relating to existing aircraft");
            }

            _manufacturerRepository.Remove(manufacturer);
            _manufacturerRepository.Commit();
        }
    }
}

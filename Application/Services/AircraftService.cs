using Microsoft.EntityFrameworkCore;
using PlaneStore.Application.Utilities;
using PlaneStore.Domain;
using PlaneStore.Domain.Entities;

namespace PlaneStore.Application.Services
{
    public interface IAircraftService
    {
        IQueryable<Aircraft> GetAircraft();
        Aircraft? GetAircraftById(Guid? id);
        Guid AddAircraft(Aircraft aircraft);
        void UpdateAircraft(Aircraft aircraft);
        void RemoveAircraftById(Guid id);
    }

    internal class AircraftService : IAircraftService
    {
        private readonly IRepository<Aircraft> _aircraftRepository;

        public AircraftService(IRepository<Aircraft> aircraftRepository)
        {
            _aircraftRepository = aircraftRepository;
        }

        public IQueryable<Aircraft> GetAircraft()
            => _aircraftRepository.GetAll()
                .Include(a => a.Manufacturer);

        public Aircraft? GetAircraftById(Guid? id)
            => GetAircraft().FirstOrDefault(a => a.Id == id);

        public Guid AddAircraft(Aircraft aircraft)
        {
            if (!aircraft.Name.Any())
            {
                throw new ServiceException("Cannot create aircraft with empty name");
            }
            if (aircraft.Price < 0)
            {
                throw new ServiceException("Cannot create aircraft with negative price");
            }
            if (aircraft.ManufacturerId == Guid.Empty)
            {
                throw new ServiceException("Cannot create aircraft without specified manufacturer");
            }

            _aircraftRepository.Add(aircraft);
            _aircraftRepository.Commit();

            return aircraft.Id;
        }

        public void UpdateAircraft(Aircraft aircraft)
        {
            if (aircraft.Id == Guid.Empty)
            {
                throw new ServiceException("Cannot update aircraft without specified id");
            }

            _aircraftRepository.Update(aircraft);
            _aircraftRepository.Commit();
        }

        public void RemoveAircraftById(Guid id)
        {
            var manufacturer = GetAircraftById(id) ?? throw new ServiceException("Cannot remove nonexisting aircraft");

            _aircraftRepository.Remove(manufacturer);
            _aircraftRepository.Commit();
        }
    }
}

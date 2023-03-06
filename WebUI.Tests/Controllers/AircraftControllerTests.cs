﻿using Microsoft.AspNetCore.Mvc;
using PlaneStore.Domain.Entities;
using PlaneStore.Domain.Repositories;
using PlaneStore.WebUI.Controllers;
using PlaneStore.WebUI.Models;
using PlaneStore.WebUI.Tests.Mocks;
using Xunit;

namespace PlaneStore.WebUI.Tests.Controllers
{
    public class AircraftControllerTests
    {
        private readonly IManufacturerRepository _manufacturerRepository;
        private readonly IAircraftRepository _aircraftRepository;

        private readonly AircraftController _controller;

        public AircraftControllerTests()
        {
            var manufacturers = new[]
            {
                new Manufacturer { Id = Guid.NewGuid(), Name = "M1" },
                new Manufacturer { Id = Guid.NewGuid(), Name = "M2" },
                new Manufacturer { Id = Guid.NewGuid(), Name = "M3" },
            }.OrderBy(m => m.Id).ToList();

            _manufacturerRepository = new ManufacturerRepositoryMock(manufacturers, m => m.Id);

            var aircraft = new[]
            {
                new Aircraft { Id = Guid.NewGuid(), Name = "A1", ManufacturerId = manufacturers[0].Id },
                new Aircraft { Id = Guid.NewGuid(), Name = "A2", ManufacturerId = manufacturers[0].Id },
                new Aircraft { Id = Guid.NewGuid(), Name = "A3", ManufacturerId = manufacturers[1].Id },
                new Aircraft { Id = Guid.NewGuid(), Name = "A4", ManufacturerId = manufacturers[2].Id },
                new Aircraft { Id = Guid.NewGuid(), Name = "A5", ManufacturerId = manufacturers[1].Id },
            }.OrderBy(a => a.Id).ToList();

            _aircraftRepository = new AircraftRepositoryMock(aircraft, a => a.Id);

            _controller = new AircraftController(_aircraftRepository, _manufacturerRepository);
        }

        [Fact]
        public void Can_Paginate()
        {
            var aircraft = _aircraftRepository.GetAll().ToList();
            _controller.PageSize = 3;

            var result = (_controller.List(page: 2) as ViewResult)?.Model as AircraftListViewModel;

            List<Aircraft> resultAircraft = result!.Aircraft.ToList();

            Assert.Equal(2, resultAircraft.Count);
            Assert.Equal(aircraft[3].Id, resultAircraft[0].Id);
            Assert.Equal(aircraft[4].Id, resultAircraft[1].Id);
        }

        [Fact]
        public void Can_Send_Paginated_View_Model()
        {
            _controller.PageSize = 3;

            var result = (_controller.List(page: 2) as ViewResult)?.Model as AircraftListViewModel;

            PagingInfo pagingInfo = result!.PagingInfo;
            Assert.Equal(2, pagingInfo.CurrentPage);
            Assert.Equal(3, pagingInfo.ItemsPerPage);
            Assert.Equal(5, pagingInfo.TotalItems);
            Assert.Equal(2, pagingInfo.TotalPages);
        }

        [Fact]
        public void Can_Filter_By_Manufacturer()
        {
            var manufacturers = _manufacturerRepository.GetAll().ToList();
            _controller.PageSize = 3;

            var result = (_controller.List(manufacturers[1].Id) as ViewResult)?.Model as AircraftListViewModel;

            List<Aircraft> resultAircraft = result!.Aircraft.ToList();

            Assert.Equal(2, resultAircraft.Count);
            Assert.Equal(manufacturers[1].Id, resultAircraft[0].ManufacturerId);
            Assert.Equal(manufacturers[1].Id, resultAircraft[1].ManufacturerId);
        }
    }
}

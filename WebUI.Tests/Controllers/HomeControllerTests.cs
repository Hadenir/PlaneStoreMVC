﻿using Microsoft.AspNetCore.Mvc;
using PlaneStore.Domain.Entities;
using PlaneStore.Domain.Repositories;
using PlaneStore.WebUI.Controllers;
using PlaneStore.WebUI.Models;
using PlaneStore.WebUI.Tests.Mocks;
using Xunit;

namespace PlaneStore.WebUI.Tests.Controllers
{
    public class HomeControllerTests
    {
        private readonly IManufacturerRepository _manufacturerRepository;
        private readonly IAircraftRepository _aircraftRepository;

        private readonly HomeController _controller;

        public HomeControllerTests()
        {
            var manufacturers = new[]
            {
                new Manufacturer { Name = "M1" },
                new Manufacturer { Name = "M2" },
                new Manufacturer { Name = "M3" },
            };

            _manufacturerRepository = new ManufacturerRepositoryMock(manufacturers);

            var aircraft = new[]
            {
                new Aircraft { Name = "A1", ManufacturerId = manufacturers[0].Id },
                new Aircraft { Name = "A2", ManufacturerId = manufacturers[0].Id },
                new Aircraft { Name = "A3", ManufacturerId = manufacturers[1].Id },
                new Aircraft { Name = "A4", ManufacturerId = manufacturers[2].Id },
                new Aircraft { Name = "A5", ManufacturerId = manufacturers[1].Id },
            };

            _aircraftRepository = new AircraftRepositoryMock(aircraft);

            _controller = new HomeController(_aircraftRepository, _manufacturerRepository);
        }

        [Fact]
        public void Can_Paginate()
        {
            var aircraft = _aircraftRepository.GetAll().ToList();
            _controller.PageSize = 3;

            var result = (_controller.Index(currentPage: 2) as ViewResult)?.Model as HomeViewModel;

            List<Aircraft> resultAircraft = result!.Aircraft.ToList();

            Assert.Equal(2, resultAircraft.Count);
        }

        [Fact]
        public void Can_Send_Paginated_View_Model()
        {
            _controller.PageSize = 3;

            var result = (_controller.Index(currentPage: 2) as ViewResult)?.Model as HomeViewModel;

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

            var result = (_controller.Index(manufacturers[1].Id) as ViewResult)?.Model as HomeViewModel;

            List<Aircraft> resultAircraft = result!.Aircraft.ToList();

            Assert.Equal(2, resultAircraft.Count);
            Assert.Equal(manufacturers[1].Id, resultAircraft[0].ManufacturerId);
            Assert.Equal(manufacturers[1].Id, resultAircraft[1].ManufacturerId);
        }
    }
}

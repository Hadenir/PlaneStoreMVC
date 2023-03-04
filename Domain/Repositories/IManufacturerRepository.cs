﻿using PlaneStore.Domain.Entities;

namespace PlaneStore.Domain.Repositories
{
    public interface IManufacturerRepository
    {
        IQueryable<Manufacturer> Manufacturers { get; }
    }
}
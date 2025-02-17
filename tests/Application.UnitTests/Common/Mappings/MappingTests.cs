﻿using ATSS.Application.Common.Mappings;
using ATSS.Application.FlightPurchases.Commands.CreateFlightPurchase;
using ATSS.Application.Flights.Queries.GetAvailableFlights;
using ATSS.Domain.Entities;
using AutoMapper;
using NUnit.Framework;
using System;
using System.Runtime.Serialization;

namespace ATSS.Application.UnitTests.Common.Mappings
{
    public class MappingTests
    {
        private readonly IConfigurationProvider _configuration;
        private readonly IMapper _mapper;

        public MappingTests()
        {
            _configuration = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<MappingProfile>();
            });

            _mapper = _configuration.CreateMapper();
        }

        [Test]
        public void ShouldHaveValidConfiguration()
        {
            _configuration.AssertConfigurationIsValid();
        }
        
        [Test]
        [TestCase(typeof(FlightPurchase), typeof(FlightPurchaseDto))]
        [TestCase(typeof(Flight), typeof(FlightDto))]
        public void ShouldSupportMappingFromSourceToDestination(Type source, Type destination)
        {
            var instance = GetInstanceOf(source);

            _mapper.Map(instance, source, destination);
        }

        private object GetInstanceOf(Type type)
        {
            if (type.GetConstructor(Type.EmptyTypes) != null)
                return Activator.CreateInstance(type);

            // Type without parameterless constructor
            return FormatterServices.GetUninitializedObject(type);
        }
    }
}

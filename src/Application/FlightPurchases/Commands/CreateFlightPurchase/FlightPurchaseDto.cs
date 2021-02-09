using ATSS.Application.Common.Mappings;
using ATSS.Application.Flights.Queries.GetAvailableFlights;
using ATSS.Domain.Entities;
using ATSS.Domain.Enums;
using ATSS.Domain.ValueObjects;
using AutoMapper;
using System;
using System.Collections.Generic;

namespace ATSS.Application.FlightPurchases.Commands.CreateFlightPurchase
{
    public class FlightPurchaseDto: IMapFrom<FlightPurchase>
    {
        public int Id { get; set; }
        public int TenantId { get; set; }
        public string TenantName { get; set; }
        public TenantGroup TenantGroup { get; set; }
        public FlightId FlightId { get; set; }
        public string From { get; set; }
        public string To { get; set; }
        public DateTimeOffset DepartureDateTime { get; set; }
        public double Price { get; set; }
        public bool IsOnTenantBirthday { get; set; }
        public bool IsOnThursday { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<FlightPurchase, FlightPurchaseDto>()
                .ForMember(d => d.TenantId, opt => opt.MapFrom(z => z.Tenant.Id))
                .ForMember(d => d.TenantName, opt => opt.MapFrom(z => z.Tenant.Name))
                .ForMember(d => d.TenantGroup, opt => opt.MapFrom(z => z.Tenant.TenantGroup))
                .ForMember(d => d.FlightId, opt => opt.MapFrom(z => z.Flight.FlightId))
                .ForMember(d => d.From, opt => opt.MapFrom(z => z.Flight.From))
                .ForMember(d => d.To, opt => opt.MapFrom(x => x.Flight.To))
                .ForMember(d => d.IsOnTenantBirthday, opt => opt.MapFrom(z => 
                    z.DepartureDateTime.Date.Month == z.Tenant.Birthday.Date.Month &&
                    z.DepartureDateTime.Date.Day == z.Tenant.Birthday.Date.Day))
                .ForMember(d => d.IsOnThursday, opt => opt.MapFrom(z => z.DepartureDateTime.DayOfWeek == DayOfWeek.Thursday));
        }
    }
}
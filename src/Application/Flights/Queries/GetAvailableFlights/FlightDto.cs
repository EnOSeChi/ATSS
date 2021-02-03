using ATSS.Application.Common.Mappings;
using ATSS.Domain.Entities;
using ATSS.Domain.ValueObjects;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATSS.Application.Flights.Queries.GetAvailableFlights
{
    public class FlightDto: IMapFrom<Flight>
    {
        public int Id { get; set; }
        public FlightId FlightId { get; set; }
        public DateTimeOffset DepartureDateTime { get; set; }
        public double Price { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Flight, FlightDto>()
                .ForMember(d => d.Price, opt => opt.Ignore())
                .ForMember(d => d.DepartureDateTime, opt => opt.Ignore());
        }
    }
}

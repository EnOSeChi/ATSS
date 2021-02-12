using ATSS.Application.Common.Mappings;
using ATSS.Application.FlightPurchases.Commands.CreateFlightPurchase;
using ATSS.Application.PriceDiscounts.Commands.ApplyPriceDiscount;
using ATSS.Domain.Entities;
using AutoMapper;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ATSS.Application.UnitTests.PriceDiscounts.Commands.ApplyPriceDiscount
{
    /// <summary>
    /// dodanie zniżek do ceny
    /// </summary>
    public class ApplyPriceDiscountCommandTests : Testing
    {
        private MapperConfiguration _configuration;
        private IMapper _mapper;

        public ApplyPriceDiscountCommandTests()
        {
            _configuration = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<MappingProfile>();
            });

            _mapper = _configuration.CreateMapper();
        }

        // tenant z grupy A dodaje zniżke z warunkiem lot w czwartek do Afryki
        [Test]
        public async Task ShouldApplyOnThursdayToAfricaAndSaveIt()
        {
            var tenant = new Tenant
            {
                TenantGroup = Domain.Enums.TenantGroup.A
            };

            Context.Tenants.Add(tenant);

            // lot w urodziny tenanta
            var priceDiscount1 = new PriceDiscount();
            priceDiscount1.Rules.Add(new PriceDiscountRule
            {
                FieldName = "IsOnTenantBirthday",
                Value = "True"
            });
            // lot w czwartek do Afryki
            var priceDiscount2 = new PriceDiscount();
            priceDiscount2.Rules.Add(new PriceDiscountRule
            {
                FieldName = "To",
                Value = "Africa"
            });
            priceDiscount2.Rules.Add(new PriceDiscountRule
            {
                FieldName = "IsOnThursday",
                Value = "True"
            });

            Context.PriceDiscounts.AddRange(priceDiscount1, priceDiscount2);

            var flight = new Flight
            {
                DefaultPrice = 25,
                To = "Africa",
                CreatedBy = tenant,
                Thursday = true
            };

            Context.Flights.Add(flight);

            var flightPurchase = new FlightPurchase
            {
                DepartureDateTime = DateTimeOffset.Parse("2021-02-11T10:00:00Z"), // czwartek
                Flight = flight,
                Price = 25,
                Tenant = tenant
            };

            Context.FlightPurchases.Add(flightPurchase);
            Context.SaveChanges();

            var applyDiscountCommandHandler = new ApplyPriceDiscountCommandHandler(Context);
            var newPrice = await applyDiscountCommandHandler.Handle(new ApplyPriceDiscountCommand{
                FlightPurchase = _mapper.Map<FlightPurchaseDto>(flightPurchase)
            }, CancellationToken.None);

            // jedna zniżka uwzględniona
            Assert.AreEqual(20, newPrice);
            // zniżka zapisana dla tenanta grupy A
            Assert.IsTrue(Context.FlightPurchases.First().Discounts.Count > 0);
        }

        // // tenant z grupy B dodaje zniżke z warunkiem lot w czwartek do Afryki
        [Test]
        public async Task ShouldApplyOnThursdayToAfricaAndNotSaveIt()
        {
            var tenant = new Tenant
            {
                TenantGroup = Domain.Enums.TenantGroup.B
            };

            Context.Tenants.Add(tenant);

            // lot w urodziny tenanta
            var priceDiscount1 = new PriceDiscount();
            priceDiscount1.Rules.Add(new PriceDiscountRule
            {
                FieldName = "IsOnTenantBirthday",
                Value = "True"
            });
            // lot w czwartek do Afryki
            var priceDiscount2 = new PriceDiscount();
            priceDiscount2.Rules.Add(new PriceDiscountRule
            {
                FieldName = "To",
                Value = "Africa"
            });
            priceDiscount2.Rules.Add(new PriceDiscountRule
            {
                FieldName = "IsOnThursday",
                Value = "True"
            });

            Context.PriceDiscounts.AddRange(priceDiscount1, priceDiscount2);

            var flight = new Flight
            {
                DefaultPrice = 25,
                To = "Africa",
                CreatedBy = tenant,
                Thursday = true
            };

            Context.Flights.Add(flight);

            var flightPurchase = new FlightPurchase
            {
                DepartureDateTime = DateTimeOffset.Parse("2021-02-11T10:00:00Z"), // czwartek
                Flight = flight,
                Price = 25,
                Tenant = tenant
            };

            Context.FlightPurchases.Add(flightPurchase);
            Context.SaveChanges();

            var applyDiscountCommandHandler = new ApplyPriceDiscountCommandHandler(Context);
            var newPrice = await applyDiscountCommandHandler.Handle(new ApplyPriceDiscountCommand
            {
                FlightPurchase = _mapper.Map<FlightPurchaseDto>(flightPurchase)
            }, CancellationToken.None);

            // jedna zniżka uwzględniona
            Assert.AreEqual(20, newPrice);
            // zniżka zapisana dla tenanta grupy A
            Assert.IsTrue(Context.FlightPurchases.First().Discounts.Count == 0);
        }

        // tenant z grupy A dodaje dwie zniżki: czwartek do Afryki i urodziny
        [Test]
        public async Task ShouldApplyOnThursdayToAfricaAndTenantBirthdayAndSaveIt()
        {
            var tenant = new Tenant
            {
                TenantGroup = Domain.Enums.TenantGroup.A,
                Birthday = DateTimeOffset.Parse("1990-02-11T10:00:00Z")
            };

            Context.Tenants.Add(tenant);

            // lot w urodziny tenanta
            var priceDiscount1 = new PriceDiscount();
            priceDiscount1.Rules.Add(new PriceDiscountRule
            {
                FieldName = "IsOnTenantBirthday",
                Value = "True"
            });
            // lot w czwartek do Afryki
            var priceDiscount2 = new PriceDiscount();
            priceDiscount2.Rules.Add(new PriceDiscountRule
            {
                FieldName = "To",
                Value = "Africa"
            });
            priceDiscount2.Rules.Add(new PriceDiscountRule
            {
                FieldName = "IsOnThursday",
                Value = "True"
            });

            Context.PriceDiscounts.AddRange(priceDiscount1, priceDiscount2);

            var flight = new Flight
            {
                DefaultPrice = 32,
                To = "Africa",
                CreatedBy = tenant,
                Thursday = true
            };

            Context.Flights.Add(flight);

            var flightPurchase = new FlightPurchase
            {
                DepartureDateTime = DateTimeOffset.Parse("2021-02-11T10:00:00Z"), // czwartek
                Flight = flight,
                Price = 32,
                Tenant = tenant
            };

            Context.FlightPurchases.Add(flightPurchase);
            Context.SaveChanges();

            var applyDiscountCommandHandler = new ApplyPriceDiscountCommandHandler(Context);
            var newPrice = await applyDiscountCommandHandler.Handle(new ApplyPriceDiscountCommand
            {
                FlightPurchase = _mapper.Map<FlightPurchaseDto>(flightPurchase)
            }, CancellationToken.None);

            // jedna zniżka uwzględniona
            Assert.AreEqual(22, newPrice);
            // zniżka zapisana dla tenanta grupy A
            Assert.IsTrue(Context.FlightPurchases.First().Discounts.Count == 2);
        }

        // tylko jedna zniżka jest wykorzystana z powodu dolnego limitu ceny
        [Test]
        public async Task ShouldApplyOnlyOneDiscountBecauseMinPriceLimit()
        {
            var tenant = new Tenant
            {
                TenantGroup = Domain.Enums.TenantGroup.A,
                Birthday = DateTimeOffset.Parse("1990-02-11T10:00:00Z")
            };

            Context.Tenants.Add(tenant);

            // lot w urodziny tenanta
            var priceDiscount1 = new PriceDiscount();
            priceDiscount1.Rules.Add(new PriceDiscountRule
            {
                FieldName = "IsOnTenantBirthday",
                Value = "True"
            });
            // lot w czwartek do Afryki
            var priceDiscount2 = new PriceDiscount();
            priceDiscount2.Rules.Add(new PriceDiscountRule
            {
                FieldName = "To",
                Value = "Africa"
            });
            priceDiscount2.Rules.Add(new PriceDiscountRule
            {
                FieldName = "IsOnThursday",
                Value = "True"
            });

            Context.PriceDiscounts.AddRange(priceDiscount1, priceDiscount2);

            var flight = new Flight
            {
                DefaultPrice = 27,
                To = "Africa",
                CreatedBy = tenant,
                Thursday = true
            };

            Context.Flights.Add(flight);

            var flightPurchase = new FlightPurchase
            {
                DepartureDateTime = DateTimeOffset.Parse("2021-02-11T10:00:00Z"), // czwartek
                Flight = flight,
                Price = 27,
                Tenant = tenant
            };

            Context.FlightPurchases.Add(flightPurchase);
            Context.SaveChanges();

            var applyDiscountCommandHandler = new ApplyPriceDiscountCommandHandler(Context);
            var newPrice = await applyDiscountCommandHandler.Handle(new ApplyPriceDiscountCommand
            {
                FlightPurchase = _mapper.Map<FlightPurchaseDto>(flightPurchase)
            }, CancellationToken.None);

            // jedna zniżka uwzględniona
            Assert.AreEqual(22, newPrice);
            // zniżka zapisana dla tenanta grupy A
            Assert.IsTrue(Context.FlightPurchases.First().Discounts.Count == 1);
        }
    }
}

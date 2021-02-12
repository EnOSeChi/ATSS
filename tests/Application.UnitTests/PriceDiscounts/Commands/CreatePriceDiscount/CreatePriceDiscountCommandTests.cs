using ATSS.Application.PriceDiscounts.Commands.CreatePriceDiscount;
using ATSS.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ATSS.Application.UnitTests.PriceDiscounts.Commands.CreatePriceDiscount
{
    /// <summary>
    /// dodawanie zniżek
    /// </summary>
    public class CreatePriceDiscountCommandTests : Testing
    {
        [Test]
        public async Task ShouldPersistPriceDiscountWithRules()
        {
            // lot w urodziny tenanta
            var priceDiscount1 = new CreatePriceDiscountCommand();        
            priceDiscount1.Rules.Add(new PriceDiscountRule
            {
                FieldName = "IsOnTenantBirthday",
                Value = "True"
            });
            // lot w czwartek do Afryki
            var priceDiscount2 = new CreatePriceDiscountCommand();
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

            var requesHandler = new CreatePriceDiscountCommandHandler(Context);
            await requesHandler.Handle(priceDiscount1, CancellationToken.None);
            await requesHandler.Handle(priceDiscount2, CancellationToken.None);

            var priceDiscounts = Context.PriceDiscounts
                .Include(x => x.Rules)
                .ToList();

            Assert.IsTrue(priceDiscounts.Count() == 2);
            Assert.AreEqual("IsOnTenantBirthday", priceDiscounts.First().Rules.First().FieldName);
            Assert.AreEqual("True", priceDiscounts.First().Rules.First().Value);
            Assert.AreEqual("To", priceDiscounts[1].Rules.First().FieldName);
            Assert.AreEqual("Africa", priceDiscounts[1].Rules.First().Value);
        }
    }
}

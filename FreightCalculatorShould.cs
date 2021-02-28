using FancyFreight.Builders;
using FancyFreight.Models;
using FancyFreight.Repositories;
using NSubstitute;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;

namespace FancyFreight
{
    [TestFixture(TestName = "Freight calculator tests")]
    public class FreightCalculatorShould
    {
        IFreightProductRepository freightProductRepository;
        Shipment shipment;

        [SetUp]
        public void SetUp()
        {
            freightProductRepository = Substitute.For<IFreightProductRepository>();
            
            var padoren = Substitute.For<IFreightProduct>();
            padoren.Id.Returns("PADOREN");
            padoren.MaxWeightInGrams.Returns(35000);
            padoren.MaxLengthInMillimeters.Returns(2400);
            padoren.PriceWithoutVat.Returns(100.0);

            var express = Substitute.For<IFreightProduct>();
            express.Id.Returns("EXPRESS");
            express.MaxWeightInGrams.Returns(35000);
            express.MaxLengthInMillimeters.Returns(1200);
            express.MaxWidthInMillimeters.Returns(60);
            express.MaxHeightInMillimeters.Returns(60);
            express.PriceWithoutVat.Returns(100.0);

            var freightProducts = new List<IFreightProduct> { padoren, express };
            freightProductRepository.GetAll(Arg.Any<string>(), Arg.Any<Shipment>()).Returns(freightProducts);

            shipment = ShipmentBuilder
                .CreateShipment()
                .WithWeightInGrams(1000)
                .WithLengthInMillimeters(100)
                .WithWidthInMillimeters(100)
                .WithHeightInMillimeters(100)
                .Build();
        }

        [Test(Description = "Filter based on post codes")]
        [TestCase(arg: "1111", ExpectedResult = "PADOREN", TestName = "Freight should be PADOREN")]
        [TestCase(arg: "2222", ExpectedResult = "EXPRESS", TestName = "Freight should be EXPRESS")]
        public string FilterFreight(string destinationPostCode)
        {
            var validPostCodesForPadoren = new List<string> { "1111" };
            var validPostCodesForExpress = new List<string> { "2222" };

            var filters = FilterBuilder
                .CreateFilter()
                .WithCriteria("Remove invalid post codes for PADOREN", freightProduct => freightProduct.Id != "PADOREN" || validPostCodesForPadoren.Contains(destinationPostCode))
                .WithCriteria("Remove invalid post codes for EXPRESS", freightProduct => freightProduct.Id != "EXPRESS" || validPostCodesForExpress.Contains(destinationPostCode))
                .Build();

            var freightOptions = FreightCalculator
                .GetFreight(freightProductRepository)
                .ForShipment(shipment)
                .ToPostCode(destinationPostCode)
                .FilteredBy(filters)
                .Build();

            Assert.IsNotNull(freightOptions);
            Assert.AreEqual(1, freightOptions.Count);

            return freightOptions.Single().Id;
        }

        [Test]
        [TestCase("PADOREN", 1100.0, 100.0, 1000.0, null, TestName = "Freight is free as sum exceeds limit", ExpectedResult = 0.0)]
        [TestCase("EXPRESS", 500, 100.0, 1000.0, null, TestName = "Freight is not free as sum doesn't exceed limit", ExpectedResult = 100.0)]
        [TestCase("PADOREN", 50.0, 100.0, 200.0, null, TestName = "Freight is extra as sum is below limit", ExpectedResult = 200.0)]
        [TestCase("PADOREN", 700.0, 600.0, 800.0, null, TestName = "Freight is untouched as sum is within limits", ExpectedResult = 100.0)]
        [TestCase("PADOREN", 700.0, 600.0, 800.0, "Gold", TestName = "Freight is free as customer status is Gold", ExpectedResult = 0.0)]
        public double MutatedFreight(string freightId, double sum, double lowerLimit, double upperLimit, string customerStatus)
        {
            var freightIsFreeIfSumIsOverLimit = MutationBuilder
                .CreateMutation($"{freightId} is free if sum is over {upperLimit}").WithAction((freightProducts) =>
                {
                    if (sum <= upperLimit) return;

                    var freightProduct = freightProducts.FirstOrDefault(freightProduct => freightProduct.Id == freightId);
                    if (freightProduct == null) return;

                    freightProduct.PriceWithoutVat = 0.0;
                })
                .Build();

            var freightIsExtraIfSumIsUnderLimit = MutationBuilder
                .CreateMutation($"{freightId} is extra if sum is below {lowerLimit}").WithAction((freightProducts) =>
                {
                    if (sum >= lowerLimit) return;

                    var freightProduct = freightProducts.FirstOrDefault(freightProduct => freightProduct.Id == freightId);
                    if (freightProduct == null) return;

                    freightProduct.PriceWithoutVat += 100;
                })
                .Build();

            var freightIsFreeIfCustomerStatusIsGold = MutationBuilder
                .CreateMutation($"{freightId} is free if customer status is Gold").WithAction((freightProducts) =>
                {
                    if (customerStatus != "Gold") return;
                    
                    var freightProduct = freightProducts.FirstOrDefault(freightProduct => freightProduct.Id == freightId);
                    if (freightProduct == null) return;

                    freightProduct.PriceWithoutVat = 0.0;
                })
                .Build();

            var freightOptions = FreightCalculator
                .GetFreight(freightProductRepository)
                .ForShipment(shipment)
                .ToPostCode("1111")
                .MutatedBy(freightIsFreeIfSumIsOverLimit)
                .MutatedBy(freightIsExtraIfSumIsUnderLimit)
                .MutatedBy(freightIsFreeIfCustomerStatusIsGold)
                .Build();

            Assert.IsNotNull(freightOptions);
            Assert.AreEqual(2, freightOptions.Count);

            return freightOptions.FirstOrDefault(freightProduct => freightProduct.Id == freightId).PriceWithoutVat;
        }
    }
}

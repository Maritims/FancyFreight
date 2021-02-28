using FancyFreight.Builders.Interfaces;
using FancyFreight.Models;
using FancyFreight.Repositories;
using System;
using System.Collections.Generic;

namespace FancyFreight.Builders
{
    public class FreightCalculator : IFreightCalculator, IFreightCalculatorForShipment, IFreightCalculatorToPostCode
    {
        private readonly IFreightProductRepository FreightProductRepository;

        private Shipment Shipment;
        private string DestinationPostCode;
        private readonly List<Filter> Filters = new List<Filter>();
        private readonly List<Mutation> Mutations = new List<Mutation>();

        private FreightCalculator(IFreightProductRepository freightProductRepository) => FreightProductRepository = freightProductRepository;

        public static IFreightCalculatorForShipment GetFreight(IFreightProductRepository freightProductRepository) => new FreightCalculator(freightProductRepository);

        public IFreightCalculatorToPostCode ForShipment(Shipment shipment)
        {
            Shipment = shipment;
            return this;
        }

        public IFreightCalculator ToPostCode(string destinationPostCode)
        {
            if (string.IsNullOrEmpty(destinationPostCode)) throw new ArgumentException("Post code is mandatory", nameof(destinationPostCode));

            DestinationPostCode = destinationPostCode;
            return this;
        }

        public IFreightCalculator FilteredBy(Filter filter)
        {
            if (filter == null) return this;

            Filters.Add(filter);
            return this;
        }

        public IFreightCalculator MutatedBy(Mutation mutation)
        {
            if (mutation == null) return this;

            Mutations.Add(mutation);
            return this;
        }

        public List<IFreightProduct> Build()
        {
            var freightProducts = FreightProductRepository.GetAll(DestinationPostCode, Shipment);
            foreach(var filter in Filters)
            {
                freightProducts = filter.Apply(freightProducts);
            }
            foreach(var mutation in Mutations)
            {
                mutation.Apply(freightProducts);
            }
            return freightProducts;
        }
    }
}

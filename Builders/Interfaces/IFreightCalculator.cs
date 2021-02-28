using FancyFreight.Models;
using System.Collections.Generic;

namespace FancyFreight.Builders.Interfaces
{
    public interface IFreightCalculator : IBuilder<List<IFreightProduct>>
    {
        IFreightCalculator FilteredBy(Filter filter);
        IFreightCalculator MutatedBy(Mutation mutation);
    }

    public interface IFreightCalculatorForShipment
    {
        IFreightCalculatorToPostCode ForShipment(Shipment shipment);
    }

    public interface IFreightCalculatorToPostCode
    {
        IFreightCalculator ToPostCode(string destinationPostCode);
    }
}

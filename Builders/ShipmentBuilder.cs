using FancyFreight.Builders.Interfaces;
using FancyFreight.Models;

namespace FancyFreight.Builders
{
    public class ShipmentBuilder : IBuilder<Shipment>, IShipmentBuilderWithWeight, IShipmentBuilderWithLength, IShipmentBuilderWithWidth, IShipmentBuilderWithHeight
    {
        private readonly Shipment Shipment = new Shipment();

        private ShipmentBuilder() { }

        public static IShipmentBuilderWithWeight CreateShipment() => new ShipmentBuilder();

        public IShipmentBuilderWithLength WithWeightInGrams(int weightInGrams)
        {
            Shipment.WeightInGrams = weightInGrams;
            return this;
        }

        public IShipmentBuilderWithWidth WithLengthInMillimeters(int lengthInMillimeters)
        {
            Shipment.LengthInMillimeters = lengthInMillimeters;
            return this;
        }

        public IShipmentBuilderWithHeight WithWidthInMillimeters(int widthInMillimeters)
        {
            Shipment.WidthInMillimeters = widthInMillimeters;
            return this;
        }

        public IBuilder<Shipment> WithHeightInMillimeters(int heightInMillimeters)
        {
            Shipment.HeightInMillimeters = heightInMillimeters;
            return this;
        }

        public Shipment Build() => Shipment;
    }
}

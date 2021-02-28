using FancyFreight.Models;

namespace FancyFreight.Builders.Interfaces
{
    public interface IShipmentBuilderWithWeight
    {
        IShipmentBuilderWithLength WithWeightInGrams(int weightInGrams);
    }

    public interface IShipmentBuilderWithLength
    {
        IShipmentBuilderWithWidth WithLengthInMillimeters(int lengthInMillimeters);
    }

    public interface IShipmentBuilderWithWidth
    {
        IShipmentBuilderWithHeight WithWidthInMillimeters(int widthInMillimeters);
    }

    public interface IShipmentBuilderWithHeight
    {
        IBuilder<Shipment> WithHeightInMillimeters(int heightInMillimeters);
    }
}
